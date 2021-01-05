using System;
using System.IO;
using System.Reflection;
using version.client.Models;
using version.client.ViewModels;

namespace version.client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RepositoryViewModel repo = new RepositoryViewModel();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = @"c:\repo\Sample\.git";
            repo.RepositoryFullPath = path;
            repo.LoadEntireRepository();
            Console.WriteLine($"Total Number of Commits: {repo.Commits.Count}");
            Console.WriteLine($"Total Number of Branches: {repo.Branches.Count}");
            Console.WriteLine($"Total Number of Tags: {repo.Tags.Count}");
            Console.WriteLine($"The Branches:");
            foreach(Branch branch in repo.Branches)
            {
                Console.WriteLine($"{branch.Name} Tracking: {branch.IsTracking} Remote: {branch.IsRemote} Tip: {branch.Tip.IsHead}");
            }
            
            Console.ReadKey();
        }
    }
}
