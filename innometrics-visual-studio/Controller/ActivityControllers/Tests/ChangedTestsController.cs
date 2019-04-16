using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Tests
{
    /// <summary>
    /// Activity controller, which records changing of tests
    /// </summary>
    class ChangedTestsController : AbstractTestsActivityController
    {
        private int changedIndex;

        public ChangedTestsController() : base("vs_tests_changed") { }

        /// <inheritdoc />
        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (LinesCount == end.Parent.EndPoint.Line + 1) return;
            changedIndex = start.Line;
            if (start.CodeElement[vsCMElement.vsCMElementOther] == null && changedIndex != start.Line)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}