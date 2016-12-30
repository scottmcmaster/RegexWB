using System;
using System.Collections.Generic;
using System.Text;

namespace RegexTest
{
    public interface IRegexTestView
    {
        string Output { get; set; }
        bool OneString { get; }
        string Strings { get; }
        bool MatchEvaluator { get; }
        string ReplaceString { get; set; }
        string RegexText { get; set; }
        bool IgnoreWhitespace { get; set; }
        bool IgnoreCase { get; set; }
        bool Compiled { get; set;  }
        bool ExplicitCapture { get; set; }
        bool Singleline { get; set; }
        bool Multiline { get; set; }
        void SaveValues();
    }
}
