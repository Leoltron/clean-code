using System.Diagnostics.CodeAnalysis;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class Md_Should
    {
        [TestCase("Parse me", ExpectedResult = "Parse me", TestName = "Не меняет строку без форматирующих символов")]
        public string Render(string mdLine)
        {
            return new Md().Render(mdLine);
        }
    }
}
