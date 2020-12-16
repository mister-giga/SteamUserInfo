using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SteamUserInfo
{
    public readonly struct SteamUser : IEquatable<SteamUser>
    {
        public string Id { get; }
        public int Id3 { get; }
        public string Id3Formatted { get; }
        public bool IsActive { get; }
        public long Id64 { get; }

        public SteamUser(int steamId3, bool isActive)
        {
            Id3 = steamId3;
            Id64 = Id3_To_Id64(steamId3);
            IsActive = isActive;
            Id = Id3_To_Id(steamId3);
            Id3Formatted = $"U:1:{steamId3}";
        }

        public static IEnumerable<SteamUser> EnumerateSeteamUsers(bool flagActiveUser)
        {
            CheckPlatform();
            var activeUserId = flagActiveUser ? GetSteamId3OfActiveUserFromRegistry(flagActiveUser) : 0;
            foreach(var userId3 in ListUserId3s())
                yield return new SteamUser(userId3, userId3 == activeUserId);
        }

        public static bool TryGetActiveUser(out SteamUser steamUser)
        {
            CheckPlatform();
            var activeUserId = GetSteamId3OfActiveUserFromRegistry(true);
            steamUser = new SteamUser(activeUserId, activeUserId > 0);
            return steamUser.IsActive;
        }

        static int GetSteamId3OfActiveUserFromRegistry(bool throwIfNone)
        {
            var id = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam\ActiveProcess", "ActiveUser", default);
            if(!(id is int idInt))
                idInt = 0;
            if(throwIfNone && idInt <= 0)
                throw new Exception("There is no active user");
            return idInt;
        }

        static void CheckPlatform()
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException("Functionality is supported only on Windows platform");
        }

        static RegistryKey SteamRegistryKey() => Registry.CurrentUser.OpenSubKey("Software")?.OpenSubKey("Valve")?.OpenSubKey("Steam");
        public static bool IsSteamAvailable() => SteamRegistryKey() != null;

        static IEnumerable<int> ListUserId3s()
        {
            foreach(var user in SteamRegistryKey().OpenSubKey("Users").GetSubKeyNames())
                yield return int.Parse(user);
        }

        static long Id3_To_Id64(int id3) => 76561197960265728L + id3;
        static string Id3_To_Id(int id3) => $"STEAM_0:{id3%2}:{id3/2}";


        public override bool Equals(object obj) => obj is SteamUser user && Equals(user);
        public bool Equals(SteamUser other) => Id3 == other.Id3;

        public override int GetHashCode() => -618296399 + Id3.GetHashCode();

        public static bool operator ==(SteamUser left, SteamUser right) => left.Equals(right);
        public static bool operator !=(SteamUser left, SteamUser right) => !(left == right);
    }
}
