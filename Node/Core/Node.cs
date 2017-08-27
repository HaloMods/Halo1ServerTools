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
using System.Xml;
using Hive.Node.Core;
using Hive.Node.Core.Servers;

namespace Hive.Node
{
	/// <summary>
	/// Represents a Node in the Hive system.  This class provides an interface to all of
	/// the functionality that a Node provides.
	/// </summary>
	public class Node : IDisposable
	{
    private bool isDisposed = false;

    public Node()
		{
      // Load the node configuration.
      XmlDocument config = new XmlDocument();
      config.Load(Environment.CurrentDirectory + "\\servers.xml");

      XmlNodeList nodes = config.SelectNodes("//server");

      foreach (XmlNode serverConfigNode in nodes)
      {
        string processID = serverConfigNode.SelectSingleNode("./process_id").InnerText;

        XmlNode processConfigNode = config.SelectSingleNode(
          String.Format("//process[@process_id='{0}']", processID));

        ProcessConfigurationData processConfig = new ProcessConfigurationData(processConfigNode, serverConfigNode);
        ServerConfigurationData serverConfig = new ServerConfigurationData(serverConfigNode);

        //TODO: Create custom exception classes for different exception reasons, and catch them seperately.
        try
        {
          ServerManager.RegisterServer(processConfig, serverConfig);        
        }
        catch (Exception ex)
        {
          Trace.WriteLine(ex.Message);
        }
      }
		}

	  public void Dispose()
	  {
      if (!isDisposed)
      {
        Trace.WriteLine("Teardown processes started.", "shutdown");
        ServerManager.ShutDown();
        Trace.WriteLine("Teardown processes completed.", "shutdown");
        this.isDisposed = true;
      }
	  }
	}
}