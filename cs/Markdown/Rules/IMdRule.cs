namespace Markdown.Rules
{
    public interface IMdRule
    {
        string StartString { get; }
        string EndString { get; }
        string Name { get; }
        AllowedRules AllowedInsideRules { get; }
        string Format(string input);
    }
}
