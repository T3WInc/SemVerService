using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using version.client.ViewModels;

namespace version.client.Models
{
    public class Branch
    {
        public string CanonicalName { get; set; }
        public string Name { get; set; }
        public Branch TrackedBranch { get; set; }
        private string TrackedBranchName { get; set; }
        public Commit Tip { get; set; }
        public string TipHash { get; set; }
        public bool IsRemote { get; set; }
        public bool IsTracking { get; set; }
        public int? AheadBy { get; set; }
        public int? BehindBy { get; set; }
        public int RightMostVisualPosition { get; set; }
        private RepositoryViewModel repositoryViewModel { get; set; }

        public static Branch Create(RepositoryViewModel repositoryViewModel, LibGit2Sharp.Repository repo, LibGit2Sharp.Branch branch)
        {
            Branch newBranch = new Branch
            {
                CanonicalName = branch.CanonicalName,
                Name = branch.FriendlyName,
                IsRemote = branch.IsRemote,
                IsTracking = branch.IsTracking,
                TipHash = branch.Tip.Sha.ToString(),
                AheadBy = branch.TrackingDetails.AheadBy,
                BehindBy = branch.TrackingDetails.BehindBy,
                TrackedBranchName = branch.TrackedBranch != null ? branch.TrackedBranch.FriendlyName : null
            };

            newBranch.repositoryViewModel = repositoryViewModel;

            // Loop through the first N commits and let them know about me.
            foreach (LibGit2Sharp.Commit branchCommit in branch.Commits) //.Take(repositoryViewModel.CommitsPerPage))
            {
                Commit commit = repositoryViewModel.Commits.Where(c => c.Hash == branchCommit.Sha.ToString()).FirstOrDefault();

                if (commit != null)
                {
                    commit.Branches.Add(newBranch); // Let the commit know that I am one of her branches.

                    if (newBranch.TipHash == commit.Hash)
                    {
                        commit.DisplayTags.Add(branch.FriendlyName);
                        newBranch.Tip = commit;
                    }
                }
            }

            return newBranch;
        }

        public void PostProcess(ObservableCollection<Branch> branches, ObservableCollection<Commit> commits)
        {
            // Set the TrackedBranch to be an actual Branch model.
            TrackedBranch = branches.Where(b => b.Name == TrackedBranchName).FirstOrDefault();

            // Set the Tip to be an actual Commit model
            Tip = commits.Where(c => c.Hash == TipHash).FirstOrDefault();
        }
    }
}
