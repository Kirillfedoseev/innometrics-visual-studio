
using System.Collections.Generic;
using innometrics_visual_studio.Model.Metrics;

namespace innometrics_visual_studio.Controller
{
    public interface IActivity
    {
        List<Metric> Metrics { get; }

    }
}
