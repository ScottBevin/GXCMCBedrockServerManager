using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore
{
    public enum ServerPermissions
    {
        Default,
        Visitor,
        Member,
        Operator,
    }

    public class ServerEnumHelpers
    {
        public static ServerPermissions ServerPermissionFromString(string s)
        {
            if(s == "visitor") { return ServerPermissions.Visitor; }
            if(s == "member") { return ServerPermissions.Member; }
            if(s == "operator") { return ServerPermissions.Operator; }

            return ServerPermissions.Default;
        }

        public static string ServerPermissionToString(ServerPermissions sp)
        {
            switch(sp)
            {
                case ServerPermissions.Visitor: { return "visitor"; }
                case ServerPermissions.Member: { return "member"; }
                case ServerPermissions.Operator: { return "operator"; }
            }

            return "default";
        }
    }
}
