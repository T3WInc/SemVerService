using System;
using Xunit;
using RestSharp;
using RestSharp.Authenticators;

namespace semverservice.test.xunit
{
    public class ApiTests
    {
        private string _key;
        private string _product;
        private string _url;

        public ApiTests()
        {
            _product = "MyProduct";
            _url = "https://version.t3winc.com";
        }

        [Fact]
        public void PostVersionNumber()
        {
            var client = new RestClient(_url);
            var request = new RestRequest("api/version", Method.POST);
            request.AddParameter("Organization","MyTestCompany");
            IRestResponse response = client.Execute(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _key = response.Content;
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }
    }
}
