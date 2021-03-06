﻿using Markdown.Rules;
using NUnit.Framework;

namespace MarkdownTests
{
    public class BoldRule_Should
    {
        [TestCase("Hello!", ExpectedResult = "<strong>Hello!</strong>")]
        [TestCase("", ExpectedResult = "<strong></strong>")]
        [TestCase("I am a very big and long string, look at me!", ExpectedResult = "<strong>I am a very big and long string, look at me!</strong>")]
        public string Format_String(string input)
        {
            return new BoldRule().Format(input);
        }
    }
}
