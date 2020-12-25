using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamUserInfo
{
    public readonly struct SteamUser : IEquatable<SteamUser>
    {
        public string Id { get; }
        public int Id3 { get; }
        public string Id3Formatted { get; }
        public long Id64 { get; }

        public SteamUser(int steamId3)
        {
            Id3 = steamId3;
            Id64 = Id3_To_Id64(steamId3);
            Id = Id3_To_Id(steamId3);
            Id3Formatted = $"U:1:{steamId3}";
        }

       
        static long Id3_To_Id64(int id3) => 76561197960265728L + id3;
        static string Id3_To_Id(int id3) => $"STEAM_0:{id3%2}:{id3/2}";

        public string GetSteamProfileUrl() => $"https://steamcommunity.com/profiles/{Id64}";
        public void OpenProfile() => Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = $"steam://openurl/{GetSteamProfileUrl()}" });


        public override bool Equals(object obj) => obj is SteamUser user && Equals(user);
        public bool Equals(SteamUser other) => Id3 == other.Id3;

        public override int GetHashCode() => -618296399 + Id3.GetHashCode();

        public static bool operator ==(SteamUser left, SteamUser right) => left.Equals(right);
        public static bool operator !=(SteamUser left, SteamUser right) => !(left == right);
    }
}
