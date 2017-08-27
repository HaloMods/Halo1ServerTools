/*
..........................................................
 Winkler Comparer
 Author: febrl Open Source Project
 Ported to c# by Justin Draper

 Utilitzes the winkler string comparison algorithm to
 compare two strings and returns a number between 0 and 1,
 with 0 indicating that the string are completely
 dissimmilar and 1 indicating that the string are
 completely identical.
..........................................................
*/

using System;

namespace HaloServerTools
{
	public class Winkler
	{
		public static double WinklerCompare (string str1, string str2)
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
			{
				if (str1.ToCharArray()[same] != str2.ToCharArray()[same]) break;
			}
			same--;
			if (same > 4) same = 4;

			common1 = (int)common1;
			double w;
			w = 1 / 3 *(common1 / (int)(len1) + common1 / (int)(len2) + (common1-transposition) / common1);

			double wn;
			wn = w + same * 0.1 * (1.0 - w);

			return wn;
		}
		public static int Max(int a, int b)
		{
			if (a>b) return a;
			return b;
		}
		public static int Min(int a, int b)
		{
			if (a<b) return a;
			return b;
		}
	}
}

/*
Original python source code

# =============================================================================

def winkler(str1, str2):
  """Return approximate string comparator measure (between 0.0 and 1.0)

  USAGE:
    score = winkler(str1, str2)

  ARGUMENTS:
    str1  The first string
    str2  The second string
 
  DESCRIPTION:
    As desribed in 'An Application of the Fellegi-Sunter Model of
    Record Linkage to the 1990 U.S. Decennial Census' by William E. Winkler
    and Yves Thibaudeau.

    Based on the 'jaro' string comparator, but modifies it according to wether
    the first few characters are the same or not.
  """

  # Quick check if the strings are the same - - - - - - - - - - - - - - - - - -
  #
  if (str1 == str2):
    return 1.0

  len1 = len(str1)
  len2 = len(str2)
  halflen = Max(len1,len2) / 2 + 1

  ass1 = ''  # Characters assigned in str1
  ass2 = ''  # Characters assigned in str2
  workstr1 = str1
  workstr2 = str2

  common1 = 0  # Number of common characters
  common2 = 0

  # Analyse the first string  - - - - - - - - - - - - - - - - - - - - - - - - -
  #
  for i in range(len1):
    start = Max(0,i-halflen)
    end   = Min(i+halflen+1,len2)
    index = workstr2.find(str1[i],start,end)
    if (index > -1):  # Found common character
      common1 += 1
      ass1 = ass1 + str1[i]
      workstr2 = workstr2[:index]+'*'+workstr2[index+1:]

  # Analyse the second string - - - - - - - - - - - - - - - - - - - - - - - - -
  #
  for i in range(len2):
    start = Max(0,i-halflen)
    end   = Min(i+halflen+1,len1)
    index = workstr1.find(str2[i],start,end)
    if (index > -1):  # Found common character
      common2 += 1
      ass2 = ass2 + str2[i]
      workstr1 = workstr1[:index]+'*'+workstr1[index+1:]

  if (common1 != common2):
    print 'error:Winkler: Something is wrong. String 1: "%s"' % (str1) + \
          ', string2: "%s", common1: %i, common2: %i' % \
          (str1, common1, common2) + ', common should be the same.'
    common1 = double(common1+common2) / 2.0  ##### This is just a fix #####

  if (common1 == 0):
    return 0.0

  # Compute number of transpositions  - - - - - - - - - - - - - - - - - - - - -
  #
  transposition = 0
  for i in range(len(ass1)):
    if (ass1[i] != ass2[i]):
      transposition += 1
  transposition = transposition / 2.0

  # Now compute how many characters are common at beginning - - - - - - - - - -
  #
  Minlen = Min(len1,len2)
  for same in range(Minlen+1):
    if (str1[:same] != str2[:same]):
      break
  same -= 1
  if (same > 4):
    same = 4

  common1 = double(common1)
  w = 1./3.*(common1 / double(len1) + common1 / double(len2) + \
           (common1-transposition) / common1)

  wn = w + same*0.1 * (1.0 - w)

  # A log message for high volume log output (level 3) - - - - - - - - - - - -
  #
  print '3:  Winkler comparator string 1: "%s", string 2: "%s"' % (str1, str2)
  print '3:    Common: %i' % (common1)
  print '3:    Assigned 1: %s, assigned 2: %s' % (ass1, ass2)
  print '3:    Transpositions: %i' % (transposition)
  print '3:    Same at beginning: %i' % (same)
  print '3:    Jaro weight: %f ' % (w)
  print '3:    Final approximate string weight: %f' % (wn)

  return wn

# =============================================================================

*/

// Sourceforge CVS homepage: http://cvs.sourceforge.net/viewcvs.py/febrl/febrl/
