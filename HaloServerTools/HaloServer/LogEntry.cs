using System;

namespace HaloServerTools
{
	public class LogEntry
	{
		public DateTime timeStamp;
		public string action;
		public string[] parameters;
		public LogEntry()
		{
		}
		public LogEntry(string s)
		{
			if (s == "") return;
			string[] parsedString = s.Split('\t');
			// Remove any padded spaces
			for (int x=0; x<parsedString.Length-1; x++)
			{
				parsedString[x] = parsedString[x].Trim();
				parsedString[x] = parsedString[x].Trim(("\r\n".ToCharArray()));
			}
			int numberOfParameters = parsedString.Length - 2;
			timeStamp = Convert.ToDateTime(parsedString[0].ToString());
			action = parsedString[1];

			if (numberOfParameters > 0)
			{
				parameters = new string[numberOfParameters];
				for (int x=0; x<numberOfParameters; x++)
				{
					parameters[x] = parsedString[x+2];
				}
			}
		}
	}
}
