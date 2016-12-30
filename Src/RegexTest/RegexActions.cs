using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexTest
{
    public class RegexActions
    {
        private readonly IRegexTestView view;

        public RegexActions(IRegexTestView view)
        {
            this.view = view;
        }

        public void Replace()
        {
            view.SaveValues();
            Regex regex = null;
            try
            {
                regex = CreateRegex();
            }
            catch (Exception ex)
            {
                view.Output = ex.ToString();
                return;
            }

            string[] strings;
            // if checked, pass all lines as a single block
            if (view.OneString)
            {
                strings = new string[1];
                strings[0] = view.Strings;
            }
            else
            {
                strings = Regex.Split(view.Strings, @"\r\n");
                //strings = Strings.Text.Split('\n\r');
            }

            ReplaceMatchEvaluator replacer = null;

            if (view.MatchEvaluator)
            {
                replacer = new ReplaceMatchEvaluator(regex, view.ReplaceString);
                string output = replacer.CreateAndLoadClass();
                if (output != null)
                {
                    view.Output = output;
                    return;
                }
            }

            StringBuilder outString = new StringBuilder();
            string replace = view.ReplaceString ?? "";
            foreach (string s in strings)
            {
                outString.Append(String.Format("Replacing: {0}\r\n", s));

                string output = null;
                if (view.MatchEvaluator)
                {
                    outString.Append("  with a custom MatchEvaluator\r\n");
                    output = regex.Replace(s, replacer.MatchEvaluator);
                }
                else
                {
                    outString.Append(String.Format("  with: {0}\r\n", replace));
                    output = regex.Replace(s, replace);
                }
                outString.Append(String.Format("  result: {0}\r\n", output));
            }
            view.Output = outString.ToString();
        }

        public void Split()
        {
            view.SaveValues();
            Regex regex = null;
            try
            {
                regex = CreateRegex();
            }
            catch (Exception ex)
            {
                view.Output = ex.ToString();
                return;
            }

            string[] strings;
            // if checked, pass all lines as a single block
            if (view.OneString)
            {
                strings = new string[1];
                strings[0] = view.Strings;
            }
            else
            {
                strings = Regex.Split(view.Strings, @"\r\n");
                //strings = Strings.Text.Split('\n\r');
            }

            StringBuilder outString = new StringBuilder();
            foreach (string s in strings)
            {
                outString.Append(String.Format("Splitting: {0}\r\n", s));

                string[] arr = regex.Split(s);

                int index = 0;
                foreach (string split in arr)
                {
                    outString.Append(String.Format("    [{0}] => {1}\r\n", index, split));
                    index++;
                }
            }
            view.Output = outString.ToString();
        }

        private Regex CreateRegex()
        {
            RegexOptions regOp = CreateRegexOptions();
            return new Regex(view.RegexText, regOp);
        }

        private RegexOptions CreateRegexOptions()
        {
            RegexOptions regOp = new RegexOptions();
            if (view.IgnoreWhitespace)
            {
                regOp |= RegexOptions.IgnorePatternWhitespace;
            }
            if (view.IgnoreCase)
            {
                regOp |= RegexOptions.IgnoreCase;
            }
            if (view.Compiled)
            {
                regOp |= RegexOptions.Compiled;
            }
            if (view.ExplicitCapture)
            {
                regOp |= RegexOptions.ExplicitCapture;
            }
            if (view.Singleline)
            {
                regOp |= RegexOptions.Singleline;
            }
            if (view.Multiline)
            {
                regOp |= RegexOptions.Multiline;
            }

            return regOp;
        }

        public void PasteFromCSharp(string value)
        {
            // first, get rid of the "Regex regex line, if it exists"
            Regex regex2 = new Regex(@"
				^.+?new\ Regex\(@""(?<Rest>.+)
				",
                RegexOptions.Multiline |
                RegexOptions.ExplicitCapture |
                RegexOptions.Singleline |
                RegexOptions.IgnorePatternWhitespace);

            Match m = regex2.Match(value);
            if (m.Success)
            {
                value = m.Groups["Rest"].Value;
            }

            // get rid of the leading whitespace on each line...
            Regex regex = new Regex(@"
				^\s+
				",
                RegexOptions.Multiline |
                RegexOptions.ExplicitCapture |
                RegexOptions.IgnorePatternWhitespace);

            value = regex.Replace(value, "");

            // see if there is a " and options after the string...
            Regex regex3 = new Regex(@"
				(?<Pattern>.+)^\s*"",(?<Rest>.+)
				",
                RegexOptions.Multiline |
                RegexOptions.ExplicitCapture |
                RegexOptions.Singleline |
                RegexOptions.IgnorePatternWhitespace);

            m = regex3.Match(value);
            if (m.Success)
            {
                value = m.Groups["Pattern"].Value;

                // clear all the patterns, and then set the ones
                // that are on...
                view.IgnoreCase = false;
                view.IgnoreWhitespace = false;
                view.Multiline = false;
                view.Singleline = false;
                view.Compiled = false;
                view.ExplicitCapture = false;

                string rest = m.Groups["Rest"].Value;
                if (rest.IndexOf("IgnoreCase") != -1)
                    view.IgnoreCase = true;

                if (rest.IndexOf("IgnorePatternWhitespace") != -1)
                    view.IgnoreWhitespace = true;

                if (rest.IndexOf("Multiline") != -1)
                    view.Multiline = true;

                if (rest.IndexOf("Singleline") != -1)
                    view.Singleline = true;

                if (rest.IndexOf("Compiled") != -1)
                    view.Compiled = true;

                if (rest.IndexOf("ExplicitCapture") != -1)
                    view.ExplicitCapture = true;
            }

            // change any double "" to "
            view.RegexText = value.Replace("\"\"", "\"");
        }
    }
}
