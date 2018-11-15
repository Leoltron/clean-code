namespace Markdown.Rules
{
    public class BoldRule : IMdRule
    {
        public string StartString() => "__";

        public string EndString() => "__";

        public string Format(string input) => $"<strong>{input}</strong>";
    }
}
