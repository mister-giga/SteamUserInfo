using SteamUserInfo;
using System;
using System.Linq;

namespace ConsoleApp11
{
    class Program
    {
        static void Main(string[] args)
        {
            if(SteamUser.IsSteamAvailable())
            {
                foreach(var user in SteamUser.EnumerateSeteamUsers(true))
                    Console.WriteLine($"{user.Id} >> {user.Id3Formatted} >> {user.Id64} >> Active: {user.IsActive}");

                Console.WriteLine(new string('-', 100));

                if(SteamUser.TryGetActiveUser(out var activeUser))
                    Console.WriteLine($"{activeUser.Id} >> {activeUser.Id3Formatted} >> {activeUser.Id64} >> Active: {activeUser.IsActive}");
            }
            else
            {
                Console.WriteLine("Steam is not available");
            }
        }
    }
}
