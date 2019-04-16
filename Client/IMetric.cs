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
}
