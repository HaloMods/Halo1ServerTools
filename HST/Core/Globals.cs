using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Core
{
	/// <summary>
	/// Core globals
	/// </summary>
	public sealed class Globals
	{
		public static Interfaces.WindowsInterface HaloConsole = null;
		public static Interfaces.MemoryInterface HaloMemory = null;

		/// <summary>
		/// Setup all the global values
		/// </summary>
		public static void Initialize()
		{
			// Find the Halo dedicated window
		}

		#region Settings
		/// <summary>
		/// Load the HST Settings
		/// </summary>
		public static void LoadSettings()
		{
		}

		/// <summary>
		/// Save the HST settings
		/// </summary>
		public static void SaveSettings()
		{
		}
		#endregion

		#region Utilities
		#region Split(string value, string term)
		/// <summary>
		/// Splits up a string
		/// </summary>
		/// <param name="value">The string to be split</param>
		/// <param name="term">The termintor to use</param>
		/// <returns></returns>
		public static string[] Split(string value, string term)
		{
			Regex rx = new Regex(term);
			
			return rx.Split(value);
		}
		#endregion

		#region UnicodeToASCII(string s)
		public static string UnicodeToASCII(string s)
		{
			StringBuilder newString = new StringBuilder();
			string newString = String.Empty;
			for (int x = 0; x < s.Length; x+=2)
			{
				// This is the first char - shouldn't be an 0x00
				newString.Append( s.Substring(x,1) );
			}
			return newString;
		}
		#endregion

		#region StringHasPattern(string text, string pattern, bool ignoreCase)
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
		public static bool StringHasPattern(string text, string pattern, bool ignoreCase)
		{
			if(text != null && pattern != null && pattern.Length > 0)
			{
				if(pattern == null || text == null)
					return false;

				if(pattern == "*" || pattern == "*.*")
					return true;

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
					return Regex.IsMatch(text, pattern, RegexOptions.Compiled);
				else
					return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			return false;
		}
		#endregion

		#region ParseCommand(string parse1, out string cmd)
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
				for (int x = 0; x < parse1.Length; x++)
				{
					if ((x >= quoteStart)
						&& (x <= quoteEnd)
						&& (parse1.Substring(x, 1) == " "))
					{
						newString += s;
					}
					else
						newString += parse1.Substring(x, 1);
				}
				parse1 = newString;
			}
			// Strip quotes
			parse1 = parse1.Replace("\"","");
			string[] parse2 = parse1.Split(' ');
			cmd = parse2[0];
			cmd = cmd.Replace(s, " ");
			for (int x = 0; x < parse2.Length; x++)
				parse2[x] = parse2[x].Replace(s, " ");
			return parse2;
		}
		#endregion

		#region 
		/// <summary>
		/// Utilitzes the winkler string comparison algorithm to
		/// compare two strings and returns a number between 0 and 1,
		/// with 0 indicating that the string are completely
		///	dissimmilar and 1 indicating that the string are
		///	completely identical.
		/// </summary>
		/// <param name="str1"></param>
		/// <param name="str2"></param>
		/// <returns></returns>
		public static double WinklerCompare(string str1, string str2)
		{
			// Quick check if the strings are the same
			if (str1 == str2) return 1.0;
		
			int len1 = str1.Length;
			int len2 = str2.Length;
			int halflen = Max(len1,len2) / 2 + 1;

			string ass1 = "";  // Characters assigned in str1
			string ass2 = "";  // Characters assigned in str2
			string workstr1 = str1;
			string workstr2 = str2;

			int common1 = 0;  // Number of common characters
			int common2 = 0;

			int start;
			int end;
			int index;

			// Analyse the first string
			for (int i=0; i<len1; i++)
			{
				start = Max(0,i-halflen);
				end   = Min(i+halflen+1,len2);
				index = workstr2.IndexOf(str1[i],start,end-start);
				if (index > -1) // Found common character
				{
					common1++;
					ass1 += str1[i];
					workstr2 = workstr2.Substring(1,index) + "*" + workstr2.Substring(index+1,workstr2.Length-(index+1));
					//workstr2 = workstr2.Substring(1,index+1) + "*" + workstr2.Substring(index+1,workstr2.Length-(index+1));
				}
			}

			// Analyse the second string
			for (int i=0; i<len2; i++)
			{
				start = Max(0,i-halflen);
				end   = Min(i+halflen+1,len1);
				index = workstr1.IndexOf(str2[i],start,end-start);
				if (index > -1)  // Found common character
				{
					common2++;
					ass2 += str2[i];
					workstr1 = workstr1.Substring(1, index) + "*" + workstr1.Substring(index+1,workstr1.Length-(index+1));
					//workstr1 = workstr1.Substring(1, index+1) + "*" + workstr1.Substring(index+1,workstr1.Length-(index+1));
				}
			}

			if (common1 != common2)
			{
				// Something is wrong - common number of letters between the two
				// string should be the same.
				common1 = (int)((float)(common1+common2) / 2.0);  // This is just a fix
			}

			if (common1 == 0) return 0.0;  // No letters in common mean ords are completetl different

			// Compute number of transpositions
			int transposition = 0;
			for (int i=0; i<ass1.Length; i++)
			{
				if (ass1.ToCharArray()[i] != ass2.ToCharArray()[i])
				{
					transposition += 1;
					transposition = (int)(transposition / 2.0);
				}
			}
		
			// Now compute how many characters are common at beginning
			int Minlen = Min(len1,len2);
			int same = 0;
			for (same=0; same<Minlen; same++)
				if (str1.ToCharArray()[same] != str2.ToCharArray()[same]) break;
			same--;
			if (same > 4) same = 4;

			common1 = (int)common1;
			double w;
			w = 1 / 3 *(common1 / (int)(len1) + common1 / (int)(len2) + (common1-transposition) / common1);

			double wn;
			wn = w + same * 0.1 * (1.0 - w);

			return wn;
		}

		// Use Math instead mb?
		private static int Max(int a, int b)
		{
			if (a>b) return a;
			return b;
		}
		// Use Math instead mb?
		private static int Min(int a, int b)
		{
			if (a<b) return a;
			return b;
		}
		#endregion
		#endregion
	};

	public struct Coordinate
	{
		public float X;
		public float Y;
		public float Z;
	};

	/// <summary>
	/// Loads and Saves the HST settings
	/// </summary>
	internal class Settings
	{
		private XmlDocument _xmlD;
		private XmlNode _rootNode;

		public class XmlNodeArrayList : System.Collections.ArrayList
		{
			public int Add(XmlNode value)
			{
				return base.Add (value);
			}

			public bool Contains(XmlNode item)
			{
				return base.Contains (item);
			}

			public void Insert(int index, XmlNode value)
			{
				base.Insert (index, value);
			}

			public new XmlNode this[int index]
			{
				get { return (XmlNode)base[index]; }
				set { base[index] = value; }
			}
		};

		public Settings(string filename)
		{
			_xmlD = new XmlDocument();
			_xmlD.Load(filename);
			_rootNode = _xmlD.SelectSingleNode("/*");
		}

		public XmlNodeArrayList GetGroup (string name)
		{
			return GetGroup (name, _rootNode);
		}
		public XmlNodeArrayList GetGroup (string name, XmlNode baseNode)
		{
			XmlNodeArrayList list = new XmlNodeArrayList();
			
			foreach (XmlNode n in baseNode)
			{
				if (n.Name == name) list.Add(n);
			}
			return list;
		}

		public string ReadProperty (string name, XmlNode baseNode)
		{
			if (baseNode.HasChildNodes == false) return null;
			
			foreach (XmlNode n in baseNode)
			{
				if (n.Name == name) return n.InnerText;
			}
			return null;
		}
	};
}