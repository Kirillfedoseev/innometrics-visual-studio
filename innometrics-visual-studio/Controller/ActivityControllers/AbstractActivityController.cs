using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using innometrics_visual_studio.Model.Metrics;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    public abstract class AbstractActivityController:IActivity
    {
        protected DTE _application;

        protected readonly string _activityType;

        protected Document _document;

        protected bool IsRecording;

        public List<Metric> Metrics { get; }

        protected AbstractActivityController(string activityType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Metrics = new List<Metric>();

            _activityType = activityType;
            
            _application = MenuController.Instance.Dte;
            _application.Events.DocumentEvents.DocumentOpened += StartActivity;
            _application.Events.DocumentEvents.DocumentClosing += EndActivity;
            _application.Events.TextEditorEvents.LineChanged += OnChanged;
            File.WriteAllText("outpu.txt",_application.Name);
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
