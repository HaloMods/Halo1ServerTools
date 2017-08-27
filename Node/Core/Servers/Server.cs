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
using System.Xml;

namespace Hive.Node.Core.Servers
{
  /// <summary>
  /// Encapsulates a ServerProcess and all of it's related objects.
  /// </summary>
  public abstract class Server
  {
    protected ServerProcess process;
    protected ServerConfigurationData configuration;
    
    /// <summary>
    /// Returns the ServerProcess object associated with this Server.
    /// </summary>
    public ServerProcess Process
    {
      get { return this.process; }
    }

    /// <summary>
    /// Returns the server's unique identifier.
    /// </summary>
    public int Identifier
    {
      get { return configuration.Identifier; }
    }

    /// <summary>
    /// Returns a value indicating if the Server's process is currently considered stable.
    /// </summary>
    public bool Stable
    {
      get { return process.Stable; }
    }

    /// <summary>
    /// Creates a new Server object based on the supplied configuraton data.
    /// </summary>
    /// <param name="serverConfig">A valid server configuration.</param>
    public Server(ProcessConfigurationData processConfig, ServerConfigurationData serverConfig)
    {
      try
      {
        this.configuration = serverConfig;
        if (!ServerProcessManager.Contains(serverConfig.ProcessID))
        {
          // The server process does not yet exist, so we need to create it.
          ServerProcessManager.CreateProcess(processConfig);
        }
        this.process = ServerProcessManager.GetProcess(serverConfig.ProcessID);
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to create server: " + ex.Message, ex);
      }
    }

    /// <summary>
    /// Starts the server.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Stops the server.
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Restarts the server.
    /// </summary>
    public virtual void Restart()
    {
      Stop();
      Start();
    }
  }

  public class ServerConfigurationData
  {
    private int processID;
    private string host;
    private int port;
    private ServerType serverType;
    private int identifier;
    private string subType;

    public int ProcessID
    {
      get { return processID; }
    }

    public string Host
    {
      get { return host; }
    }

    public int Port
    {
      get { return port; }
    }

    public ServerType ServerType
    {
      get { return serverType; }
    }

    public int Identifier
    {
      get { return identifier; }
    }

    public string SubType
    {
      get { return subType; }
    }

    public ServerConfigurationData(XmlNode configNode)
    {
      this.identifier = Convert.ToInt32(configNode.Attributes["server_id"].InnerText);
      this.processID = Convert.ToInt32(configNode.SelectSingleNode("process_id").InnerText);
      this.serverType = (ServerType)Enum.Parse(typeof(ServerType), configNode.Attributes["type"].InnerText, true);
      this.host = configNode.SelectSingleNode("host").InnerText;
      this.port = Convert.ToInt32(configNode.SelectSingleNode("port").InnerText);
      this.subType = configNode.SelectSingleNode("subtype").InnerText;    
    }
  }

  public enum ServerType
  {
    Game,
    Voice
  }

  /// <summary>
  /// Implements a type-safe collection of Server objects.
  /// </summary>
  public class ServerCollection : CollectionBase
  {
    public void Add(Server server)
    {
      InnerList.Add(server);
    }
    public void Remove(Server server)
    {
      InnerList.Remove(server);
    }
    public Server LocateByIdentifier(int identifier)
    {
      foreach (Server s in InnerList)
      {
        if (s.Identifier == identifier)
          return s;
      }
      throw new Exception("Item does not exist in collection."); 
    }
    public Server this[int index]
    {
      get { return (Server)InnerList[index]; }
    }
  }
}
