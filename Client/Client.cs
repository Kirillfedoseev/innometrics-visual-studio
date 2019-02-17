using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiClient
{
    public class Client:IDisposable
    {
        private readonly Uri _apiUrl = new Uri("https://innometric.guru:8120");

        public static bool IsAuthDataCorrect(string username, string password)
        {

                try
                {
                    bool result;
                    using (var client = new Client())
                    {
                        result = client.GetAuthToken(username, password) != null;
                    }
                    return result;
                }
                catch (Exception e)
                {
                    return false;
                }
        }

        private HttpClient ApiClient { get; }

        private Client()
        {
            ApiClient = new HttpClient { BaseAddress = _apiUrl };
        }

        public Client(string username, string password)
        {
            ApiClient = new HttpClient {BaseAddress = _apiUrl};
            string authToken = GetAuthToken(username, password);
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }


        public void SendMetrics(IEnumerable<IMetric> metrics)
        {
            var content = new FormUrlEncodedContent(
                new Dictionary<string,string>(1)
                {
                    {"activity", MetricsToJson(metrics).ToString()}
                });
            
            var response = ApiClient.PostAsync("/activity", content).Result;
            if (!response.IsSuccessStatusCode)
                throw new AuthenticationException($"Activity post failed with error: {response.StatusCode} {response.ReasonPhrase} ");
        }


        //todo change if format incorrec
        private JObject MetricsToJson(IEnumerable<IMetric> metrics)
        {
            JArray jArray = new JArray();
            foreach (IMetric metric in metrics)
            {
                JObject json = new JObject()
                {
                    {"executable_name", metric.ExecutableName},
                    {"start_time ", metric.StartTime.ToString(CultureInfo.InvariantCulture)},
                    {"end_time", metric.EndTime.ToString(CultureInfo.InvariantCulture)},
                    {"ip_address", metric.Ip4Address},
                    {"mac_address", metric.MacAddress},
                    {"activity_type", metric.ActivityType},
                    {"value", metric.Value}
                };
                jArray.Add(json);
            }

            JObject data = new JObject {{"activities", jArray}};

            Console.WriteLine(data.ToString(Formatting.Indented));
            return data;
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

            if (!response.IsSuccessStatusCode)
                throw new AuthenticationException($"Login failed with error: {response.StatusCode} {response.ReasonPhrase} ");

            var resp = response.Content.ReadAsStringAsync().Result;

            return (string)JObject.Parse(resp)["token"];
        }


        private void LogOut()
        {
            var response = ApiClient.PostAsync("/logout", null).Result;
            if (!response.IsSuccessStatusCode) return;
        }


        public void Dispose()
        {
            LogOut();
            ApiClient?.Dispose();
        }
    }

}
