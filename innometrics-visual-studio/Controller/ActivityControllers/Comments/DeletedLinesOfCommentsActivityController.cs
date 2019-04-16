using System.Linq;
using EnvDTE;
using innometrics_visual_studio.Controller.ActivityControllers.LOC;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Comments
{
    /// <summary>
    /// Activity controller, which records deleting of comments
    /// </summary>
    class DeletedLinesOfCommentsActivityController : AbstractLinesOfCodeActivityController
    {
        public DeletedLinesOfCommentsActivityController() : base("vs_comments_deleted") { }

        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {

            if (LinesCount <= end.Parent.EndPoint.Line + 1) return;

            if (start.CodeElement[vsCMElement.vsCMElementOther] != null)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}