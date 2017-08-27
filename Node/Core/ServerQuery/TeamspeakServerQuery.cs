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
using Hive.Node.Core.ServerInterfaces;

namespace Hive.Node.Core.Query
{
  /// <summary>
  /// Provides the capability to query a Teamspeak 2 server and obtain the results.
  /// </summary>
  public class TeamspeakServerQuery : IServerQuery
  {
    private string host;
    private int port;
    private TeamspeakServerInfo serverInfo;

    /// <summary>
    /// The host address of the server being queried.
    /// </summary>
    public string Host
    {
      get { return host; }
    }

    /// <summary>
    /// The port of the server being queried.
    /// </summary>
    public int Port
    {
      get { return port; }
    }

    /// <summary>
    /// Contains information about the server after a successful query.
    /// </summary>
    public TeamspeakServerInfo ServerInfo
    {
      get { return serverInfo; }
    }

    /// <summary>
    /// Creates a new instance of the object based on the given parameters.
    /// </summary>
    /// <param name="host">The host address of the server being queried.</param>
    /// <param name="port">The port of the server being queried.</param>
    public TeamspeakServerQuery(string host, int port)
    {
      this.host = host;
      this.port = port;
    }

    /// <summary>
    /// Queries the Teamspeak server and stores the results.
    /// </summary>
    public void Query()
    {
      TeamspeakServerInterface i = new TeamspeakServerInterface(this.host);
      try
      {
        i.Query(this.port);
        this.serverInfo = i.ServerInfo;
      }
      catch (Exception ex)
      {
        if (QueryFailed != null) QueryFailed(this, new QueryFailedEventArgs(ex));
      }
    }

    /// <summary>
    /// Occurs when an attempt to query the Teamspeak server fails.
    /// </summary>
    public event QueryFailedEventHandler QueryFailed;
  }
}