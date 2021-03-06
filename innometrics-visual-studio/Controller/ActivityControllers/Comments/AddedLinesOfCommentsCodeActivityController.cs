﻿using System.Linq;
using System.Text.RegularExpressions;
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

            if (changedIndex == start.Line) return;
            if (LinesCount >= end.Parent.EndPoint.Line + 1) return;

            var text = start.Parent.CreateEditPoint(start.Parent.StartPoint).GetText(start.Parent.EndPoint).Split('\n')[start.Line];

            bool isComment = Regex.Matches(text, $@"//(.*?)\r?\n?").Count != 0;
            if (!isComment) return;


            Metrics.Last().IncrementMetric();
            LinesCount = end.Parent.EndPoint.Line + 1;
        }
    }
}