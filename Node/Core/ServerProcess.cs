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
using System.Threading;
using System.Xml;

namespace Hive.Node.Core
{
  /// <summary>
  /// Encapsulates a physical process running on the Node.  In addition to handling configuration data,
  /// this class also provides methods for controlling and monitoring the process.
  /// </summary>
  public class ServerProcess : IDisposable
  {
    private ProcessConfigurationData configuration;
    private Process process;
    private DateTime startTime;
    private Thread monitorThread;
    private ServerProcessWatchdog watchdog;

    private bool isDisposed = false;
    
    /// <summary>
    /// Returns a DateTime representing when the process was started.
    /// </summary>
    public DateTime StartTime
    {
      get { return this.startTime; }
    }

    /// <summary>
    /// Returns the process's unique identifier.
    /// </summary>
    public int Identifier
    {
      get { return configuration.Identifier; }
    }

    /// <summary>
    /// Returns a boolean indicating if the process is currently running.
    /// </summary>
    public bool IsRunning
    {
      get
      {
        if (process != null) return !process.HasExited;
        return false;
      }
    }

    /// <summary>
    /// Returns a value indicating if the process is currently considered stable.
    /// </summary>
    public bool Stable
    {
      get { return watchdog.Stable; }
    }

    /// <summary>
    /// Occurs when the process terminates unexpectedly.
    /// </summary>
    public event EventHandler ProcessCrash;

    #region Constructor
    public ServerProcess(ProcessConfigurationData configuration)
    { 
      this.configuration = configuration;
      this.watchdog = new ServerProcessWatchdog(this);
    }
    #endregion

    /// <summary>
    /// Starts the process. 
    /// </summary>
    public void StartProcess()
    {
      try
      {
        this.process = new System.Diagnostics.Process();
        this.process.StartInfo.FileName = configuration.FullPath;
        this.process.StartInfo.WorkingDirectory = configuration.Folder;
        this.process.Start();

        BeginMonitoring();
        Trace.WriteLine(String.Format("Successfully started and began monitoring process - HiveID:{0}, SystemID:{1} ({2})",
          configuration.Identifier, this.process.Id, configuration.FullPath), "info");
      }
      catch (Exception ex)
      {
        this.process = null;
        throw new Exception("Unable to start process '" + configuration.FullPath + "' : " + ex.Message);
      }
    }

    /// <summary>
    /// Stops the process.
    /// </summary>
    public void StopProcess()
    {
      // Stop monitoring the process.
      if (monitorThread.IsAlive)
      {
        monitorThread.Abort();
        monitorThread.Join();
      }
      process.Kill();
    }

    /// <summary>
    /// Stops the Process, then immediately Starts it again.
    /// </summary>
    public void RestartProcess()
    {
      StopProcess();
      StartProcess();
    }

    /// <summary>
    /// Start monitoring the process.
    /// </summary>
    private void BeginMonitoring()
    {
      this.startTime = DateTime.Now;
      this.monitorThread = new Thread(new ThreadStart(MonitorThreadBegin));
      this.monitorThread.Start();
    }

    /// <summary>
    /// This function continuously checks the process, and throws a ProcessCrash event when
    /// the process is no longer running.
    /// </summary>
    private void MonitorThreadBegin()
    {
      try
      {
        Trace.WriteLine("Process monitoring thread started.");
        while (true)
        {
          if (this.process.HasExited) break;
          Thread.Sleep(500);
        } 
        if (ProcessCrash != null) ProcessCrash(this, new EventArgs());
      }
      catch (ThreadAbortException)
      {
        Trace.WriteLine("Process monitoring thread terminated.");
      }
    }

    public void Dispose()
    {
      if (!isDisposed)
      {
        if (this.IsRunning) this.StopProcess();
        isDisposed = true;
      }
    }
  }

  /// <summary>
  /// Contains the information needed to start a process.
  /// </summary>
  public class ProcessConfigurationData
  {
    private string folder;
    private string filename;
    private string parameters;
    private int identifier;

    /// <summary>
    /// The folder that the process starts in.
    /// </summary>
    public string Folder
    {
      get { return this.folder; }
    }

    /// <summary>
    /// The process filename.
    /// </summary>
    public string Filename
    {
      get { return filename; }
    }

    /// <summary>
    /// The commandline parameters that the process starts with.
    /// </summary>
    public string Parameters
    {
      get { return parameters; }
    }

    /// <summary>
    /// The unique identifier for the process.
    /// </summary>
    public int Identifier
    {
      get { return identifier; }
    }

    /// <summary>
    /// The fully qualified path to the process.
    /// </summary>
    public string FullPath
    {
      get { return Folder + "\\" + filename; }
    }

    /// <summary>
    /// Creates the object based on configuration nodes from an XML configuration file.
    /// </summary>
    /// <param name="processConfigNode">A valid process configuration node.</param>
    /// <param name="serverConfigNode">A valid server configuration node.  This is
    /// required in the event that the process needs commandline variables that are
    /// contained within the server's configuration.</param>
    public ProcessConfigurationData(XmlNode processConfigNode, XmlNode serverConfigNode)
    {
      try
      {
        this.folder = processConfigNode.SelectSingleNode("folder").InnerText;
        this.filename = processConfigNode.SelectSingleNode("filename").InnerText;
        this.identifier = Convert.ToInt32(processConfigNode.Attributes["process_id"].InnerText);
        
        // Replace any parameter variables with the correct data from the Server config.
        string[] args = processConfigNode.SelectSingleNode("parameters").InnerText.Split('#');
        for (int x=1; x<args.Length; x+=2)
        {
          string name = args[x];
          args[x] = serverConfigNode.SelectSingleNode(name).InnerText;
        }

        // Construct the string.
        this.parameters = String.Empty; 
        for (int x=0; x<args.Length; x++)
        {
          this.parameters += args[x];
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to configure process.", ex);
      }
    }
  }

  /// <summary>
  /// Implements a type-safe collection of Server objects.
  /// </summary>
  public class ServerProcessCollection : CollectionBase
  {
    public void Add(ServerProcess process)
    {
      InnerList.Add(process);
    }
    public void Remove(ServerProcess process)
    {
      InnerList.Remove(process);
    }
    public ServerProcess this[int identifier]
    {
      get
      {
        foreach (ServerProcess p in InnerList)
        {
          if (p.Identifier == identifier)
            return p;
        }
        throw new Exception("Item does not exist in collection.");
      }
    }
    public bool Contains(int identifier)
    {
      foreach (ServerProcess p in InnerList)
      {
        if (p.Identifier == identifier)
          return true;
      }
      return false;
    }
  }
}