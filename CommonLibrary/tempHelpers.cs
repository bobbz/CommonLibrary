using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class tempHelpers
    {
        #region "Regular expression helper"
        /// <summary>
        /// Similar string.Format, this function replaces the place holders with regular expressions, and escapes special regex chars for the rest of the string.
        /// </summary>
        /// <param name="originalString">The original string with place holders.</param>
        /// <param name="replacements">A list of string to place in the place holders.</param>
        /// <param name="regexp">A list of boolean values that specify which strings in the replacements parameter are regexp or not.</param>
        /// <returns></returns>
        public static string RegexFormatter(string originalString, string[] replacements, bool[] regexp)
        {
            if (originalString == null || replacements == null || regexp == null)
            {
                throw new System.ArgumentNullException();
            }
            if (replacements.Length != regexp.Length)
            {
                throw new System.ArgumentException("Array arguments are related, and are expected to be the same length.");
            }
            int i = 0;
            int pos;
            StringBuilder newString = new StringBuilder();
            while (originalString.Length > 0)
            {
                Regex r = new Regex(@".*(\{" + i.ToString() + @"(:[^}]+)?\}).*");
                Match m = r.Match(originalString);
                if (m.Success && m.Groups.Count > 1)
                {

                    pos = m.Groups[1].Index;
                    newString.Append(Regex.Escape(originalString.Substring(0, pos)));
                    if (regexp[i])
                    {
                        newString.Append(replacements[i]);
                    }
                    else
                    {
                        newString.Append(Regex.Escape(replacements[i]));
                    }
                    originalString = originalString.Substring(pos + m.Groups[1].Length);
                }
                else
                {
                    newString.Append(Regex.Escape(originalString));
                    originalString = string.Empty;
                }

                i++;
            }
            return newString.ToString();
        }
    }
}
