namespace Markdown.Rules
{
    public class IitalicizationRule : IMdRule
    {
        public string StartString() => "_";

        public string EndString() => "_";

        public string Format(string input) => $"<em>{input}</em>";
    }
}
