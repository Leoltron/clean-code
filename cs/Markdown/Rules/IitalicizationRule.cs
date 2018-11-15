namespace Markdown.Rules
{
    public class IitalicizationRule : IMdRule
    {
        public const string RuleName = "Italic";

        public string StartString => "_";
        public string EndString => "_";
        public string Name => RuleName;
        public AllowedRules AllowedInsideRules { get; } = new AllowedRules(ListType.BlackList, BoldRule.RuleName);
        public string Format(string input) => $"<em>{input}</em>";
    }
}
