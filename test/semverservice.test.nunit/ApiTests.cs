using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using t3winc.version.data.Repos;
using t3winc.version.data;
using t3winc.version.common.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace semverservice.test.nunit
{
    public class ApiTests
    {
        private string _key;
        private string _product;
        private string _url;


        public ApiTests()
        {
            _product = "MyProduct";
            //_url = "https://version.t3winc.com";
            _url = "https://localhost:5001";

        }

        [OneTimeSetUp]
        public void Setup()
        {
            _key = Helper.Setup.Execute(_url, _product);
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(true);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            var connectionstring = "Server=stage.t3winc.com,1433;Database=VersionDb;User Id=sa;Password=thankYou99";
            var optionBuilder = new DbContextOptionsBuilder<VersionContext>()
                                              .UseSqlServer(connectionstring);
            VersionContext dbContext = new VersionContext(optionBuilder.Options, Log.Logger);
            var data = new VersionRepo(dbContext);
            data.DeleteOrganization(_key);
        }
    }
}