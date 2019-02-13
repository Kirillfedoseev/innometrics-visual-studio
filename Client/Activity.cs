using System.Collections.Generic;
using Newtonsoft.Json;

namespace Client
{
    public class Activity
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("measurements")]
        public List<Measurement> Measurements { get; set; }

        public Activity(string name)
        {
            Name = name;
            Measurements = new List<Measurement>();
        }

        public Activity(string name, List<Measurement> measurements)
        {
            Name = name;
            Measurements = measurements;
        }

        public string Searialize()
        {
            return "";
        }

        public IEnumerable<KeyValuePair<string, string>> GetDictinary()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Measurement
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("type")]
        public string Type { get; }
        [JsonProperty("value")]
        public string Value { get; }

        public Measurement(string name, string type, string value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
