using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Model.Model;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    /// <summary>
    /// Abstract activity, which can be derrived and will be add to plugin as new activity automatically
    /// </summary>
    public abstract class AbstractActivityController:IActivity
    {
        /// <summary>
        /// Specific type of activity
        /// </summary>
        protected readonly string _activityType;

        /// <summary>
        /// Reference on document, in which currently activity is being recorded 
        /// </summary>
        protected Document _document;

        /// <summary>
        /// Flag, that activity is already recoding
        /// </summary>
        protected bool IsRecording;
        
        /// <summary>
        /// List of Collected metrics
        /// </summary>
        public List<Metric> Metrics { get; }

        /// <summary>
        /// Constructor of ActivityController
        /// </summary>
        /// <param name="activityType">specific type of the activity, which must be specified in successors</param>
        protected AbstractActivityController(string activityType)
        {
            _activityType = activityType;
            Metrics = new List<Metric>();
        }

        /// <summary>
        /// The event on start activity
        /// </summary>
        /// <param name="document">The document in which activity starts</param>
        public abstract void StartActivity(Document document);

        /// <summary>
        /// The event on end activity
        /// </summary>
        /// <param name="document">The document in which activity starts</param>
        public void EndActivity(Document document)
        {
            Metrics.Last().EndTime = DateTime.Now;
            IsRecording = false;
        }

        /// <summary>
        /// The event on change document's text
        /// </summary>
        /// <param name="start">start point of change</param>
        /// <param name="end">end point of change</param>
        /// <param name="i">difference between points</param>
        public abstract void OnChanged(TextPoint start, TextPoint end, int i);

    }
}
