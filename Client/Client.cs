using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class Client:IDisposable
    {
        private readonly Uri _apiUrl = new Uri("https://innometric.guru:8120");

        private HttpClient ApiClient { get; }



        public Client(string username, string password)
        {
            ApiClient = new HttpClient {BaseAddress = _apiUrl};

            string authToken = GetAuthToken(username, password);

            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }


        public void SendActivities(List<Activity> activities)
        {
            foreach (Activity activity in activities)
            {
                AddActivity(activity);
            }
        }



        private string GetAuthToken(string username, string password)
        {
            var values = new Dictionary<string, string>
            {
                { "email", username },
                { "password", password }
            };

            var content = new FormUrlEncodedContent(values);
            var response = ApiClient.PostAsync("/login", content).Result;

            if (!response.IsSuccessStatusCode) return null;

            var resp = response.Content.ReadAsStringAsync().Result;

           return (string)JObject.Parse(resp)["token"];
        }

        private void AddActivity(Activity activity)
        {
            var content = new FormUrlEncodedContent(activity.GetDictinary());
            var response = ApiClient.PostAsync("/activity", content).Result;
            if (!response.IsSuccessStatusCode) return;
        }

        private void Logout()
        {
            var response = ApiClient.PostAsync("/logout", null).Result;
            if (!response.IsSuccessStatusCode) return;
        }

        public void Dispose()
        {
            Logout();
            ApiClient?.Dispose();
        }
    }

}
