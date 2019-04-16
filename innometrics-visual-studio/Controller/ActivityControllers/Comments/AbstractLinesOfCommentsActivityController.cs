using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Model.Model;

namespace innometrics_visual_studio.Controller.ActivityControllers.Comments
{
    /// <summary>
    /// Abstract activity controller, which are associated with comments
    /// </summary>
    abstract class AbstractLinesOfCommentsActivityController : AbstractActivityController
    {
        protected int LinesCount;

        protected AbstractLinesOfCommentsActivityController(string activityType) : base(activityType) { }



        public override void StartActivity(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (IsRecording) EndActivity(_document);
            IsRecording = true;
            _document = document;
            var textDocument = (TextDocument)document.Object("TextDocument");
            LinesCount = textDocument.EndPoint.Line + 1;

            Metrics.Add(new Metric(document.Name, _activityType));
        }
    }
}