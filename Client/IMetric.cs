using System;

namespace Client
{
    public interface IMetric
    {
        string ExecutableName { get; }

        DateTime StartTime { get; }

        DateTime EndTime { get; }

        string Ip4Address { get; }

        string MacAddress { get; }

        string ActivityType { get; }

        string Value { get; }

    }

    class TestMetric : IMetric
    {
        public string ExecutableName { get; } = "visual studio";
        public DateTime StartTime { get; } = DateTime.Now;
        public DateTime EndTime { get; } = DateTime.Now;
        public string Ip4Address { get; } = "127.0.0.1";
        public string MacAddress { get; } = "34:D4:32:16:17:76";
        public string ActivityType { get; } = "eclipse_lines_insert";
        public string Value { get; } = "228";
    }
}
