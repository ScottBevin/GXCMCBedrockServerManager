using System.Collections.Generic;

namespace GXCMCBedrockServerManagerCore.Files
{
    public class PermissionsFile
    {
        static string PermissionsFileName { get { return "Permissions"; } }
        public struct Permission
        {
            // Lowercase names to match server json schema
            public string permission { get; set; }
            public string xuid { get; set; }
        }

        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public static PermissionsFile Load(string path)
        {
            PermissionsFile newfile = new PermissionsFile();
            newfile.Permissions = Utils.JsonFile<List<Permission>>.LoadJsonFile(path, PermissionsFileName, true);

            return newfile;
        }
        public bool Save(string path)
        {
            return Utils.JsonFile<List<Permission>>.SaveJsonFile(Permissions, path, PermissionsFileName, true);
        }
    }
}
