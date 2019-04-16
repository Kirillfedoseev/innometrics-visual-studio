using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace innometrics_visual_studio.Controller.ActivityControllers.Tests
{
    /// <summary>
    /// Activity controller, which records adding of tests
    /// </summary>
    class AddedTestsActivityController : AbstractTestsActivityController
    {
        public AddedTestsActivityController() : base("vs_tests_added") { }


        /// <inheritdoc />
        public override void OnChanged(TextPoint start, TextPoint end, int i)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (changedIndex == start.Line) return;

            var text = start.Parent.CreateEditPoint(start.Parent.StartPoint).GetText(start.Parent.EndPoint);
            int testsCount = Regex.Matches(text, $@"\[TestMethod\]").Count;

            if (TestsCount >= testsCount) return;

            Metrics.Last().IncrementMetric();
            LinesCount = end.Parent.EndPoint.Line + 1;
            TestsCount = testsCount;         
        }
    }
}