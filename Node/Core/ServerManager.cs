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
using System.Diagnostics;
using Hive.Node.Core.Servers;

namespace Hive.Node.Core
{
  /// <summary>
  /// Provides a centralized interface to all servers running on the node.
  /// </summary>
  public class ServerManager
  {
    private static ServerCollection servers = new ServerCollection();

    //TODO: Create a "ConfigurationManager" so that process and server info can be queried and provided seperately.
    //This will remove the need to pass references to Process and Server configs to each object.

    /// <summary>
    /// Creates a server based on the supplied configuration data, and adds
    /// it to the Server Manager.
    /// </summary>
    public static void RegisterServer(ProcessConfigurationData processConfig, ServerConfigurationData serverConfig)
    {
      //TODO: Decide how best to determine which type of server to create.
      //For now, we will use the if/then approach.
      if (serverConfig.ServerType == ServerType.Game)
      {
        if (serverConfig.SubType == "HaloCombatEvolved")
        {
          try
          {
            Server s = new GenericServer(processConfig, serverConfig);    
            servers.Add(s);
          }  
          catch (Exception ex)
          {
            throw new Exception("Unable to create server object: " + ex.Message);
          }
        }
        else
        {
          throw new Exception("Unable to create server object: " + serverConfig.SubType + " is not a supported game server type.");
        }
      }
    }

    /// <summary>
    /// Removes it from the server manager.
    /// </summary>
    /// <param name="identifier">The Server's unique identifier.</param>
    public static void RemoveServer(int identifier)
    {
      Server s = GetServer(identifier);
      
      // Check to see if any other servers are using this server's process.
      // If not, dispose of it.
      bool inUse = false;
      foreach (Server x in servers)
      {
        if (x.Process == s.Process) inUse = true;
      }
      
      if (!inUse)
      {
        ServerProcessManager.RemoveProcess(s.Process.Identifier);
      }

      servers.Remove(s);
    }

    /// <summary>
    /// Stops all servers currently running in the server manager.
    /// </summary>
    public static void ShutDown()
    {
      while (servers.Count > 0)
      {
        Server s = servers[0];
        s.Stop();
        servers.Remove(s);
        Trace.WriteLine("Server " + s.Identifier.ToString() + " has been stopped.", "shutdown");
      }
    }

    /// <summary>
    /// Returns the Server matching the supplied identifier.
    /// </summary>
    public static Server GetServer(int identifier)
    {
      try
      {
        return servers.LocateByIdentifier(identifier);
      }
      catch (Exception ex)
      {
        throw new Exception("Server [" + identifier.ToString() + "] does not exist in the server manager.", ex);
      }
    }
  }
}
