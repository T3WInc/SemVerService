using System;
using System.Linq;
using Serilog;
using t3winc.version.common.Interfaces;
using t3winc.version.domain;

namespace t3winc.version.data.repos
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