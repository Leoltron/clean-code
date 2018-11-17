using System.Collections.Generic;
using Markdown.Rules;

namespace Markdown
{
    public class AllowedRules
    {
        private readonly ISet<string> ruleNameList;
        private readonly ListType listType;

        public AllowedRules() : this(ListType.BlackList)
        {
        }

        public AllowedRules(ListType listType, params string[] ruleNameList)
        {
            this.listType = listType;
            this.ruleNameList = new HashSet<string>(ruleNameList);
        }

        public bool DoesAccept(IMdRule rule)
        {
            return ruleNameList.Contains(rule.Name)
                ? listType.DoesAcceptIfInList()
                : listType.DefaultAcceptance();
        }
    }
}
