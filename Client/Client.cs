
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Client
    {
        public Uri ApiUrl { get; } = new Uri("https://innometric.guru:8120");

        public string Username { get; }
        public string Password { get; }

        public HttpClient ApiClient;
       

        public Client(string username, string password)
        {
            Username = username;
            Password = password;
            ApiClient = new HttpClient {BaseAddress = ApiUrl};
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAuthToken());
        }


        public string GetAuthToken()
        {
            var values = new Dictionary<string, string>
            {
                { "email", Username },
                { "password", Password }
            };

            var content = new FormUrlEncodedContent(values);
            var response = ApiClient.PostAsync("/login", content).Result;

            if (!response.IsSuccessStatusCode) return null;

            var resp = response.Content.ReadAsStringAsync().Result;

           return (string)JObject.Parse(resp)["token"];
        }



        public string RegisterUser()
        {

            var response = ApiClient.GetAsync("/activity?offset=10&amount_to_return=10").Result;

            if (!response.IsSuccessStatusCode) return null;
            var resp = response.Content.ReadAsStringAsync().Result;
            return (string)JObject.Parse(resp)["token"];


            //var values = new Dictionary<string, string>
            //{
            //    { "username", Username },
            //    { "password", Password }
            //};
            //var content = new FormUrlEncodedContent(values);
            //Console.WriteLine(content);

            //HttpResponseMessage response = await ApiClient.PostAsync("/user", content);

            //if (!response.IsSuccessStatusCode) return null;

            //var resp = response.Content.ReadAsStringAsync().Result;
            ////return (string)JObject.Parse(resp)["token"];


            ////response.EnsureSuccessStatusCode();

            //// return URI of the created resource.
            //return response.Headers.Location;
        }
    }

}
