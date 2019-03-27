using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers
{
    class ChangedLinesOfCommentsActivityController : AbstractLinesOfCodeActivityController
    {
        private int changedIndex;

        public ChangedLinesOfCommentsActivityController() : base("vs_comments_changed") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount == end.Parent.EndPoint.Line + 1) return;
            changedIndex = start.Line;
            if (start.CodeElement[vsCMElement.vsCMElementOther] != null && changedIndex != start.Line)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}