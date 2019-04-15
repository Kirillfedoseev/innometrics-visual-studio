using System.Text.RegularExpressions;
using EnvDTE;
using EnvDTE80;
using innometrics_visual_studio.Model.Metrics;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    abstract class AbstractTestsActivityController : AbstractActivityController
    {
        protected int LinesCount;
        protected int TestsCount;

        protected AbstractTestsActivityController(string activityType) : base(activityType) { }



        public override void StartActivity(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (IsRecording) EndActivity(_document);
            var textDocument = (TextDocument)document.Object("TextDocument");
            var text = textDocument.CreateEditPoint(textDocument.StartPoint).GetText(textDocument.EndPoint);

            if (!text.Contains("[TestClass]")) return;

            IsRecording = true;

            _document = document;

            LinesCount = textDocument.EndPoint.Line + 1;
            TestsCount = Regex.Matches(text, $@"\[TestMethod\]").Count;
            Metrics.Add(new Metric(document.Name, _activityType));
        }

       

    }
}