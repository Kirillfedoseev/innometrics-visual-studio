using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.LOC
{
    /// <summary>
    /// Activity controller, which records changing of LOC
    /// </summary>
    class ChangedLinesOfCodeActivityController : AbstractLinesOfCodeActivityController
    {
        private int changedIndex;

        public ChangedLinesOfCodeActivityController() : base("vs_lines_changed") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount == end.Parent.EndPoint.Line + 1) return;
            changedIndex = start.Line;
    
            //todo get line
            if (start.CodeElement[vsCMElement.vsCMElementOther] == null && changedIndex != start.Line)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}