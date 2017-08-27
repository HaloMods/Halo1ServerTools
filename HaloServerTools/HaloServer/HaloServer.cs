/* ---------------------------------------------------------------
 * Halo Server Tools, MultiGame Server Hosting Management System
 * (c)2004, EPG Servers, LLC.
 * ---------------------------------------------------------------
 * Class       : HaloServerTools.HaloServer : GameServer
 * Description : Extends the GameServer base class to encapsulate
 *               a Halo PC or CE dedicated server.
 * Author      : Justin Draper
 * Co-Authors  : None
 * ---------------------------------------------------------------
 * This code is protected under the copyright laws of the
 * United States of America, and is confidential.  Use or
 * posession of this code without express written consent of
 * the copyright holder is expressly forbidden and will be
 * prosecuted to the fullest extent allowed by law.
 * ---------------------------------------------------------------
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Extends the GameServer base class to encapsulate
	/// a Halo PC or CE dedicated server.
	/// </summary>
	public class HaloServer : GameServer
	{
		public enum ServerType { pc, ce }

		public ServerExtensions _serverExtensions;
		public Delagginator delag;
		private ConsoleWindow _consoleWindow;
		private bool m_active; // Used to store server status

		// Constructor - Initialize all of the objects		
		public HaloServer(string startupFolder)
		{
			_startupFolder = startupFolder;
			ProcMonitor = new ProcessMonitor();
			ProcMonitor.ProcessCrash += new ProcessMonitor.ProcessEventHandler(ProcessCrashHandler);
			_serverExtensions = new ServerExtensions(Environment.CurrentDirectory + "\\settings\\settings.ini");
		}
		~HaloServer()
		{
			Stop();
		}

		public override bool Active()
		{
			return m_active;
		}

		public override void ProcessCrashHandler(object o, HaloServerTools.ProcessMonitor.ProcessEventArgs e)
		{
			// If the process is stable, restart it - if not, log the event.
			if (!IntentionalStop) ProcMonitor.NumberOfCrashes++;
			if (e.Stable)
			{
				Start();
			}
			else
			{
				Trace.WriteLine("Unstable Process: " + "[" + id + "] " + name);
				Trace.WriteLine("Process restart aborted - contact the system administrator.");
			}
		}
		
		// Start the server
		public override void Start()
		{
			if (!System.IO.Directory.Exists(_startupFolder))
			{
				Trace.WriteLine("Startup folder not found for server: " + name);
				Trace.WriteLine("Unable to initialize process.");
				return;
			}
			
			// Replace parameters as necessary
			_parameters = _parameters.Replace("%ip%", _ipAddress.ToString());
			_parameters = _parameters.Replace("%port%", Convert.ToString(_port));

			StartNewProcess();
			if (BindServerExtensions())
			{
				m_active = true;
				IntentionalStop = false;
			}
			else
			{
				Trace.WriteLine("Could not bind server extensions!");
				Trace.WriteLine("Error: Stopping server process!");
				Stop();
			}
		}

		public void StartNewProcess()
		{
			// Create a new process object
			proc = new System.Diagnostics.Process();
			
			// Check and see if the previously logged process is still running
			bool pickupProcess = false;
			if (File.Exists(_startupFolder + "\\proclog"))
			{
				StreamReader sr = new StreamReader(new FileStream(_startupFolder + "\\proclog", FileMode.Open));
				int procID = Convert.ToInt32(sr.ReadLine());
				string windowTitle = sr.ReadLine();
				sr.Close();
				
				// Check for a process match
				try
				{
					Process tempProcess = Process.GetProcessById(procID);
					if (!((object)tempProcess == null))
					{
						// Verify the window title
						if (tempProcess.MainWindowTitle == windowTitle)
						{
							// Yep - process is still running
							Trace.WriteLine("Picked up on previous process.");
							pickupProcess = true;
							proc = tempProcess;
						}
					}
				}
				catch
				{
					// Process must not exist
					pickupProcess = false;
				}
			}
			
			if (!pickupProcess)
			{
				proc = new System.Diagnostics.Process();
				proc.StartInfo.FileName = _startupFolder + "\\" + _commandLine;
				proc.StartInfo.WorkingDirectory = _startupFolder;
				proc.StartInfo.Arguments = _parameters;
				proc.Start();
				System.Threading.Thread.Sleep(1000);
				// Log the process
				if (File.Exists(_startupFolder + "\\proclog")) File.Delete(_startupFolder + "\\proclog");
				StreamWriter sw = new StreamWriter(new FileStream(_startupFolder + "\\proclog", FileMode.Create));
				sw.WriteLine(proc.Id);
				sw.WriteLine(proc.MainWindowTitle);
				sw.Close();
				Trace.WriteLine("Window Handle: " + proc.MainWindowHandle);
				Trace.WriteLine("Window Title: " + proc.MainWindowTitle);

			}
			
			// Bind the monitor to this process and start it
			ProcMonitor.BindToProcess(ref proc);
			ProcMonitor.StartMonitor();

			// Start the delagginator
			//delag = new Delagginator(proc.Id);
			//delag.Delaggit();
		}
		
		public bool BindServerExtensions()
		{
			try
			{
				Trace.WriteLine("Attempting to create Console Window.");
				_consoleWindow = new ConsoleWindow(proc.MainWindowHandle);
				
				// Hide the console window
				//WindowTools.Window.ShowWindowAsync(proc.MainWindowHandle,WindowTools.Window.SW_HIDE);
				Trace.WriteLine("Console window initialized.");

				// Bind
				Trace.WriteLine("Attempting to bind server extensions.");
				_serverExtensions.Bind(ref _consoleWindow);
				Trace.WriteLine("Server extensions successfully bound.");
				Trace.WriteLine("Starting services");
				_serverExtensions.StartServices();
				Trace.WriteLine("Services started.");
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override void Stop()
		{
			try
			{
				//_serverExtensions.StopServices();
				IntentionalStop = true;
				if (!proc.HasExited) proc.Kill();
				m_active = false;
				ProcMonitor.StopMonitor();
			}
			catch
			{
				Trace.WriteLine("CRITICAL ERROR! *** Exception while stopping server process!");
			}
		}
	}
}
