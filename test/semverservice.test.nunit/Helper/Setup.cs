
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RestSharp;

namespace semverservice.test.nunit.Helper
{
    public static class Setup
    {
        public static string Execute(string url, string product)
        {
            string key = "";

            var client = new RestClient(url);
            var request = new RestRequest("api/version/", Method.POST);
            request.AddQueryParameter("organization", "MyTestCompany");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                key = response.Content.Trim( new char[] { '"' });
            }

            // Add a Product to our sample data...
            client = new RestClient(url);
            request = new RestRequest("api/product/", Method.POST);
            request.AddQueryParameter("key", key);
            request.AddQueryParameter("Product", product);
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "error";
            }

            // Increment the Major number....
            client = new RestClient(url);
            request = new RestRequest("api/product/", Method.PUT);
            request.AddQueryParameter("key", key);
            request.AddQueryParameter("Product", product);
            request.AddQueryParameter("Increment", "Major");
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "error";
            }

            // Increment the Minor number...
            client = new RestClient(url);
            request = new RestRequest("api/product/", Method.PUT);
            request.AddQueryParameter("key", key);
            request.AddQueryParameter("Product", product);
            request.AddQueryParameter("Increment", "Minor");
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "error";
            }

            // Increment the Patch number....
            client = new RestClient(url);
            request = new RestRequest("api/product/", Method.PUT);
            request.AddQueryParameter("key", key);
            request.AddQueryParameter("Product", product);
            request.AddQueryParameter("Increment", "Patch");
            response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "error";
            }


            return key;
        }
    }
}
