using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using t3winc.version.data.Repos;
using t3winc.version.data;
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
            Helper.Setup.Execute(_url, _product);
        }

        [Test]
        public void Test1()
        {
            Assert.IsTrue(true);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DbContextOptions<VersionContext> options = new DbContextOptions<VersionContext>();
            var context = new VersionContext(options, Log.Logger);
            var data = new VersionRepo(context);

            data.DeleteOrganization(_key);
        }
    }
}