using System;

namespace HaloServerTools
{
	/// <summary>
	/// Collection of static methods used globally in various functions.
	/// </summary>
	public class Utility
	{
    public static string DeUnicode(string s)
    {
      string newString = String.Empty;
      for (int x=0; x<s.Length; x++)
      {
        // This is the first char - shouldn't be an 0x00
        newString += s.Substring(x,1);
        //Skip the next char
        x++;
      }
      return newString;
    }
	}
}
