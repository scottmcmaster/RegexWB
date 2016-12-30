using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace RegexTest
{
    [TestFixture]
    public class TestStringMaker
    {
        private const string AreaCodeRegex = @"\((?<AreaCode>\d{3})\)";

        private StringMaker stringMaker;

        [SetUp]
        public void Setup()
        {
            this.stringMaker = new StringMaker();    
        }

        [Test]
        public void TestMakeCSharpString()
        {
            var result = stringMaker.MakeCSharpString(
                AreaCodeRegex, RegexOptions.Compiled);
            Assert.AreEqual("Regex regex = new Regex(@\"\r\n\t\t\t\t\\((?<AreaCode>\\d{3})\\)\", \r\n\t\t\t\tRegexOptions.Compiled);", result);
        }

        [Test]
        public void TestMakeVBString()
        {
            var result = stringMaker.MakeVBString(
                AreaCodeRegex, RegexOptions.Compiled);
            Assert.AreEqual("Dim r as Regex\r\n\r\nr = new Regex( _\r\n\"\\((?<AreaCode>\\d{3})\\)\" _\r\n, RegexOptions.Compiled)", result);
        }
    }
}
