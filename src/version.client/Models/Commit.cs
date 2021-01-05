using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using version.client.Libraries;

namespace version.client.Models
{
    public class Commit
    {
        public string AuthorEmail { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public List<string> DisplayTags { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }
        public string Hash { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Branch> BranchesAround { get; set; }
        public List<string> ParentHashes { get; set; }
        public int ParentCount { get; set; }
        public List<Commit> Parents { get; set; }
        public int VisualPosition { get; set; }
        public bool IsHead { get; set; }
        public List<Commit> Children { get; set; }
        public LibGit2Sharp.ObjectId ObjectId { get; set; }

        public static Commit Create(LibGit2Sharp.Repository repo, LibGit2Sharp.Commit commit, EnhancedObservableCollection<Tag> tags)
        {
            var c = new Commit();

            // Process Tags (Git tags to display next to the commit description).
            var commitTags = new ObservableCollection<Tag>();
            foreach (var tag in tags)
            {
                if (tag.TargetSha == commit.Sha)
                {
                    commitTags.Add(tag);
                    tag.Target = c;
                }
            }

            // Process display tags.
            var displayTags = new List<string>();
            if (repo.Head.Tip == commit)
                displayTags.Add("HEAD");

            // Process ParentHashes.
            var parentHashes = new List<string>();
            foreach (var parentCommit in commit.Parents)
            {
                parentHashes.Add(parentCommit.Sha);
            }

            // Set properties.
            c.AuthorEmail = commit.Author.Email;
            c.AuthorName = commit.Author.Name;
            c.Date = commit.Author.When.DateTime;
            c.Description = commit.Message;
            c.ShortDescription = commit.MessageShort;
            c.DisplayTags = displayTags;
            c.Branches = new List<Branch>();
            c.Tags = commitTags;
            c.Hash = commit.Sha;
            c.ParentHashes = parentHashes;
            //c.ParentCount = commit.ParentsCount;
            c.Parents = new List<Commit>();
            c.ObjectId = commit.Id;
            c.VisualPosition = -1; // -1 means it's not yet calculated.
            c.Children = new List<Commit>();

            return c;
        }

        public bool IsMergeCommit()
        {
            return ParentCount > 1;
        }

        /// <summary>
        /// Post-process the commit.  This means that we set up the parent object relationship.
        /// </summary>
        /// <param name="commits"></param>
        /// <param name="branches"></param>
        public void PostProcess(EnhancedObservableCollection<Commit> commits, EnhancedObservableCollection<Branch> branches)
        {
            // Set Parents.
            if (ParentCount > 0)
            {
                foreach (string hash in ParentHashes)
                {
                    Commit parentCommit = commits.Where(c => c.Hash == hash).FirstOrDefault();

                    if (parentCommit != null)
                    {
                        Parents.Add(parentCommit);
                        parentCommit.Children.Add(this);
                    }
                }
            }

            BranchesAround = RepoUtil.GetBranchesAroundCommit(this, branches);
        }
    }
}
