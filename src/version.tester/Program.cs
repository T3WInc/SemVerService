using System;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;

namespace version.tester
{
    class Program
    {
        static string key = "87c55270-57ef-4707-bb54-c0774afd670e";
        static string product = "MyProduct";
        static void Main(string[] args)
        {
            var client = new RestClient("https://localhost:5001/");
            var request = new RestRequest($"api/Product/{key}", Method.PUT);
            request.AddParameter("Product", product);
            request.AddParameter("Increment", "Major");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
        }
    }
}
