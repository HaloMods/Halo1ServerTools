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

namespace Hive.Node.Core.Query
{
  /// <summary>
  /// Provides an Interface for querying different types of servers.
  /// </summary>
  public interface IServerQuery
  {
    string Host { get; }
    int Port { get; }
    
    void Query();

    event QueryFailedEventHandler QueryFailed;
  }

  public delegate void QueryFailedEventHandler(IServerQuery sender, QueryFailedEventArgs e);
  
  public class QueryFailedEventArgs : EventArgs
  {
    private Exception exception;

    /// <summary>
    /// The exception that was generated when the query failed.
    /// </summary>
    public Exception Exception
    {
      get { return exception; }
    }

    public QueryFailedEventArgs(Exception exception)
    {
      this.exception = exception;
    }
  }
}