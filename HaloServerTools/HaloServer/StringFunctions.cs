using System;
using System.Text.RegularExpressions;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for StringFunctions.
	/// </summary>
	public class StringFunctions
	{
		/// <summary>
		/// Indicates whether the regular expression specified in "pattern" could be found in the "text".
		/// </summary>
		/// <param name="text">The string to search for a match</param>
		/// <param name="pattern">The pattern to find in the "text" string 
		/// (supports the *, ? and # wildcard characters).
		/// </param>
		/// <returns>Returns true if the regular expression finds a match otherwise returns false.</returns>
		/// <remarks>
		/// ?			Matches any single character (between A-Z and a-z)
		/// #			Matches any single digit. For example, 7# matches numbers that include 7 followed by another number, such as 71, but not 17. 
		/// *			Matches any one or more characters. For example, new* matches any text that includes "new", such as newfile.txt. 
		/// </remarks>
		public static bool Like(string text, string pattern, bool ignoreCase)
		{
			if(text != null && pattern != null && pattern.Length > 0)
			{
				if(pattern == null || text == null)
				{
					return false;
				}

				if(pattern == "*" || pattern == "*.*")
				{
					return true;
				}

				//Replace the pattern matching characters with temp strings
				pattern = pattern.Replace(@"*",@"³");
				pattern = pattern.Replace(@"?",@"²");
				pattern = pattern.Replace(@"#",@"´");
				pattern = pattern.Replace(@"[",@"µ");
				pattern = pattern.Replace(@"]",@"·");
				pattern = pattern.Replace(@"!",@"¸");

				//Escape all other characters
				pattern = Regex.Escape(pattern);

				//Replace the temp strings back with the required regular expressions
				pattern = pattern.Replace(@"³",@".*");
				pattern = pattern.Replace(@"²",@"[A-Za-z]");
				pattern = pattern.Replace(@"´",@"[0-9]+");
				pattern = pattern.Replace(@"µ",@"[");
				pattern = pattern.Replace(@"·",@"]");
				pattern = pattern.Replace(@"¸",@"^");

				//Add begin and end blocks
				pattern = "^" + pattern + "$";

				if(ignoreCase == false)
				{
					return Regex.IsMatch(text, pattern, RegexOptions.Compiled);
				}
				else
				{
					return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
				}
			}
			return false;
		}
		public static string[] ParseCommand(string parse1, out string cmd)
		{
			// In the case of quotes, replace all whitespace between them with ascii 0x01
			int quoteStart = parse1.IndexOf("\"");
			string s = System.Text.Encoding.Default.GetString(new byte[] {0x01});
			if ((quoteStart > 0) && (quoteStart < parse1.Length))
			{
				// Yep, we have quotes - find the end quote, if present
				int quoteEnd = parse1.IndexOf(parse1, quoteStart);
				if (quoteEnd < quoteStart) quoteEnd = parse1.Length;
				// Replace inner characters
				string newString = "";
				for (int x=0; x<parse1.Length; x++)
				{
					if ((x >= quoteStart)
						&& (x <= quoteEnd)
						&& (parse1.Substring(x, 1) == " "))
					{
						newString += s;
					}
					else
					{
						newString += parse1.Substring(x, 1);
					}
				}
				parse1 = newString;
			}
			// Strip quotes
			parse1 = parse1.Replace("\"","");
			string[] parse2 = parse1.Split(' ');
			cmd = parse2[0];
			cmd = cmd.Replace(s, " ");
			for (int x=0; x<parse2.Length; x++)
			{
				parse2[x] = parse2[x].Replace(s, " ");
			}
			return parse2;
		}
	}
}
