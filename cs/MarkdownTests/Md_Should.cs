using System.Diagnostics.CodeAnalysis;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class Md_Should
    {
        [TestCase("Parse me", ExpectedResult = "Parse me",
            TestName = "Не меняет строку без форматирующих символов")]
        [TestCase("_Parse me_", ExpectedResult = "<em>Parse me</em>",
            TestName = "Оборачивает в <em> строку, обернутую в знаки подчерка")]
        [TestCase("Pase me, _me_ and me", ExpectedResult = "Pase me, <em>me</em> and me", 
            TestName = "Оборачивает в <em> строку, обернутую в знаки подчерка, не трогая то, что лежит вне знаков")]
        [TestCase("Pase me, _ and me", ExpectedResult = "Pase me, _ and me",
            TestName = "Не трогает одиночные знаки подчерка внутри строки")]
        public string Render(string mdLine)
        {
            return new Md().Render(mdLine);
        }
    }
}
