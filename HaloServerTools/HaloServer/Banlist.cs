using System;
using System.Collections;
using System.IO;

namespace HaloServerTools
{
	/// <summary>
	/// Encapsulates an interface to a HaloCE banlist file.
	/// </summary>
	public class Banlist
	{
		private string m_filename;
		
		public class BanlistEntry
		{
			public string name;
			public string keyHash;
			public string banCount;
			public string banEndDate;
			public BanlistEntry(string s)
			{
				// # Name, CD key hash, ban count, ban end date
				// Comma delimited
				string[] parts = s.Split(',');
				if (parts.Length == 4)
				{
					name = parts[0];
					keyHash = parts[1];
					banCount = parts[2];
					banEndDate = parts[3];
				}
			}
		}
		
		public BanlistEntry[] Entries;

		public Banlist(string filename)
		{
			m_filename = filename;
			Entries = new BanlistEntry[0];
		}
		private void ParseFile()
		{
			if (!File.Exists(m_filename))
			{
				System.Diagnostics.Trace.WriteLine("Error: Banlist file not found: " + m_filename);
				return;
			}
			
			try
			{
				FileStream fs = new FileStream(m_filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				StreamReader sr = new StreamReader(fs);
				string data = sr.ReadToEnd();
				data = data.Replace("\r\r\n","\n"); // Strange delimiter sequence in this file... o_O
				string[] lines = data.Split('\n');
				
				Entries = new BanlistEntry[lines.Length];
				
				for (int x=1; x<lines.Length-1; x++)
				{
					if (lines[x].Length > 5) Entries[x-1] = new BanlistEntry(lines[x]);
				}
			}
			catch
			{
				System.Diagnostics.Trace.WriteLine("Error: Could not read banlist file: " + m_filename);
			}
		}
		// Manually add a player to the banlist file
		public bool AddPlayer(Player p)
		{
			try	
			{
				// # Name, CD key hash, ban count, ban end date				// A Stoner,124d23cdf2ecd85f106d4df26347dcee,1,2004-09-19 03:02:08
				
				// Auto 10-day ban
				FileStream fs = new FileStream(m_filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
				StreamWriter sw = new StreamWriter(fs);
				sw.BaseStream.Seek(0, SeekOrigin.End);
				string d = ",";
				string s = p.name + d + p.keyHash + d + "1" + d + "--" + "\r\r\n";
				sw.Write(s);
				sw.Close();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public class BannedPlayer
		{
			public string name;
			public int banlistIndex;
			public BannedPlayer(BanlistEntry b, int index)
			{
				name = b.name;
				banlistIndex = index;
			}
		}
		public ArrayList FindPlayerByName(string s)
		{
			ParseFile();
			ArrayList r = new ArrayList();
			if (Entries.Length > 1)
			{
				for (int x=0; x<Entries.Length-1; x++)
				{
					if (((object)Entries[x]) != null)
					{
						if ((Entries[x].name == s) || (StringFunctions.Like(Entries[x].name, s, true)))
						{
							BannedPlayer b = new BannedPlayer(Entries[x], x);
							r.Add(b);
						}
					}
				}
			}
			return r;
		}
	}
}
