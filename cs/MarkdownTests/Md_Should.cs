using System.Diagnostics.CodeAnalysis;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class Md_Should
    {
        [TestCase("Parse me", ExpectedResult = "Parse me", TestName = "Не меняет строку без форматирующих символов")]
        [TestCase("_Parse me_", ExpectedResult = "<em>Parse me</em>", TestName = "Оборачивает в <em> строку, обернутую в знаки подчерка")]
        public string Render(string mdLine)
        {
            return new Md().Render(mdLine);
        }
    }
}
