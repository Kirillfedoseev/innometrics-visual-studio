using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Comments
{
    /// <summary>
    /// Activity controller, which records adding of comments
    /// </summary>
    class AddedLinesOfCommentsCodeActivityController : AbstractLinesOfCommentsActivityController
    {
        public AddedLinesOfCommentsCodeActivityController() : base("vs_comments_added") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount >= end.Parent.EndPoint.Line + 1) return;

            if (start.CodeElement[vsCMElement.vsCMElementOther] != null)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}