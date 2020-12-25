using SteamUserInfo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ISteamUserLoader loader = new SteamUserDefaultLoader();
            if(await loader.IsSteamInstalledAsync())
            {
                var allUsers = await loader.LoadUsersAsync();
                var activeUser = await loader.LoadActiveUserAsync();

                Console.WriteLine("Users signed in on this device: ");
                int index = 0;
                foreach(var user in allUsers)
                {
                    bool isActive = activeUser == user;
                    Console.WriteLine($"{++index}) id: {user.Id}, id3: {user.Id3Formatted}, id64: {user.Id64}, persona name: {await loader.LoadSteamPersonaName(user)}, is active: {isActive}");
                }

                if(index > 0)
                {
                    Console.Write("Enter user number to open steam account: ");
                    if(int.TryParse(Console.ReadLine(), out var requestedUserNumber) && requestedUserNumber <= index && requestedUserNumber > 0)
                    {
                        allUsers.ElementAt(requestedUserNumber - 1).OpenProfile();
                    }
                    else
                    {
                        Console.WriteLine("Wrong user number");
                    }
                }
            }
            else
            {
                Console.WriteLine("Steam is not available");
            }
        }
    }
}
