using GXCMCBedrockServerManagerCore.Utils;
using System;
using System.IO;

namespace GXCMCBedrockServerManagerCore.Files
{
    public class ServerPropertiesFile
    {
        public string ServerName = "My Server";
        public string GameMode = "creative";
        public bool ForceGameMode = false;
        public string Difficulty = "easy";
        public bool AllowCheats = false;
        public uint MaxPlayers = 10;
        public bool OnlineMode = true;
        public bool WhiteList = true;
        public UInt16 ServerPort = 19132;
        public UInt16 ServerPortV6 = 19133;
        public uint ViewDistance = 32;
        public UInt16 TickDistance = 4;
        public uint PlayerIdleTimeout = 30;
        public uint MaxThreads = 8;
        public string LevelName = "My Level";
        public string LevelSeed = "";
        public ServerPermissions DefaultPlayerPermissionLevel = ServerPermissions.Member;
        public bool TexturePackRequired = false;
        public bool ContentLogFileEnabled = false;
        public UInt16 CompressionThreshold = 1;
        public string ServerAuthorativeMovement = "server-auth";
        public uint PlayerMovementScoreThreshold = 20;
        public float PlayerMovementDistanceThreshold = 0.3f;
        public uint PlayerMovementDurationThresholdMS = 500;
        public bool CorrectPlayerMovement = true;
        public bool ServerAuthorativeBlockBreaking = true;
        public bool FileHasErrors { get; private set; } = false;

        public static ServerPropertiesFile Load(string path)
        {
            string serverPropsPath = Path.Combine(path, "server.properties");

            PropertiesFile propsFile = PropertiesFile.Readfile(serverPropsPath);

            if (propsFile != null)
            {
                ServerPropertiesFile spf = new ServerPropertiesFile();

                bool allFound = true;

                allFound &= propsFile.TryGetValue("server-name", ref spf.ServerName);
                allFound &= propsFile.TryGetValue("gamemode", ref spf.GameMode);
                allFound &= propsFile.TryGetValue("force-gamemode", ref spf.ForceGameMode);
                allFound &= propsFile.TryGetValue("difficulty", ref spf.Difficulty);
                allFound &= propsFile.TryGetValue("allow-cheats", ref spf.AllowCheats);
                allFound &= propsFile.TryGetValue("max-players", ref spf.MaxPlayers);
                allFound &= propsFile.TryGetValue("online-mode", ref spf.OnlineMode);
                allFound &= propsFile.TryGetValue("white-list", ref spf.WhiteList);
                allFound &= propsFile.TryGetValue("server-port", ref spf.ServerPort);
                allFound &= propsFile.TryGetValue("server-portv6", ref spf.ServerPortV6);
                allFound &= propsFile.TryGetValue("view-distance", ref spf.ViewDistance);
                allFound &= propsFile.TryGetValue("tick-distance", ref spf.TickDistance);
                allFound &= propsFile.TryGetValue("player-idle-timeout", ref spf.PlayerIdleTimeout);
                allFound &= propsFile.TryGetValue("max-threads", ref spf.MaxThreads);
                allFound &= propsFile.TryGetValue("level-name", ref spf.LevelName);
                allFound &= propsFile.TryGetValue("level-seed", ref spf.LevelSeed);

                string permissions = "";
                allFound &= propsFile.TryGetValue("default-player-permission-level", ref permissions);
                spf.DefaultPlayerPermissionLevel = ServerEnumHelpers.ServerPermissionFromString(permissions);

                allFound &= propsFile.TryGetValue("texturepack-required", ref spf.TexturePackRequired);
                allFound &= propsFile.TryGetValue("content-log-file-enabled", ref spf.ContentLogFileEnabled);
                allFound &= propsFile.TryGetValue("compression-threshold", ref spf.CompressionThreshold);
                allFound &= propsFile.TryGetValue("server-authoritative-movement", ref spf.ServerAuthorativeMovement);
                allFound &= propsFile.TryGetValue("player-movement-score-threshold", ref spf.PlayerMovementScoreThreshold);
                allFound &= propsFile.TryGetValue("player-movement-distance-threshold", ref spf.PlayerMovementDistanceThreshold);
                allFound &= propsFile.TryGetValue("player-movement-duration-threshold-in-ms", ref spf.PlayerMovementDurationThresholdMS);
                allFound &= propsFile.TryGetValue("correct-player-movement", ref spf.CorrectPlayerMovement);
                allFound &= propsFile.TryGetValue("server-authoritative-block-breaking", ref spf.ServerAuthorativeBlockBreaking);

                bool anyErrorsFound = false;

                anyErrorsFound |= (spf.ServerPort < 1);
                anyErrorsFound |= (spf.ServerPortV6 < 1);
                anyErrorsFound |= (spf.ViewDistance < 5);
                anyErrorsFound |= (spf.TickDistance < 4 || spf.TickDistance > 12);

                spf.FileHasErrors = !allFound || anyErrorsFound;

                return spf;
            }

            return null;
        }
    }
}
