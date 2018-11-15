namespace Markdown.Rules
{
    public interface IMdRule
    {
        string StartString();
        string EndString();
        string Format(string input);
    }
}
