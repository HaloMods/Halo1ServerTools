using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using Nini.Config;
using System.IO;
using System.Threading;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for ServerExtensions.
	/// </summary>
	public class ServerExtensions
	{
		private LogfileMonitor _logfileMonitor;
		public Interpreter _logInterpreter;
		//private IrcBot _ircBot;
		//private Thread botTh;
		//public Listener listener;
		public AdminList Admins;

		public Banlist banlist;
		public Delagginator delag;

		// Settings
		public string _dataFolder;
		private string _logfileName;
		private string _banlistFileName;
        
		public ServerExtensions(string settingsFile)
		{
			Trace.WriteLine("Loading Server Extensions Settings...");
			LoadSettings(settingsFile);
			Trace.WriteLine("Done");
			//_ircBot = new IrcBot();
			//botTh = new Thread(new ThreadStart(_ircBot.Connect));
			//botTh.Start();

			// Initialize the Logfile Monitor
			// Step 1: Get the name of the logfile from the init.txt file
			//_logfileName = "haloserver.log";  // Add the code for that later
	
			// Step 2: Initialize and start the monitor.
			Trace.WriteLine("Initializing Logfile Monitor Object");
			_logfileMonitor = new LogfileMonitor(_dataFolder + "\\" + _logfileName);
			_logfileMonitor.BeforeLog += new LogfileMonitor.LogEventHandler(_logfileMonitor_BeforeLog);

			// Initialize banlist object
			 banlist = new Banlist(_dataFolder + "\\" + _banlistFileName);

			//StartServices();
			//listener = new Listener()
			
			Trace.WriteLine("Done");
		}

		public void StartServices()
		{
			//_logfileMonitor.Start();
			//Trace.WriteLine("Starting logfile monitoring thread");
		}

		public void ReInitializeLogfileMonitor()
		{
			_logfileMonitor = new LogfileMonitor(_dataFolder + "\\" + _logfileName);
			_logfileMonitor.BeforeLog += new LogfileMonitor.LogEventHandler(_logfileMonitor_BeforeLog);
			_logfileMonitor.OverrideSize(0);
			_logfileMonitor.Start();
		}

		public void StopServices()
		{
			_logfileMonitor.Stop();
		}

		public void LoadSettings (string filename)
		{
			IConfigSource source = new IniConfigSource(filename);
			string installedFolder = source.Configs["Paths"].GetString("InstalledFolder");
			_dataFolder = source.Configs["Paths"].GetString("DataFolder");
			_logfileName = source.Configs["Filenames"].GetString("Logfile");
			//********** This may be dynamic - not sure
			//If so, this will not be hard coded, but in the settings.
			_banlistFileName = "banned.txt";
			ReadAdmins();
		}
		public class PlayerRecord
		{
			public string name;
			public string keyhash;
			public int id;
		}
		public class AdminList : System.Collections.Hashtable
		{
			public PlayerRecord LocateById(int id)
			{
				foreach (PlayerRecord p in base.Values)
				{
					//PlayerRecord p = (PlayerRecord)d.Value;
					if (p.id == id) return p;
				}
				return null;
			}
		}
		public void ReadAdmins()
		{
			try
			{
				// Setup the admins array
				Admins = new AdminList();
				OleDbDataReader drAdmins = DB.GetData("select * from haloce_server_admins");
				if (drAdmins.HasRows)
				{
					while (drAdmins.Read())
					{
						string name = drAdmins.GetString(drAdmins.GetOrdinal("name"));		
						string keyhash = drAdmins.GetString(drAdmins.GetOrdinal("key_hash"));
						int id = drAdmins.GetInt32(drAdmins.GetOrdinal("id"));
						PlayerRecord pr = new PlayerRecord();
						pr.id = id; pr.keyhash = keyhash; pr.name = name;
						if (!Admins.Contains(keyhash)) Admins.Add(keyhash, pr);
					}
				}
				drAdmins.Close();
			}
			catch
			{
				Admins = new AdminList();
				Trace.WriteLine("ERROR: Failed to load admins!");
			}
		}
		
		// After restarting the server process, resync to the new process
		public void Bind(ref ConsoleWindow consoleWindow)
		{
			_logInterpreter = new Interpreter(ref consoleWindow);
			_logfileMonitor.Start();
				
		}

		private void _logfileMonitor_BeforeLog (object sender, LogfileMonitor.LogEventArgs e)
		{
			for (int x=0; x<e.newLines.Length-1; x++)
			{
				if (e.newLines[x] != null)  // Make sure this is a valid entry
				{
					if (e.newLines[x] != "")
					{
						Console.WriteLine(e.newLines[x]); // echo the log to the console
						_logInterpreter.ProcessLogEntry(e.newLines[x]);
					}
				}
			}
			Trace.WriteLine("  \r\n");
		}

		public void ProcessLog()
		{
			string[] s = _logfileMonitor.GetLines(0);
			for (int x=0; x<s.Length-1; x++)
			{
				//Trace.WriteLine(s[x]); // echo the log to the console
				//_logInterpreter.ProcessLogEntry(s[x]);
			}
		}
	}
}
