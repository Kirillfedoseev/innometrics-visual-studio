using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Tests
{
    /// <summary>
    /// Activity controller, which records deleting of tests
    /// </summary>
    class DeletedTestsActivityController : AbstractTestsActivityController
    {
        public DeletedTestsActivityController() : base("vs_tests_deleted") { }

        /// <inheritdoc />
        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var text = start.Parent.CreateEditPoint(start.Parent.StartPoint).GetText(start.Parent.EndPoint);
            int testsCount = Regex.Matches(text, $@"\[TestMethod\]").Count;
            if (LinesCount >= end.Parent.EndPoint.Line + 1) return;

            if (start.CodeElement[vsCMElement.vsCMElementOther] == null)
                Metrics.Last().IncrementMetric();

            LinesCount = end.Parent.EndPoint.Line + 1;
            TestsCount = testsCount;

        }
    }
}