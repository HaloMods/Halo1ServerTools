using System;
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

namespace Hive.Node.Core
{
	/// <summary>
	/// Manages starting, stopping, and tracking all servder processed running on the node.
	/// </summary>
	public class ServerProcessManager
	{
		private static ServerProcessCollection processes = new ServerProcessCollection();

    /// <summary>
    /// Creates a server process based on the supplied configuration data, and adds
    /// it to the Process Manager.
    /// </summary>
    /// <param name="processConfig">A valid ProcessConfigurationData object.</param>
    /// <returns>The process's unique identifier.</returns>
    public static int CreateProcess(ProcessConfigurationData processConfig)
    {
      ServerProcess proc = new ServerProcess(processConfig);
      proc.StartProcess();
      processes.Add(proc);

      return proc.Identifier;
    }

    /// <summary>
    /// Stops the specified process, and removes it from the process manager.
    /// </summary>
    /// <param name="identifier">The process's unique identifier.</param>
    public static void RemoveProcess(int identifier)
    {
      ServerProcess proc = GetProcess(identifier);

      lock(proc)
      {
        if (proc.IsRunning)
        {
          proc.StopProcess();
        }
        processes.Remove(proc); 
      }
    }

    /// <summary>
    /// Returns the process matching the supplied identifier.
    /// </summary>
    public static ServerProcess GetProcess(int identifier)
    {
      try
      {
        return processes[identifier];
      }
      catch (Exception ex)
      {
        throw new Exception("Process [" + identifier.ToString() + "] does not exist in the process manager.", ex);
      }
    }

    /// <summary>
    /// Returns a boolean indicating if the ServerProcessManager conttains the specified process.
    /// </summary>
    public static bool Contains(int identifier)
    {
      return processes.Contains(identifier);
    }
	}
}
