namespace Markdown.Rules
{
    public class BoldRule : IMdRule
    {
        public const string RuleName = "Bold";

        public string StartString => "__";
        public string EndString => "__";
        public string Name => RuleName;
        public AllowedRules AllowedInsideRules { get; } = new AllowedRules();
        public string Format(string input) => $"<strong>{input}</strong>";
    }
}
