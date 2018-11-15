﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Markdown.Rules;

namespace Markdown
{
    public class Md
    {
        private static readonly Dictionary<string, IMdRule> StartStrings;
        private static readonly Dictionary<string, IMdRule> EndStrings;
        private static readonly ISet<char> SpecialSymbols;

        static Md()
        {
            StartStrings = new Dictionary<string, IMdRule>();
            EndStrings = new Dictionary<string, IMdRule>();
            SpecialSymbols = new HashSet<char>();

            foreach (var ruleType in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IMdRule))))
            {
                var rule = (IMdRule) Activator.CreateInstance(ruleType);
                StartStrings.Add(rule.StartString(), rule);
                EndStrings.Add(rule.EndString(), rule);

                SpecialSymbols.AddRange(rule.StartString());
                SpecialSymbols.AddRange(rule.EndString());
            }
        }

        public string Render(string mdLine)
        {
            var marks = ScanForMarks(mdLine);
            var pairs = ToPairs(marks).OrderByDescending(pair => pair.depth).ToList();
            var remainingMarks = pairs
                .SelectMany(pair => new[] {pair.startMark, pair.endMark})
                .OrderBy(mark => mark.Start).ToList();

            var sb = new StringBuilder(mdLine);
            foreach (var (startMark, endMark, _) in pairs)
            {
                var content = sb.ToString(startMark.End, endMark.Start - startMark.End);
                var formattedContent = startMark.RelatedRule.Format(content);
                var lengthDifference = formattedContent.Length - (startMark.Length + content.Length + endMark.Length);
                foreach (var affectedMarks in remainingMarks.SkipWhile(mark => mark.Start <= endMark.Start))
                {
                    affectedMarks.Move(lengthDifference);
                }

                sb.Remove(startMark.Start, endMark.End - startMark.Start);
                sb.Insert(startMark.Start, formattedContent);
            }

            return sb.ToString();
        }

        private static IEnumerable<Mark> ScanForMarks(string mdLine)
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

        private static IEnumerable<(Mark startMark, Mark endMark, int depth)> ToPairs(IEnumerable<Mark> marks)
        {
            var markStack = new Stack<Mark>();
            foreach (var mark in marks)
            {
                if (mark.Type.IsEnd() && markStack.Any() && markStack.Peek().RelatedToTheSameRule(mark))
                {
                    yield return (markStack.Pop(), mark, markStack.Count);
                }
                else if (mark.Type.IsStart())
                {
                    markStack.Push(mark);
                }
            }
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
            while (startIndex < input.Length && !IsSymbolSpecial(input[startIndex]))
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
