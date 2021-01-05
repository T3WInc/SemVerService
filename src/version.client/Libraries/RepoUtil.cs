using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using version.client.Models;

namespace version.client.Libraries
{
    public class RepoUtil
    {
        //public static LibGit2Sharp.TreeChanges GetTreeChangesForCommit(LibGit2Sharp.Repository repo, Commit commit)
        //{
        //    // Retrieve the Tree for this commit.
        //    LibGit2Sharp.Tree thisTree = ((LibGit2Sharp.Commit) repo.Lookup(commit.ObjectId)).Tree;

        //    // Retrieve the Tree for the previous commit (parent).
        //    LibGit2Sharp.Tree parentTree = ((LibGit2Sharp.Commit)repo.Lookup(commit.Parents.ElementAt(0).ObjectId)).Tree;

        //    return repo.Diff.Compare(parentTree, thisTree);
        //}

        public static List<Branch> GetBranchesAroundCommit(Commit commit, EnhancedObservableCollection<Branch> branches)
        {
            List<Branch> list = new List<Branch>();

            // Loop through all branches and determine if they are around the spedified commit.
            foreach (Branch branch in branches)
            {
                // Tip has to be found and in case multiple branches share the tree, get rid of the others -- messes up visual position counting.
                if (branch.Tip == null || list.Any(b => branch.Tip.Branches.Contains(b)) || list.Any(b => b.Tip.Branches.Contains(branch)))
                    continue;

                // The branch's tip must be newer/same than the commit.
                if (branch.Tip.Date >= commit.Date)
                {
                    list.Add(branch);
                }
                else
                {
                    // If there's a branch with a tip commit older than commit.Date, then it's around this commit if they don't share a single branch.
                    bool foundThisBranch = branch.Tip.Branches.Any(b => commit.Branches.Contains(b));

                    if (foundThisBranch == false)
                        list.Add(branch);
                }
            }

            return list;
        }

        public static void IncrementCommitTreeVisualPositionsRecursively(Commit commit, int level = 0)
        {
            // The visual position for this commit has already been calculated or the commit does not exist.
            if (commit == null || commit.VisualPosition != -1)
                return;

            // We have reached a commit with multiple children, we only continue if this commit is the "left most chain".
            if (commit.Children.Count > 1 && level > 0)
                return;

            // Update commit's visual position.
            commit.VisualPosition = level;

            // Update commit's branches' visual positions if needed.
            commit.Branches.ForEach(b =>
            {
                if (b.RightMostVisualPosition < level)
                   b.RightMostVisualPosition = level;
            });

            if (commit.IsMergeCommit())
            {
                int i = 0;
                foreach (Commit parentCommit in commit.Parents)
                {
                    RepoUtil.IncrementCommitTreeVisualPositionsRecursively(parentCommit, level + i);

                    i++;
                }
            }
            else if (commit.Parents.Count > 0)
            {
                RepoUtil.IncrementCommitTreeVisualPositionsRecursively(commit.Parents.ElementAt(0));
            }
            
        }

        internal static List<Branch> GetBranchesAroundCommit(Commit commit, ObservableCollection<Branch> branches)
        {
            throw new NotImplementedException();
        }
    }
}
