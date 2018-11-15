using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
            IMdRule lastFoundRule = null;
            var lastFoundRuleStart = -1;
            var index = 0;
            var stringBuilder = new StringBuilder();

            while (index < mdLine.Length)
            {
                if (IsSymbolSpecial(mdLine[index]))
                {
                    var specialEnd = SkipUntilNonSpecialSymbol(mdLine, index);
                    var specialString = mdLine.Substring(index, specialEnd - index);

                    if (lastFoundRule != null &&
                        EndStrings.TryGetValue(specialString, out var rule) &&
                        ReferenceEquals(rule, lastFoundRule))
                    {
                        stringBuilder.Append(rule.Format(mdLine.Substring(lastFoundRuleStart,
                            index - lastFoundRuleStart)));
                        lastFoundRule = null;
                    }
                    else if (lastFoundRule == null &&
                             StartStrings.TryGetValue(specialString, out rule))
                    {
                        lastFoundRule = rule;
                        lastFoundRuleStart = specialEnd;
                    }

                    index = specialEnd;
                }
                else
                {
                    var newIndex = SkipUntilSpecialSymbol(mdLine, index);
                    if(lastFoundRule == null)
                    {
                        stringBuilder.Append(mdLine, index, newIndex - index);
                    }

                    index = newIndex;
                }
            }

            return stringBuilder.ToString();
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
