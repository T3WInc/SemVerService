using System;
using System.IO;
using System.Reflection;
using version.client.Models;
using version.client.ViewModels;
using RestSharp;
using RestSharp.Authenticators;

namespace version.client
{
    class Program
    {
        static string key = "d4756207-07ac-4ffa-beb0-3149ebe0fe19";
        static string branch = "";

        public static void Main(string[] args)
        {
            var semver = Environment.GetEnvironmentVariable("SemVerService", EnvironmentVariableTarget.User);
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("Version-Client has Required Arguments");
                Console.WriteLine("*************************************");
                Console.WriteLine("1. path to the .git repository");
                Console.WriteLine("2. project name which must be consistant " +
                                  "to all calls");
                Console.WriteLine("3. guid - version key that you got when " +
                                  "registering the organization");
                if (semver == null)
                {
                    Console.WriteLine("*************************************");
                    Console.WriteLine("You also need to create an Environment ");
                    Console.WriteLine("variable called SemVerService as this ");
                    Console.WriteLine("will contain the final semVer version."); 
                }
                Console.WriteLine("***************************************");
                Console.WriteLine();
                Console.WriteLine("Press any key to close this window");
                Console.ReadKey();
                Environment.Exit(0);
            }
            if (semver == null)
            {
                Console.WriteLine("*************************************");              
                Console.WriteLine("You need to create an Environment ");
                Console.WriteLine("variable called SemVerService as this ");
                Console.WriteLine("will contain the final semVer version.");
                
                Console.WriteLine("*************************************");
                Console.WriteLine();
                Console.WriteLine("Press any key to close this window");
                Console.ReadKey();
                Environment.Exit(0);
            }

            string path = args[0];
            string project = args[1];
            string versionKey = args[2];
            string returnValue = "";
            
            RepositoryViewModel repo = new RepositoryViewModel();
            //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //path = @"c:\repo\Sample\.git";
            repo.RepositoryFullPath = path;
            repo.LoadEntireRepository();
            Console.WriteLine($"Total Number of Commits: {repo.Commits.Count}");
            Console.WriteLine($"Total Number of Branches: {repo.Branches.Count}");
            Console.WriteLine($"Total Number of Tags: {repo.Tags.Count}");
            Console.WriteLine($"The Branches:");
            string[] tips = Helper.GetTips(repo.Branches);

            if (tips.Length > 1)
            {
                Console.WriteLine("**NO_CI**");
                Console.ReadKey();
            }
            else
            {
                if (tips[0].StartsWith("remotes/origin/"))
                {
                    branch = tips[0].Remove(0, 15);
                }
                else if (tips[0].StartsWith("origin/"))
                {
                    branch = tips[0].Remove(0, 7);
                }
                else
                {
                    branch = tips[0];
                }
                
                Console.WriteLine($"{branch} is going to be sent to the api.");
                var client = new RestClient("https://localhost:5001/");

                var request = new RestRequest($"api/Version/{key}", Method.GET);
                request.AddParameter("Product", project);
                request.AddParameter("Branch", branch);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Environment.SetEnvironmentVariable("SemVerService", response.Content, EnvironmentVariableTarget.User);
                    Console.WriteLine(response.Content);                    
                }
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
            }


            foreach(Branch branch in repo.Branches)
            {
                if (branch.Tip != null)
                    Console.WriteLine($"{branch.Name} Tracking: {branch.IsTracking} Remote: {branch.IsRemote} Tip: {branch.Tip.IsHead}");
                else
                    Console.WriteLine($"{branch.Name} Tracking: {branch.IsTracking} Remote: {branch.IsRemote} Tip: null");
            }            
        }
    }
}
