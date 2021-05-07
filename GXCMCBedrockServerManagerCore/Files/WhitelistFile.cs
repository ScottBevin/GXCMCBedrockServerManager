using System.Collections.Generic;

namespace GXCMCBedrockServerManagerCore.Files
{
    class WhitelistFile
    {
        static string WhitelistFileName { get { return "whitelist"; } }

        public struct WhitelistEntry
        {
            // Lowercase names to match server json schema
            public bool ignoresPlayerLimit { get; set; }
            public string name { get; set; }
            public string xuid { get; set; }
        }

        public List<WhitelistEntry> Whitelist { get; set; } = new List<WhitelistEntry>();

        public static WhitelistFile Load(string path)
        {
            WhitelistFile newfile = new WhitelistFile();
            newfile.Whitelist = Utils.JsonFile<List<WhitelistEntry>>.LoadJsonFile(path, WhitelistFileName, false);

            return newfile;
        }
        public bool Save(string path)
        {
            return Utils.JsonFile<List<WhitelistEntry>>.SaveJsonFile(Whitelist, path, WhitelistFileName, true);
        }
    }
}
