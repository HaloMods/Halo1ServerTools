using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using Nini.Config;
using System.Threading;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for HaloServerTools.
	/// </summary>
	public class HST
	{
		public HST()
		{
			StartUp();
		}
		public void StartUp()
		{
			// Setup a file0based Trace Listener
			Trace.AutoFlush = true;
			
			// First, determine the filename to use:
			string dir = Environment.CurrentDirectory + "\\";
			string baseName = "hstlogfile." + DateTime.Now.ToShortDateString().Replace("/","-");
			string ext = ".txt";

			// If this file exists, rename appropriately
			//if (System.IO.File.Exists(dir + baseName + ext))
			//{
			//	int x = 1;
			//	while (System.IO.File.Exists(dir + baseName + "." + Convert.ToString(x) + ext))
			//	{
			//		x++;
			//	}
			//	baseName = baseName + "." + Convert.ToString(x);
			//}
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

			// Read the settings.ini file for global settings
			string settingsFile = Environment.CurrentDirectory + "\\settings\\settings.ini";
			if (!System.IO.File.Exists(settingsFile))
			{
				Trace.WriteLine("Applications settings file (" + settingsFile + ") does not exist!");
				Trace.WriteLine("Could not initialize - reinstall HST to repair this problem.");
				return;
			}
			
			IConfigSource source = new IniConfigSource(settingsFile);
			string installedFolder = source.Configs["Paths"].GetString("InstalledFolder");
			string dataFolder = source.Configs["Paths"].GetString("DataFolder");
			
			ServerManager.ServerList = new System.Collections.ArrayList();
			HaloServer h = new HaloServer(installedFolder);
			h._ipAddress = System.Net.IPAddress.Parse(source.Configs["Settings"].GetString("IPAddress"));
			h._port = Convert.ToInt32(source.Configs["Settings"].GetString("Port"));
			h._commandLine = "haloceded.exe";
			h._parameters = "-ip " + h._ipAddress + " " +
				"-port " + Convert.ToString(h._port) + " " +
				"-path \"" + dataFolder + "\"";
			ServerManager.ServerList.Add(h);
		}
	}
}
