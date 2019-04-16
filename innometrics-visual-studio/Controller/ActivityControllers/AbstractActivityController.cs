using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using innometrics_visual_studio.Model.Metrics;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    public abstract class AbstractActivityController:IActivity
    {
        protected readonly string _activityType;

        protected Document _document;

        protected bool IsRecording;

        public List<Metric> Metrics { get; }

        protected AbstractActivityController(string activityType)
        {
            _activityType = activityType;
            Metrics = new List<Metric>();
        }

        public abstract void StartActivity(Document document);

        public void EndActivity(Document document)
        {
            Metrics.Last().EndTime = DateTime.Now;
            IsRecording = false;
        }


        public abstract void OnChanged(TextPoint start, TextPoint end, int i);

    }
}
