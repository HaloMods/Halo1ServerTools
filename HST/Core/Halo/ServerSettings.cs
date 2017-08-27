using System;
using System.Collections;
using System.IO;

namespace Core.Halo
{
	/// <summary>
	/// Summary description for ServerSettings.
	/// </summary>
	public class ServerSettings
	{
		#region Banlist
		public class BanlistEntry
		{
			public string Name = string.Empty;
			public string KeyHash = string.Empty;
			public string BanCount = string.Empty;
			public string BanEndDate = string.Empty;

			public static readonly BanlistEntry Null;
			static BanlistEntry()
			{
				Null = new BanlistEntry();
			}

			private BanlistEntry() {}

			public BanlistEntry(string s)
			{
				// # Name, CD key hash, ban count, ban end date
				// Comma delimited
				string[] parts = s.Split(',');
				if (parts.Length == 4)
				{
					Name = parts[0];
					KeyHash = parts[1];
					BanCount = parts[2];
					BanEndDate = parts[3];
				}
			}

			public override string ToString()
			{
				return string.Format("{0},{1},{2},{3}", Name, KeyHash, BanCount, (BanEndDate));
			}

			public string ToString(string end)
			{
				return string.Format("{0},{1},{2},{3}{4}", Name, KeyHash, BanCount, BanEndDate, end);
			}
		};

		private static ArrayList Banlist;
		/// <summary>
		/// Gets the banned entry at index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public BanlistEntry BanIndex(int index)
		{
			if(Banlist == null || index < 0) return BanlistEntry.Null;
			else return Banlist[index] as BanlistEntry;
		}

		private static StreamReader BanlistFileReader;
		private static StreamWriter BanlistFileWriter;
		public static void BanlistOpen(string file)
		{
			BanlistFileReader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			BanlistFileWriter = new StreamWriter(new FileStream(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite));
			Banlist = new ArrayList();
		}
		public static void BanlistClose()
		{
			if(BanlistFileReader != null)
			{
				BanlistFileReader.Close();
				BanlistFileReader = null;
			}
			if(BanlistFileWriter != null)
			{
				BanlistFileWriter.Close();
				BanlistFileWriter = null;
			}
			if(Banlist != null)
			{
				Banlist.Clear();
				Banlist = null;
			}
		}
		public static void BanlistRead()
		{
			string[] lines = BanlistFileReader.ReadToEnd().Replace("\r\r\n", "\n").Split('\n');
			BanlistFileReader.BaseStream.Position = 0;
			foreach(string line in lines)
				Banlist.Add(new BanlistEntry(line));

		}
		public static void BanlistWrite()
		{
			BanlistFileWriter.Write("# Name, CD key hash, ban count, ban end date\r\r\n");
			foreach(BanlistEntry entry in Banlist)
			{
				BanlistFileWriter.Write(entry.ToString("\r\r\n"));
				BanlistFileWriter.Flush();
			}
		}
		#endregion
	};
}