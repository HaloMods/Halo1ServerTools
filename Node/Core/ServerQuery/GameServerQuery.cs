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
using GameStat;

namespace Hive.Node.Core.Query
{
  /// <summary>
  /// Provides the capability to query a game server and obtain the results.  Makes use of the GameStat
  /// game server query library - http://www.int64.org/gamestat.html
  /// </summary>
  public class GameServerQuery : IServerQuery
  {
    private string host;
    private int port;
    private GameStat.GameType serverType;
    private ServerInfo serverInfo;

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
    /// The type of game server to be queried.
    /// </summary>
    public GameType ServerType
    {
      get { return serverType; }
    }

    /// <summary>
    /// Contains information about the server after a successful query.
    /// </summary>
    public ServerInfo ServerInfo
    {
      get { return serverInfo; }
    }

    /// <summary>
    /// Creates a new instance of the object based on the given parameters.
    /// </summary>
    /// <param name="host">The host address of the server being queried.</param>
    /// <param name="port">The port of the server being queried.</param>
    /// <param name="serverType">The type of game server to be queried.</param>
    public GameServerQuery(string host, int port, GameType serverType)
    {
      this.host = host;
      this.port = port;
      this.serverType = serverType;
    }

    /// <summary>
    /// Queries the game server and stores the results.
    /// </summary>
    public void Query()
    {
      try
      {
        serverInfo = ServerInfo.Query(serverType, host, (ushort)port);
      }
      catch (Exception ex)
      {
        if (QueryFailed != null) QueryFailed(this, new QueryFailedEventArgs(ex));
      }
    }

    /// <summary>
    /// Occurs when an attempt to query the game server fails.
    /// </summary>
    public event QueryFailedEventHandler QueryFailed;
  }
}