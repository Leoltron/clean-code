namespace Markdown
{
    public interface IMdRule
    {
        string StartString();
        string EndString();
        string Format(string input);
    }
}
