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

namespace Hive.Node.Core.Servers
{
	/// <summary>
	/// Represents a basic server, in which start/stop functionality involves
	/// starting and terminating the host process.
	/// </summary>
	public class GenericServer : Server
	{
    /// <summary>
    /// Creates a new Server object based on the supplied configuraton data.
    /// </summary>
    /// <param name="serverConfig">A valid server configuration.</param>
    public GenericServer(ProcessConfigurationData processConfig, ServerConfigurationData serverConfig)
      : base(processConfig, serverConfig) { ; }

    /// <summary>
	  /// Starts the server.
	  /// </summary>
	  public override void Start()
    {
      if (!this.process.IsRunning) this.process.StartProcess();
    }

	  /// <summary>
	  /// Stops the server.
	  /// </summary>
	  public override void Stop()
	  {
      if (this.process.IsRunning) this.process.StopProcess();
	  }
	}
}
