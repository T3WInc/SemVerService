using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using version.client.Libraries;
using version.client.Models;


namespace version.client.ViewModels
{
    public class RepositoryViewModel : BaseViewModel
    {
        public RepositoryViewModel()
        {
            // Initialize empty collections.
            Commits = new EnhancedObservableCollection<Commit> { };
            Branches = new EnhancedObservableCollection<Branch> { };
            Tags = new EnhancedObservableCollection<Tag> { };

            // Initialize commands.
            // ? not sure there are any right now ?
        }

        public string Name { get; set; }
        public string RepositoryFullPath { get; set; }
        public bool NotOpened { get; set; }
        public int CommitsPerPage { get; set; }
        public int RecentCommitMessageCount { get; set; }
        public EnhancedObservableCollection<Commit> Commits { get; set; }
        public EnhancedObservableCollection<Branch> Branches { get; set; }
        public EnhancedObservableCollection<Tag> Tags { get; set; }

        private Branch head;

        public Branch Head
        {
            get { return head; }
            set
            {
                head = value;
                RaisePropertyChanged("Head");
            }
        }

        public void LoadEntireRepository()
        {
            //Task.Run(() =>
            //{
                using (var repo = new LibGit2Sharp.Repository(RepositoryFullPath))
                {
                    LoadTags(repo);
                    LoadBranchesAndCommits(repo);
                    var count = Branches.Count;
                }
            //});
        }

        private void LoadBranchesAndCommits(LibGit2Sharp.Repository repo = null)
        {
            var dispose = false;
            if (repo == null)
            {
                repo = new LibGit2Sharp.Repository(RepositoryFullPath);
                dispose = true;
            }

            // Small performance boosts.
            Commits.DisableNotifications();
            Branches.DisableNotifications();

            // Create commits.
            Commits.Clear();
            var commitList = new List<Commit>();
            //LibGit2Sharp.Filter filter = new LibGit2Sharp.Filter; //{ Since = repo.Branches };
            //foreach( var commit in repo.Commits.QueryBy(LibGit2Sharp.Filter { Since = repo.Branches }).Take(CommitsPerPage))
            foreach ( var commit in repo.Commits)
            {
                commitList.Add(Commit.Create(repo, commit, Tags));
            }
            Commits.AddRange(commitList);

            // Create branches.
            Branches.Clear();
            foreach (var branch in repo.Branches)
            {
                var b = Branch.Create(this, repo, branch);
                Branches.Add(b);
            }

            // Post-process branches (tips and tracking branches).
            foreach (var branch in Branches)
            {
                // Set the HEAD property if it matches.
                if (repo.Head.CanonicalName == branch.CanonicalName)
                {
                    Head = branch;
                    branch.Tip.IsHead = true;
                }

                branch.PostProcess(Branches, Commits);
            }

            // Calculate commit visual positions for each branch tree.
            foreach (var branch in Branches)
            {
                RepoUtil.IncrementCommitTreeVisualPositionsRecursively(branch.Tip);
            }

        }

        private void LoadTags(LibGit2Sharp.Repository repo = null)
        {
            var dispose = false;
            if (repo == null)
            {
                repo = new LibGit2Sharp.Repository(RepositoryFullPath);
                dispose = true;
            }

            Tags.DisableNotifications();
            Tags.Clear();

            foreach (var tag in repo.Tags)
            {
                var t = Tag.Create(repo, tag);

                if (t.HasCommitAsTarget)
                    Tags.Add(t);
            }

        }
    }
}
