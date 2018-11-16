using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string mdLine)
        {
            var marks = MarkSearchEngine.ScanForMarks(mdLine);
            var pairs = MarkSearchEngine.ToPairsFromDeepToShallow(mdLine, marks).ToList();
            return RenderPairs(mdLine, pairs);
        }

        private static string RenderPairs(string mdLine, List<(Mark startMark, Mark endMark)> pairsFromDeepToShallow)
        {
            var remainingMarks = pairsFromDeepToShallow
                .SelectMany(pair => new[] {pair.startMark, pair.endMark})
                .OrderBy(mark => mark.Start).ToList();
            var stringBuilder = new StringBuilder(mdLine);
            foreach (var (start, end) in pairsFromDeepToShallow)
            {
                var content = stringBuilder.ToString(start.End, end.Start - start.End);
                var formattedContent = start.RelatedRule.Format(content);
                var lengthDifference = formattedContent.Length - (start.Length + content.Length + end.Length);
                foreach (var affectedMarks in remainingMarks.SkipWhile(mark => mark.Start <= end.Start))
                {
                    affectedMarks.Move(lengthDifference);
                }

                stringBuilder.Remove(start.Start, end.End - start.Start);
                stringBuilder.Insert(start.Start, formattedContent);
            }

            return stringBuilder.ToString();
        }
    }
}
