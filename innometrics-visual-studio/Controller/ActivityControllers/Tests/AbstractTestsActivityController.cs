using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Model.Model;

namespace innometrics_visual_studio.Controller.ActivityControllers.Tests
{

    /// <summary>
    /// Abstract activity controller, which are associated with tests
    /// </summary>
    abstract class AbstractTestsActivityController : AbstractActivityController
    {
        protected int LinesCount;
        protected int TestsCount;

        /// <inheritdoc />
        protected AbstractTestsActivityController(string activityType) : base(activityType) { }


        /// <inheritdoc />
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