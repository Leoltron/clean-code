using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Markdown.Rules;

namespace Markdown
{
    public static class MarkSearchEngine
    {
        private static readonly Dictionary<string, IMdRule> StartStrings;
        private static readonly Dictionary<string, IMdRule> EndStrings;
        private static readonly ISet<char> SpecialSymbols;

        static MarkSearchEngine()
        {
            StartStrings = new Dictionary<string, IMdRule>();
            EndStrings = new Dictionary<string, IMdRule>();
            SpecialSymbols = new HashSet<char>();

            foreach (var ruleType in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IMdRule))))
            {
                var rule = (IMdRule) Activator.CreateInstance(ruleType);
                StartStrings.Add(rule.StartString, rule);
                EndStrings.Add(rule.EndString, rule);

                SpecialSymbols.AddRange(rule.StartString);
                SpecialSymbols.AddRange(rule.EndString);
            }
        }

        public static IEnumerable<Mark> ScanForMarks(string mdLine)
        {
            for (var index = 0; index < mdLine.Length;)
            {
                if (IsSymbolSpecial(mdLine[index]))
                {
                    var specialEnd = SkipUntilNonSpecialSymbol(mdLine, index);
                    var specialString = mdLine.Substring(index, specialEnd - index);
                    if (TryFindRule(specialString, out var result))
                    {
                        yield return new Mark(result.rule, index, specialEnd, result.type);
                    }

                    index = specialEnd;
                }
                else
                {
                    index = SkipUntilSpecialSymbol(mdLine, index);
                }
            }
        }

        public static IEnumerable<(Mark start, Mark end)> ToPairsFromDeepToShallow(
            string initialString,
            IEnumerable<Mark> marks)
        {
            var marksByDepth = new List<List<(Mark start, Mark end)>>();
            foreach (var (start, end, depth) in ToPairs(initialString, marks))
            {
                while (marksByDepth.Count <= depth)
                {
                    marksByDepth.Add(new List<(Mark, Mark)>());
                }
                marksByDepth[depth].Add((start,end));
            }

            marksByDepth.Reverse();
            return marksByDepth.SelectMany(pair => pair);
        }

        public static IEnumerable<(Mark startMark, Mark endMark, int depth)> ToPairs(string initialString,
            IEnumerable<Mark> marks)
        {
            var markStack = new Stack<Mark>();
            foreach (var mark in marks)
            {
                if (markStack.Any(upperMark => !upperMark.RelatedRule.AllowedInsideRules.DoesAccept(mark.RelatedRule) &&
                                               !upperMark.RelatedToTheSameRule(mark)))
                {
                    continue;
                }

                if (IsValidEndMark(initialString, mark) &&
                    markStack.Any() && markStack.Peek().RelatedToTheSameRule(mark))
                {
                    yield return (markStack.Pop(), mark, markStack.Count);
                }
                else if (IsValidStartMark(initialString, mark))
                {
                    markStack.Push(mark);
                }
            }
        }

        private static bool IsValidEndMark(string initialString, Mark mark)
        {
            return mark.Type.IsEnd() &&
                   mark.Start != 0 && !char.IsWhiteSpace(initialString, mark.Start - 1) &&
                   (mark.End == initialString.Length || char.IsWhiteSpace(initialString, mark.End));
        }

        private static bool IsValidStartMark(string initialString, Mark mark)
        {
            return mark.Type.IsStart() &&
                   (mark.Start == 0 || char.IsWhiteSpace(initialString, mark.Start - 1)) &&
                   mark.End != initialString.Length && !char.IsWhiteSpace(initialString, mark.End);
        }

        private static bool TryFindRule(string key, out (IMdRule rule, MarkType type) value)
        {
            var hasStart = StartStrings.TryGetValue(key, out var startRule);
            var hasEnd = EndStrings.TryGetValue(key, out var endRule);

            if (!hasStart && !hasEnd)
            {
                value = (null, MarkType.None);
                return false;
            }

            if (hasStart && hasEnd)
            {
                value = (startRule, MarkType.StartOrEnd);
            }
            else if (hasStart)
            {
                value = (startRule, MarkType.Start);
            }
            else /*hasEnd*/
            {
                value = (endRule, MarkType.End);
            }

            return true;
        }

        private static bool IsSymbolSpecial(char c) => SpecialSymbols.Contains(c);

        private static int SkipUntilSpecialSymbol(string input, int startIndex)
        {
            while (startIndex < input.Length &&
                   (startIndex != 0 && input[startIndex - 1] == '\\' || !IsSymbolSpecial(input[startIndex])))
            {
                startIndex++;
            }

            return startIndex;
        }

        private static int SkipUntilNonSpecialSymbol(string input, int startIndex)
        {
            while (startIndex < input.Length && IsSymbolSpecial(input[startIndex]))
            {
                startIndex++;
            }

            return startIndex;
        }
    }
}
