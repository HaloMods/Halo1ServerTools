/* ---------------------------------------------------------------
 * The Hive
 * Server Hosting Management System
 * (c)2005, EPG Servers, LLC.
 * ---------------------------------------------------------------
 * Author      : Justin Draper
 * Co-Authors  : None
 * ---------------------------------------------------------------
 * This code is protected under the copyright laws of the
 * United States of America, and is confidential.  Use or
 * posession of this code without express written consent of
 * EPG Servers, LLC. is expressly forbidden.
 * ---------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Diagnostics;

namespace Hive.Node.Core
{
	/// <summary>
  /// Analyzes the stability of a process, and provides fault tolerance/automatic restarts
  /// in the event of a crash.
	/// </summary>
	public class ServerProcessWatchdog
	{
    private ServerProcess process;
    private CrashReportCollection reports;

    /// <summary>
    /// Determines if the process is stable based on it's crash history.
    /// </summary>
    public bool Stable
    {
      get 
      {
        int recentCrashCount = 0;
        foreach (CrashReport report in reports)
        {
          // Determine if the crash occurred within the past half hour.
          if (DateTime.Now.Subtract(report.CrashTime).TotalMinutes < 30)
          {
            recentCrashCount++;
          }
        }
        
        // If there have been 5 or more crashes within the threshold time, the process is unstable.
        if (recentCrashCount >= 5)
        {
          return false;
        }
        else
        {
          return true;          
        }
      }
    }

    /// <summary>
    /// Occurs when the process is declared unstable.
    /// </summary>
    public event EventHandler ProcessUnstable;

		/// <summary>
		/// Creates a new ServerProcessWatchdog object for the specified ServerProcess object.
		/// </summary>
		/// <param name="process">The ServerProcess object to be associated with this ServerProcessWatchdog object.</param>
    public ServerProcessWatchdog(ServerProcess process)
		{
      this.process = process;
      reports = new CrashReportCollection(10);
      process.ProcessCrash += new EventHandler(Process_ProcessCrash);
    }

    private void Process_ProcessCrash(object sender, EventArgs e)
    {
      CrashReport report = new CrashReport(this.process);
      reports.Add(report);
      
      // Determine the stability of the process and if we should restart it.
      if (Stable)
      {
        Trace.WriteLine("ServerProcessWatchdog has determined that the process is stable, and is attempting to restart it.");
        process.StartProcess();
      }
      else
      {
        Trace.WriteLine("ServerProcessWatchdog has declared the process to be unstable, and will no longer monitor or attempt to start it until manual intervention takes place.");
        if (ProcessUnstable != null) ProcessUnstable(this, new EventArgs());
      }
    }
  }

  /// <summary>
  /// Records data related to a process crash.
  /// </summary>
  public class CrashReport
  {
    private DateTime crashTime;
    private DateTime startTime;
    
    /// <summary>
    /// Returns a DateTime object representing when the process crashed.
    /// </summary>
    public DateTime CrashTime
    {
      get { return this.crashTime; }
    }

    /// <summary>
    /// Returns a DateTime object representing when the process was originally started.
    /// </summary>
    public DateTime StartTime
    {
      get { return this.startTime; }
    }

    /// <summary>
    /// Returns a TimeSpan object representing the total length of time that the process was running.
    /// </summary>
    public TimeSpan RunTime
    {
      get { return crashTime.Subtract(startTime); }
    }

    public CrashReport(ServerProcess process)
    {
      crashTime = DateTime.Now;
      startTime = process.StartTime;
    }
  }

  /// <summary>
  /// Implements a type-safe fixed-size collection of CrashReport objects.  When the history
  /// length is exceeded, records are removed on a first in, first out basis to make room.
  /// </summary>
  public class CrashReportCollection : CollectionBase
  {
    private int historyLength;

    /// <summary>
    /// The number of records that can be stored in this collection.
    /// </summary>
    public int HistoryLength
    {
      get { return this.historyLength; }
    }

    /// <summary>
    /// Adds a CrashReport object to the collection.
    /// </summary>
    /// <param name="report">The CrashReport object to add.</param>
    public void Add(CrashReport report)
    {
      int size = InnerList.Count;
      if (size >= historyLength) InnerList.RemoveAt(0);
      InnerList.Add(report);
    }

    /// <summary>
    /// Returns the CrashReport object at the specified index.
    /// </summary>
    public CrashReport this[int index]
    {
      get { return (InnerList[index] as CrashReport); }
    }
    
    /// <summary>
    /// Creates a CrashReportCollection with the specified history length.
    /// </summary>
    /// <param name="historyLength">The number of records to store in the history.</param>
    public CrashReportCollection(int historyLength) : this()
    {
      this.historyLength = historyLength;
    }

    /// <summary>
    /// Creates a CrashReportCollection with a default history length of 5 reports.
    /// </summary>
    public CrashReportCollection()
    {
      this.historyLength = 5;
    }
  }

  public class ServerProcessWatchdogCollection : CollectionBase
  {
    public void Add(ServerProcessWatchdog watchdog)
    {
      InnerList.Add(watchdog);
    }
    public void Remove(ServerProcessWatchdog watchdog)
    {
      InnerList.Remove(watchdog);
    }
  }
}