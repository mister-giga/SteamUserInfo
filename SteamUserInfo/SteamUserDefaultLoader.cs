using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamUserInfo
{
    public class SteamUserDefaultLoader : ISteamUserLoader
    {
        public SteamUserDefaultLoader()
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException($"{typeof(SteamUserDefaultLoader)} is supported only on Windows platform");
        }

        public virtual Task<IReadOnlyCollection<SteamUser>> LoadUsersAsync() => Task.FromResult<IReadOnlyCollection<SteamUser>>(ListUserId3s().Select(x => new SteamUser(x)).ToArray());

        public virtual Task<SteamUser?> LoadActiveUserAsync()
        {
            var activeUserId = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam\ActiveProcess", "ActiveUser", default);
            SteamUser? result = null;
            if(activeUserId is int id && id > 0)
                result = new SteamUser(id);
            return Task.FromResult(result);
        }
        public virtual Task<bool> IsSteamInstalledAsync() => Task.FromResult(IsSteamAvailable());

        protected virtual RegistryKey SteamRegistryKey() => Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("Valve")?.OpenSubKey("Steam");
        protected virtual bool IsSteamAvailable() => SteamRegistryKey() != null;

        protected virtual IEnumerable<int> ListUserId3s()
        {
            foreach(var user in SteamRegistryKey().OpenSubKey("Users").GetSubKeyNames())
                yield return int.Parse(user);
        }

        readonly protected static Dictionary<int, string> PersonaNameCache = new Dictionary<int, string>();
      

        public async Task<string> LoadSteamPersonaName(SteamUser user, bool enableCache = true)
        {
            string personaName = null;

            if(enableCache && PersonaNameCache.ContainsKey(user.Id3))
                personaName = PersonaNameCache[user.Id3];
            else
            {
                var profileUrl = user.GetSteamProfileUrl();

                using(var client = new HttpClient())
                {
                    var response = await client.GetAsync(profileUrl);

                    using(var responseStram = await response.Content.ReadAsStreamAsync())
                    using(var streamReader = new StreamReader(responseStram))
                    {
                        string line;
                        while((line = streamReader.ReadLine()) != null)
                        {
                            if(Regex.IsMatch(line, "<title>Steam Community :: .*</title>"))
                            {
                                var startIndex = line.IndexOf("<title>") + 26;
                                var endIndex = line.IndexOf("</title>");
                                personaName = line.Substring(startIndex, endIndex - startIndex);
                                break;
                            }
                        }
                    }

                    if(!string.IsNullOrWhiteSpace(personaName))
                        PersonaNameCache[user.Id3] = personaName;
                }
            }

            return personaName;
        }
    }
}
