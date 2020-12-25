You can list steam accounts that were used to sign in the steam app on windows.
It can also detect active steam account that is currently running steam app.
Data is based on steam registry keys.

You can use ```SteamUserDefaultLoader``` class instance to access steam information.

Retrieved ```SteamUser``` contains steam id in different formats: steamID, steamID3, steamID64.
It also provides steam profile url that can be loaded in web brouser via ```SteamUser.GetSteamProfileUrl()```,
and a helper method ```SteamUser.OpenProfile()``` to open steam profile directly in the steam app.

Additionally ```SteamUserDefaultLoader.LoadSteamPersonaName(SteamUser)``` can be used to get user's current display name.


Sample usage:
```cs
ISteamUserLoader loader = new SteamUserDefaultLoader();
if(await loader.IsSteamInstalledAsync())
{
    var allUsers = await loader.LoadUsersAsync();
    var activeUser = await loader.LoadActiveUserAsync();

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
            allUsers.ElementAt(requestedUserNumber - 1).OpenProfile();
        else
            Console.WriteLine("Wrong user number");
    }
}


    
![Console window screenshot](https://github.com/mister-giga/SteamUserInfo/blob/master/Media/Screenshot_1.png?raw=true)
