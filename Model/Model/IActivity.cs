using System.Collections.Generic;

namespace Model.Model
{
    public interface IActivity
    {
        List<Metric> Metrics { get; }

        void CleanMetricsStorage();

        event OnMetricsUpdated OnMetricsUpdated;
    }

    public delegate void OnMetricsUpdated(IActivity metrics);
}
