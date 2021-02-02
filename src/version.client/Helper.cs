using System;
using System.Collections.Generic;
using System.Text;
using version.client.Libraries;
using version.client.Models;

namespace version.client
{
    public static class Helper
    {
        public static string [] GetTips(EnhancedObservableCollection<Branch> Branches)
        {
            string result = string.Empty;
            string backup = string.Empty;
            foreach(var branch in Branches)
            {
                // We want to ignore the local branches...
                if (branch.IsRemote == true)
                {
                    if (branch.Tip != null)
                    {
                        // We just want the current active branch
                        if (branch.Tip.IsHead == true)
                        {
                            if (result == string.Empty)
                            {
                                result = branch.Name;
                            }
                            else
                            {
                                result = result + "," + branch.Name;
                            }
                        }
                    }
                }
                else
                {
                    // Just in case we do not have a remote that is not the head get a backup
                    // if this one is local but has a link to the tip...
                    if (branch.Tip != null)
                    {
                        if (branch.Tip.IsHead == true)
                        {
                            backup = branch.Name;
                        }
                    }
                }
            }

            if (result == string.Empty)
            {
                result = backup;
            }
            return result.Split(',');
        }
    }
}
