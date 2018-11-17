using Markdown.Rules;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ItalicizationRule_Should
    {
        [TestCase("Hello!", ExpectedResult = "<em>Hello!</em>")]
        [TestCase("", ExpectedResult = "<em></em>")]
        [TestCase("I'm a very big and long string, look at me!", ExpectedResult = "<em>I'm a very big and long string, look at me!</em>")]
        public string Format_String(string input)
        {
            return new ItalicizationRule().Format(input);
        }
    }
}
