using System.Linq;
using EnvDTE;
using innometrics_visual_studio.Controller.ActivityControllers.LOC;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Comments
{
    /// <summary>
    /// Activity controller, which records changing of comments
    /// </summary>
    class ChangedLinesOfCommentsActivityController : AbstractLinesOfCodeActivityController
    {
        private int _changedIndex;

        public ChangedLinesOfCommentsActivityController() : base("vs_comments_changed") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount == end.Parent.EndPoint.Line + 1) return;
            _changedIndex = start.Line;
            if (start.CodeElement[vsCMElement.vsCMElementOther] != null && _changedIndex != start.Line)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}