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
        [TestCase("__Parse _me_ and me__",
            ExpectedResult = "<strong>Parse <em>me</em> and me</strong>",
            TestName = "Выделение курсивом работает внутри выделения полужирным")]
        [TestCase("_Parse __me__ and me_",
            ExpectedResult = "<em>Parse __me__ and me</em>",
            TestName = "Выделение полужирным не работает внутри выделения курсивом")]
        [TestCase("ignore\\_me\\_",
            ExpectedResult = "ignore\\_me\\_",
            TestName = "Экранирование символов должно работать")]
        [TestCase("Hello _ me_",
            ExpectedResult = "Hello _ me_",
            TestName = "Подчерки, начинающие выделение и после которых идет пробельный символ, должны быть проигнорированы")]
        [TestCase("Hello_me_",
            ExpectedResult = "Hello_me_",
            TestName = "Подчерки, начинающие выделение и перед которыми нет пробельного символа, должны быть проигнорированы")]
        public string Render(string mdLine)
        {
            return new Md().Render(mdLine);
        }
    }
}
