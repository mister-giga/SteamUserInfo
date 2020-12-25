using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SteamUserInfo
{
    public interface ISteamUserLoader
    {
        Task<IReadOnlyCollection<SteamUser>> LoadUsersAsync();
        Task<SteamUser?> LoadActiveUserAsync();
        Task<bool> IsSteamInstalledAsync();
        Task<string> LoadSteamPersonaName(SteamUser user, bool enableCache = true);
    }
}
