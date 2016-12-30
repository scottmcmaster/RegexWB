using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using RegexTest;

namespace RegexWorkbench.Tests
{
    public class TestRegexActions
    {
        private const string AreaCodeRegex = @"\((?<AreaCode>\d{3})\)";
        private const string PhoneNumber = "(425) 123-4567";
        private const string CSharpRegex = "Regex regex = new Regex(@\"\\((?<AreaCode>\\d{3})\\)\");";

        [Test]
        public void TestReplace_Simple()
        {
            Mock<IRegexTestView> view = new Mock<IRegexTestView>();
            view.SetupGet(v => v.RegexText).Returns(AreaCodeRegex);
            view.SetupGet(v => v.Strings).Returns(PhoneNumber);

            var actions = new RegexActions(view.Object);
            actions.Replace();

            view.VerifySet(v => v.Output = "Replacing: (425) 123-4567\r\n  with: \r\n  result:  123-4567\r\n");
        }

        [Test]
        public void TestSplit_Simple()
        {
            Mock<IRegexTestView> view = new Mock<IRegexTestView>();
            view.SetupGet(v => v.RegexText).Returns(AreaCodeRegex);
            view.SetupGet(v => v.Strings).Returns(PhoneNumber);

            var actions = new RegexActions(view.Object);
            actions.Split();

            view.VerifySet(v =>
                v.Output = "Splitting: (425) 123-4567\r\n    [0] => \r\n    [1] => 425\r\n    [2] =>  123-4567\r\n");
        }

        [Test]
        public void TestPasteFromCSharp_Blank()
        {
            Mock<IRegexTestView> view = new Mock<IRegexTestView>();

            var actions = new RegexActions(view.Object);
            actions.PasteFromCSharp("");

            view.VerifySet(v =>
                v.RegexText = "");
        }

        [Test]
        public void TestPasteFromCSharp_Simple()
        {
            Mock<IRegexTestView> view = new Mock<IRegexTestView>();

            var actions = new RegexActions(view.Object);
            actions.PasteFromCSharp(CSharpRegex);

            // TODO: This result looks wrong...like, the ); on the end for example...
            view.VerifySet(v =>
                v.RegexText = "\\((?<AreaCode>\\d{3})\\)\");");
        }
    }
}
