using System.Diagnostics.CodeAnalysis;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class Md_Should
    {
        [TestCase("Parse me",
            ExpectedResult = "Parse me",
            TestName = "Не меняет строку без форматирующих символов")]
        [TestCase("_Parse me_",
            ExpectedResult = "<em>Parse me</em>",
            TestName = "Оборачивает в <em> строку, обернутую в знаки подчерка")]
        [TestCase("Parse me, _me_ and me",
            ExpectedResult = "Parse me, <em>me</em> and me",
            TestName = "Оборачивает в <em> строку, обернутую в знаки подчерка, не трогая то, что лежит вне знаков")]
        [TestCase("Parse me, _ and me",
            ExpectedResult = "Parse me, _ and me",
            TestName = "Не трогает одиночные знаки подчерка внутри строки")]
        [TestCase("Parse me, and me_",
            ExpectedResult = "Parse me, and me_",
            TestName = "Не трогает одиночные знаки подчерка в конце строки")]
        [TestCase("__Parse me__",
            ExpectedResult = "<strong>Parse me</strong>",
            TestName = "Оборачивает в <strong> строку, обернутую в двойные знаки подчерка")]
        public string Render(string mdLine)
        {
            return new Md().Render(mdLine);
        }
    }
}
