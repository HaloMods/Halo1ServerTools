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

using Hive.Node.Core.ServerInterfaces;

namespace Hive.Node.Core.Servers
{
	/// <summary>
	/// Represents a Teamspeak voice server, whihc hosts multiple virtual servers inside a single process.
	/// </summary>
	public class TeamspeakServer : GenericServer
	{
    /// <summary>
    /// Creates a new Server object based on the supplied configuraton data.
    /// </summary>
    /// <param name="serverConfig">A valid server configuration.</param>
    public TeamspeakServer(ProcessConfigurationData processConfig, ServerConfigurationData serverConfig)
      : base(processConfig, serverConfig) { ; }

    public override void Start()
    {
      base.Start();
      TeamspeakServerInterface i = new TeamspeakServerInterface(this.configuration.Host);
      i.SuperAdminLogin("Justin", "msi11496");
      i.StartServer(this.configuration.Port);
    }

    public override void Stop()
    {
      TeamspeakServerInterface i = new TeamspeakServerInterface(this.configuration.Host);
      i.SuperAdminLogin("Justin", "msi11496");
      i.StopServer(this.configuration.Port);
      base.Stop();
    }
	}
}
