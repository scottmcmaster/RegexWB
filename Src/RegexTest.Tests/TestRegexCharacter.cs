using System;
using NUnit.Framework;

namespace RegexTest
{
    [TestFixture]
    public class TestRegexCharacter
    {
        public TestRegexCharacter()
        {
        }

        [Test]
        public void TestSpace()
        {
            var reChar = new RegexCharacter(new RegexBuffer(" "));
            Assert.IsFalse(reChar.Special);
            Assert.AreEqual(RegexCharacter.SpaceDescription, reChar.ToString(0));
        }

        [Test]
        public void TestEscapeSpaceNoBackRef()
        {
            var reChar = new RegexCharacter(new RegexBuffer("\\ "));
            Assert.IsFalse(reChar.Special);
            Assert.AreEqual(RegexCharacter.SpaceDescription, reChar.ToString(0));
        }

        [Test]
        public void TestEscapeBoringCharacterNoBackRef()
        {
            var reChar = new RegexCharacter(new RegexBuffer("\\Q"));
            Assert.IsFalse(reChar.Special);
            Assert.AreEqual("Q", reChar.ToString(0));
        }

        [Test]
        public void TestBackRef()
        {
            var reChar = new RegexCharacter(new RegexBuffer("\\k<blah>"));
            Assert.IsTrue(reChar.Special);
            Assert.AreEqual(string.Format(RegexCharacter.BackreferenceToMatchDescriptionTemplate, "blah"),
                reChar.ToString(0));
        }
    }
}
