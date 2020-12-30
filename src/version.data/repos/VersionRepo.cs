using System;
using System.Linq;
using Serilog;
using t3winc.version.common.Interfaces;
using t3winc.version.data.Helper;
using t3winc.version.domain;

namespace t3winc.version.data.Repos
{
    public class VersionRepo : IVersionRepo
    {
        private VersionContext _context;

        public VersionRepo(VersionContext context)
        {
            _context = context;
        }

        public string NewRegistration(string organization)
        {
            Log.Information("VersionRepo:NewRegistration:Beginning");
            Log.Information("1. Check if the organization already exists");
            if (_context.Version.Any(e => e.Organization == organization))
            {
                Log.Error($"{organization} already exists in the database.");
                return "Sorry, This Organizaition already exits";
            }           
            
            Log.Information("2. Create a new guid");
            var guid = Guid.NewGuid();

            Log.Information("3. Save to the db.Table");
            MyVersion registration = new MyVersion();
            registration.Key = guid.ToString();
            registration.Organization = organization;
            _context.Add(registration);
            _context.SaveChanges();

            Log.Information("4. Return the guid");
            Log.Information("VersionRepo:NewRegistration:Completed");
            return guid.ToString();
        }

        public string GetNextVersionNumber(int version, string product, string branch)
        {
            Log.Information("VersionRepo:GetNextVersionNumber:Beginning");
            Log.Information("1. Check if the branch for this product already exists");
            BranchRepo repo = new BranchRepo(_context);
            Branch result = new Branch();
            if (repo.BranchExists(version, product, branch))
            {
                Log.Information("2. If existing then increment the revision number.");
                result = repo.GetBranch(version, product, branch);
                result.Revision++;
                result.Version = $"{result.Major}.{result.Minor}.{result.Patch}-{result.Suffix}.{result.Revision}";
            }
            else
            {
                Log.Information("2. If New check the type of branch (feature, bug, pull, major) setup appropriate increments");                
                result = repo.NewBranch(version, product, branch);
            }
            _context.SaveChanges();                       
            Log.Information("3. Return the next version number for the product.");
            Log.Information("VersionRepo:GetNextVersionNumber:Completed");
            return result.Version;
        }

        public bool IsKeyValid(string key)
        {
            if (_context.Version.Any(e => e.Key == key))
            {
                return true;
            }
            return false;
        }

        public int GetVersionId(string key)
        {
            return _context.Version.Where(e => e.Key == key).Select(e => e.Id).FirstOrDefault();                
        }
    }
}