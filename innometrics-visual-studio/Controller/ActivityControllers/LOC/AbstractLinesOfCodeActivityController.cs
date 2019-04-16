using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Model.Model;

namespace innometrics_visual_studio.Controller.ActivityControllers.LOC
{
    /// <summary>
    /// Abstract activity controller, which are associated with LOC
    /// </summary>
    abstract class  AbstractLinesOfCodeActivityController : AbstractActivityController
    {
        protected int LinesCount;
        protected int changedIndex;

        protected AbstractLinesOfCodeActivityController(string activityType) : base(activityType){}


        /// <inheritdoc />
        public override void StartActivity(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (IsRecording) EndActivity(_document);
            IsRecording = true;
            _document = document;
            var textDocument = (TextDocument) document.Object("TextDocument");
            LinesCount = textDocument.EndPoint.Line + 1;

            Metrics.Add(new Metric(document.Name,_activityType));
        }

    }
}