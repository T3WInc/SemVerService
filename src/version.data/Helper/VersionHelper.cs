using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t3winc.version.data.Helper
{
    public static class VersionHelper
    {
        internal static string GetSuffix(string branch)
        {
            var starter = branch.Split("/");
            switch (starter[0].ToLower())
            {
                case "feature":
                    return "alpha";
                case "bug":
                    return "beta";
                case "major":
                    return "torn";
                case "pull":
                    return "close";
                case "master":
                    return "";
                default:
                    return "not valid";
            }
        }
    }
}
