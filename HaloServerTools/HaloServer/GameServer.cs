/* ---------------------------------------------------------------
 * Halo Server Tools, MultiGame Server Hosting Management System
 * (c)2004, EPG Servers, LLC.
 * ---------------------------------------------------------------
 * Class       : HaloServerTools.GameServer
 * Description : Abstract class encapsulating the core components
 *               of an HST managed server process.
 * Author      : Justin Draper
 * Co-Authors  : None
 * ---------------------------------------------------------------
 * This code is protected under the copyright laws of the
 * United States of America, and is confidential.  Use or
 * posession of this code without express written consent of
 * the copyright holder is expressly forbidden and will be
 * prosecuted to the fullest extent allowed by law.
 * ---------------------------------------------------------------
 */

using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Xml;

namespace HaloServerTools
{
	/// <summary>
	/// Abstract class encapsulating the core components
	/// of an HST managed server process.
	/// </summary>
	public abstract class GameServer : MarshalByRefObject
	{
		public System.Net.IPAddress _ipAddress;
		public int _port;
		public string _startupFolder;
		public string _commandLine;
		public string _parameters;
		public string name;
		public string id;
		public System.Diagnostics.Process proc;
		public bool IntentionalStop = false;

		public ProcessMonitor ProcMonitor;

		public abstract bool Active();

		public abstract void ProcessCrashHandler(object o, ProcessMonitor.ProcessEventArgs e);
				
		// Start the server child process.
		public abstract void Start();

		public abstract void Stop();

		public void Restart()
		{
			Stop();
			Start();
		}
	}
	// Implements a type-safe searchable collection of HaloServerName objects
	public class GameServerCollection : System.Collections.DictionaryBase
	{
		public GameServer this[string gameServerName]
		{
			get { return ((GameServer)(Dictionary[gameServerName])); }
			set { Dictionary[gameServerName] = value; }
		}
		public void Add(string gameServerName, GameServer gameServer)
		{
			Dictionary.Add(gameServerName, gameServer);
		}
		public void Remove(string gameServerName)
		{
			Dictionary.Remove(gameServerName);
		}
		public bool Contains(string gameServerName)
		{
			return Dictionary.Contains(gameServerName);
		}
	}
}
