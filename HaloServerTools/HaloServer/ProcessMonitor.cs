/* ---------------------------------------------------------------
 * Halo Server Tools, MultiGame Server Hosting Management System
 * (c)2004, EPG Servers, LLC.
 * ---------------------------------------------------------------
 * Class       : HaloServerTools.ProcessMonitor
 * Description : Implements a child process monitoring system
 *               with crash event notification and stability
 *               assessment.
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
using System.Threading;

namespace HaloServerTools
{
	/// <summary>
	/// Implements a child process monitoring system with crash event
	/// notification and stability assessment.
	/// </summary>
	public class ProcessMonitor
	{
		private Process m_process;
		private DateTime m_startTime;
		private Thread m_thread;

		public int MaxCPU=0;
		public int AverageCPU=0;
		public int MaxMemory=0;
		public int AverageMemory=0;
		public int NumberOfScans=0;

		public int NumberOfCrashes;

		// Set up our process crash event
		public class ProcessEventArgs : System.EventArgs 
		{
			public bool Stable;
			public ProcessEventArgs(bool stable)
			{
				Stable = stable;
			}
		}
		public event ProcessEventHandler ProcessCrash;
		public delegate void ProcessEventHandler(object sender, ProcessEventArgs e);
		protected virtual void OnProcessCrash(ProcessEventArgs e) 
		{
			if (ProcessCrash != null) ProcessCrash(this, e);
		}
		public bool IsStable
		{
			get 
			{
				TimeSpan runLength = DateTime.Now.Subtract(m_startTime);
				int totalSecondsRunning = runLength.Seconds;
				
				// Process must have crashed more than 5 times total to be declared unstable.
				if (NumberOfCrashes < 5) return true;
				
				// If the the process has crashed more than once per 5 minutes, it's unstable.
				if ((totalSecondsRunning / NumberOfCrashes) <= 300) return false;

				// If the above conditions were not triggered, the process is stable.
				return true;
			}
		}
		public ProcessMonitor()
		{
		}
		public void BindToProcess(ref Process p)
		{
			m_process = p;
			if ((object)m_startTime == null) m_startTime = DateTime.Now;
		}
		// Begins monitoring a process on a new thread.
		public void StartMonitor()
		{
			m_thread = new Thread(new ThreadStart(MonitorThread));
			m_thread.Start();
		}
		public void StopMonitor()
		{
			m_thread.Suspend();
			m_thread.Abort();
		}
		public void MonitorThread()
		{
			Trace.WriteLine("[ Process monitoring thread started ]");
			while (true)
			{
				Thread.Sleep(5000);
				if (m_process.HasExited)
				{
					break;
				}
				// Get process stats
				NumberOfScans++;
				int currentMem = m_process.PagedMemorySize;
				//*** Finish implementing this at some point.
			}
			ExitHandler();
		}
		public void ExitHandler()
		{
			ProcessCrash(this, new ProcessEventArgs(IsStable));			
		}
	}
}