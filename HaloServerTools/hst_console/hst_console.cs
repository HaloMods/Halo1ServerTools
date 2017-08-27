using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for hst_console.
	/// </summary>
	public class hst_console
	{
		//public static HttpServer hServ;
		public static Listener listener;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// Setup a trace listener to the console
			Trace.AutoFlush = true;
			
			// First, determine the filename to use:
			string dir = Environment.CurrentDirectory + "\\";
			string baseName = "hstlogfile." + DateTime.Now.ToShortDateString().Replace("/","-");
			string ext = ".txt";

			// If this file exists, rename appropriately
			if (System.IO.File.Exists(dir + baseName + ext))
			{
				int x = 1;
				while (System.IO.File.Exists(dir + baseName + "." + Convert.ToString(x) + ext))
				{
					x++;
				}
				baseName = baseName + "." + Convert.ToString(x);
			}
            System.IO.FileStream traceLog = new System.IO.FileStream(dir + baseName + ext, System.IO.FileMode.OpenOrCreate);
			
			// Creates the new trace listener.
			System.Diagnostics.TextWriterTraceListener newListener = 
				new System.Diagnostics.TextWriterTraceListener(traceLog);
			
            // Add the listener
			Trace.Listeners.Add(newListener);
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

			// If the settings folder does not exist, create it
			if (!System.IO.Directory.Exists(Environment.CurrentDirectory + "\\settings"))
			{
				Trace.WriteLine("Settings folder was not found - it is being created.");
				System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\settings");
			}

			Thread.CurrentThread.Priority = ThreadPriority.Lowest; // We don't want anything to interfere with
			// the game servers that are being managed.
			
			//////////////////////////////////////////////////////////////
			// Initialization: Part One - Load settings from files
			//////////////////////////////////////////////////////////////

			// Open the database connection
			string dbFile = Environment.CurrentDirectory + "\\settings\\global_settings.mdb";
			if (!System.IO.File.Exists(dbFile))
			{
				Trace.WriteLine("Database file not found - " + dbFile);
				Trace.WriteLine("Could not initialize - reinstall HST to repair this problem.");
			}
			DB.OpenDatabase(dbFile);

			// Start all of the server processes
			ServerManager.ServerList = new System.Collections.ArrayList();

			// Load the server list from the database
			OleDbDataReader dr = DB.GetData("select * from servers");
			if (dr.HasRows)
			{
				while (dr.Read())
				{
					string startupFolder = dr.GetString(dr.GetOrdinal("installed_dir"));
					HaloServer h = new HaloServer(startupFolder);
					h.name = dr.GetString(dr.GetOrdinal("name"));
					h.id = Convert.ToString(dr.GetInt32(dr.GetOrdinal("server_id")));
					h._ipAddress = System.Net.IPAddress.Parse(dr.GetString(dr.GetOrdinal("ip_address")));
					h._port = dr.GetInt32(dr.GetOrdinal("port"));
					h._commandLine = dr.GetString(dr.GetOrdinal("commandline"));
					h._parameters = dr.GetString(dr.GetOrdinal("parameters"));
					ServerManager.ServerList.Add(h);
					h.Start();
				}
			}
			else
			{
				Trace.WriteLine("No servers found.");
			}
			dr.Close();
			
			string input;
			while (true)
			{
				input = Console.ReadLine().ToUpper();
				if (input == "QUIT")
				{
					// There are still child threads running - we need to fix that.
					foreach (System.Collections.DictionaryEntry de in ServerManager.ServerList)
					{
						GameServer g = (GameServer)de.Value;
						g.Stop();
					}
					break;
				}
				if (input.StartsWith("STOP"))
				{
					GameServer g = (GameServer)ServerManager.ServerList[0];
					Trace.WriteLine("Stopping Server: " + g.name);
					g.Stop();
				}
				if (input.StartsWith("START"))
				{
					GameServer g = (GameServer)ServerManager.ServerList[0];
					Trace.WriteLine("Starting Server: " + g.name);
					g.Start();
				}
				/*if (input.StartsWith("WEB"))
				{
					if (hServ.Active)
					{
						Trace.WriteLine("Web server active.");
						Trace.WriteLine("IP: " + hServ.Ip);
						Trace.WriteLine("Port: " + Convert.ToString(hServ.Port));
						Trace.WriteLine(" ");
					}
					else
					{
						Trace.WriteLine("Web Server not currently active.");
					}
				}*/
			}
		}
	}
}