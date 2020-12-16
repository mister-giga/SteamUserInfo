You can list every steam account (user id in all formats) that was used to sign in the steam app on windows.
It can also detect active steam account that is currently running steam app.
Data is based on steam registry keys.

Sample usage:

    if(SteamUser.IsSteamAvailable())
    {
        foreach(var user in SteamUser.EnumerateSeteamUsers(true))
            Console.WriteLine($"{user.Id} >> {user.Id3Formatted} >> {user.Id64} >> Active: {user.IsActive}");
        Console.WriteLine(new string('-', 100));
        if(SteamUser.TryGetActiveUser(out var activeUser))
            Console.WriteLine($"{activeUser.Id} >> {activeUser.Id3Formatted} >> {activeUser.Id64} >> Active: {activeUser.IsActive}");
    }

output:

    STEAM_0:1:526041363 >> U:1:1052082727 >> 76561199012348455 >> Active: False
    STEAM_0:0:53039380 >> U:1:106078760 >> 76561198066344488 >> Active: True
    -----------------------------------------------------------------------------
    STEAM_0:0:53039380 >> U:1:106078760 >> 76561198066344488 >> Active: True
    
