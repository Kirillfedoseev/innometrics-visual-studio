using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    class DeletedLinesOfCommentsActivityController : AbstractLinesOfCodeActivityController
    {
        public DeletedLinesOfCommentsActivityController() : base("vs_comments_deleted") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount <= end.Parent.EndPoint.Line + 1) return;

            if (start.CodeElement[vsCMElement.vsCMElementOther] != null)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}