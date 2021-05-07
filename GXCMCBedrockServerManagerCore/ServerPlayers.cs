using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GXCMCBedrockServerManagerCore
{
    public class ServerPlayers
    {
        public class ServerPlayersSettings
        {
            public string BanPlayerMessage { get; set; } = "";
            public string BannedPlayerKickReason { get; set; } = "";
            public string PlayerFirstJoinedWelcomeMessage { get; set; } = "";
        }

        public class Player
        {
            public string Name { get; set; } = "";
            public string XUID { get; set; } = "";
            public bool IsWhitelisted { get; set; } = false;
            public bool IsBanned { get; set; } = false;
            public bool IgnoresPlayerLimit { get; set; } = false;
            public ServerPermissions Permissions { get; set; } = ServerPermissions.Default;
            public DateTime FirstJoined { get; set; } = new DateTime(0);
            public DateTime LastJoined { get; set; }

            [JsonIgnore]
            public bool IsOnline { get; set; }
        }

        public class PlayersFile : Utils.JsonFile<PlayersFile>
        {
            static string PlayersFileName { get { return "KnownPlayers"; } }

            public List<Player> Players { get; set; } = new List<Player>();

            public static PlayersFile Load(string path)
            {
                return LoadJsonFile(path, PlayersFileName, true);
            }

            public bool Save(string path)
            {
                return SaveJsonFile(this, path, PlayersFileName, true);
            }
        }

        public List<Player> Players { get; private set; }

        ServerInstance Server { get; set; }

        public bool Initialise(ServerInstance parent)
        {
            Server = parent;

            if(!Reload())
            {
                return false;
            }

            // Update all relevent files before we boot the server
            return SaveAll();
        }

        public bool Reload()
        {
            PlayersFile playersfile = PlayersFile.Load(Server.ServerPath);

            if (playersfile == null)
            {
                Server.Log.LogError("Failed to load players file");
                return false;
            }

            Players = playersfile.Players;

            // Read the permissions file and update eny entries to ensure our cache is correct

            Files.PermissionsFile permissionsFile = Files.PermissionsFile.Load(Server.ServerPath);

            if (permissionsFile != null)
            {
                foreach (var permissionEntry in permissionsFile.Permissions)
                {
                    Player player = Players.Find(P => (P.XUID == permissionEntry.xuid));

                    if (player != null)
                    {
                        player.Permissions = ServerEnumHelpers.ServerPermissionFromString(permissionEntry.permission);
                    }
                }
            }

            // Read the whitelist file and update any entries

            Files.WhitelistFile whitelistFile = Files.WhitelistFile.Load(Server.ServerPath);

            if (whitelistFile != null)
            {
                foreach (var whitelistEntry in whitelistFile.Whitelist)
                {
                    Player player = Players.Find(P => (P.Name == whitelistEntry.name));

                    if (player != null)
                    {
                        player.IgnoresPlayerLimit = whitelistEntry.ignoresPlayerLimit;
                        player.IsWhitelisted = true;

                        if (string.IsNullOrEmpty(player.XUID))
                        {
                            player.XUID = whitelistEntry.xuid;
                        }
                    }
                }
            }

            return true;
        }

        bool SaveAll()
        {
            bool allOK = true;

            PlayersFile playersFile = new PlayersFile();
            playersFile.Players = Players;
            allOK &= playersFile.Save(Server.ServerPath);

            Files.PermissionsFile permissionsFile = new Files.PermissionsFile();
            Files.WhitelistFile whitelistFile = new Files.WhitelistFile();

            foreach (Player player in Players)
            {
                if(player.Permissions != ServerPermissions.Default && player.Permissions != Server.ServerProperties.DefaultPlayerPermissionLevel)
                {
                    permissionsFile.Permissions.Add(new Files.PermissionsFile.Permission()
                    {
                        permission = ServerEnumHelpers.ServerPermissionToString(player.Permissions),
                        xuid = player.XUID
                    }); ;
                }

                if(player.IsWhitelisted)
                {
                    whitelistFile.Whitelist.Add(new Files.WhitelistFile.WhitelistEntry()
                    {
                        ignoresPlayerLimit = player.IgnoresPlayerLimit,
                        name = player.Name,
                        xuid = player.XUID,
                    });
                }
            }

            allOK &= permissionsFile.Save(Server.ServerPath);
            allOK &= whitelistFile.Save(Server.ServerPath);

            return allOK;
        }

        public void SaveAndNotifyServer()
        {
            if(SaveAll())
            {
                Server.RunCommand("whitelist reload");
                Server.RunCommand("permission reload");
            }
        }

        public Player FindOrRegisterNewPlayer(string name)
        {
            Player player = FindPlayerByName(name);

            if (player == null)
            {
                player = new Player()
                {
                    Name = name
                };

                Players.Add(player);

                SaveAll();
            }

            return player;
        }

        public Player FindPlayerByName(string name)
        {
            return Players.Find(P => (P.Name == name));
        }

        public string FormatPlayerMessage(Player player, string message)
        {
            if(!string.IsNullOrEmpty(message))
            {
                message = message.Replace("[Player.Name]", player.Name);
            }

            return message;
        }

        public void KickPlayer(Player player, string reason)
        {
            Server.RunCommand($"kick {player.Name} {reason}");
        }
        public void BanPlayer(Player player)
        {
            if(!player.IsBanned)
            {
                player.IsBanned = true;
                SaveAll();

                Server.SendServerMessage(FormatPlayerMessage(player, Server.ServerSettings.Players.BanPlayerMessage));
            }

            if(player.IsOnline)
            {
                KickPlayer(player, FormatPlayerMessage(player, Server.ServerSettings.Players.BannedPlayerKickReason));
            }
        }

        public void NotifyPlayerJoinedServer(Player player)
        {
            player.IsOnline = true;

            if(player.IsBanned)
            {
                // todo, this doesn't work, need to delay the action.
                KickPlayer(player, FormatPlayerMessage(player, Server.ServerSettings.Players.BannedPlayerKickReason));
            }

            player.LastJoined = DateTime.Now;

            if(player.FirstJoined == new DateTime(0))
            {
                player.FirstJoined = DateTime.Now;
                Server.SendServerMessage(FormatPlayerMessage(player, Server.ServerSettings.Players.PlayerFirstJoinedWelcomeMessage));
            }
        }

        public void NotifyPlayerLeftServer(Player player)
        {
            player.IsOnline = false;
        }
    }
}
