using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Core.Monitoring
{
	/// <summary>
	/// Scans a Halo dedicated server logfile for additional entries.
	/// </summary>
	public class LogFileMonitor
	{
		#region Filename
		private string _filename;
		public string Filename
		{
			get { return _filename; }
		}
		#endregion

		#region File Size
		private long currentFileSize = 0;
		/// <summary>
		/// Resets the file size to a manually specified value.
		/// </summary>
		/// <param name="size">A manually specified size that will override the tracked size.</param>
		public void OverrideSize(long size)
		{
			currentFileSize = size;
		}
		#endregion

		/// <summary>
		/// Value in milliseconds to wait between logfile checks
		/// </summary>
		private int readDelay;
		private Thread monitorThread;
		private bool _enabled = false;
		public event LogFileEventHandler LogUpdated;

		public LogFileMonitor(string filename)
		{
			if (!File.Exists(filename))  // Make sure the file exists
			{
				Trace.WriteLine("Logfile not found: " + filename);				
				return;
			}

			_filename = filename;
			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			currentFileSize = fs.Length;
			fs.Close();
			
			// Until we are reading this in with the config, set it manually
			readDelay = 1000;
			monitorThread = new Thread(new ThreadStart(MonitorLog));
			Trace.WriteLine("Logfile Monitor Object created and ready to start");		
		}

		/// <summary>
		/// 
		/// </summary>
		public void MonitorLog()
		{
			while (_enabled)
			{
				CheckLog();
				Thread.Sleep(readDelay);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			if (monitorThread != null)
			{
				if (monitorThread.ThreadState == System.Threading.ThreadState.Running)
					monitorThread.Suspend();
			}
			_enabled = true;
			monitorThread.Start();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			_enabled = false;
		}

		/// <summary>
		/// Look at the log file and see if any new lines have been written to it.
		/// </summary>
		public void CheckLog()
		{
			StreamReader sr = new StreamReader(
				new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
				System.Text.Encoding.Unicode);

			if (sr.BaseStream.Length > currentFileSize)
			{
				// A new line has been written
				sr.BaseStream.Seek(currentFileSize, SeekOrigin.Begin);
				string data = sr.ReadToEnd();
				//int dataSize = data.Length;  // Keep a record of the original size

				Console.Write(data);

				string[] s = Globals.Split(
					data, 
					"\r\n");

				// The last string should be empty - if not, we've read in the middle
				// of a buffered write and should ignore the last element
				// Note: This is extremely unlikely to occur.
				if (s[s.Length-1] != "") s[s.Length-1] = "";
	
				for (int x = 0; x < s.Length-1; x++)
					if (s[x] != "") currentFileSize += (s[x].Length + 2) * 2;

				// Fire the event
				if (LogUpdated != null) LogUpdated(this, new LogEventArgs(s));
			}

			if (sr.BaseStream.Length < currentFileSize)
			{
				// We may have encountered a logfile split.  If all of the entries from the previous
				// log weren't read, this could cause a problem.  We'll deal with that later - just keep it in mind.
				if (sr.BaseStream.Length < (currentFileSize / 2))
					currentFileSize = 0;
			}
			sr.Close();
		}

		public string[] GetLines(int startingLine)
		{
			StreamReader sr = new StreamReader(
				new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

			long currentLocation = sr.BaseStream.Position;

			sr.BaseStream.Seek(0, SeekOrigin.Begin);
			
			string[] s = Globals.Split(
								Globals.UnicodeToASCII(sr.ReadToEnd()), 
								"\r\n");
			sr.BaseStream.Seek(currentLocation, SeekOrigin.Begin);
			
			if (startingLine > s.Length) return null; // Make sure we have enough lines
			
			string[] s2 = new string[s.Length - startingLine]; 
			s.CopyTo(s2, startingLine);	
			sr.Close();
			return s2;
		}
	};

	/// <summary>
	/// 
	/// </summary>
	public class LogEventArgs : System.EventArgs 
	{
		#region NewLines
		private string[] newLines;
		public string[] NewLines
		{
			get { return newLines; }
		}
		#endregion

		public LogEventArgs(string[] lines)
		{
			newLines = new string[lines.Length];
			for(int x = 0; x < lines.Length-1; x++) // 'x < lines.Length' mb?
				newLines[x] = lines[x];
		}
	};

	/// <summary>
	/// 
	/// </summary>
	public delegate void LogFileEventHandler(object sender, LogEventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public class LogFileEventStream
	{
	};
}