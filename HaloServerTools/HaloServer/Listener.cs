using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for Listener.
	/// </summary>
	public class Listener
	{
		public IPAddress serverIP;
		public IPEndPoint serverReceive;
		public UdpClient server;
		private bool m_readyToListen;
		private Thread m_listenerThread;
		
		/// <summary>
		/// Listens for UDP packets on the specified ip address and port.
		/// </summary>
		/// <param name="ip">The ip address to listen on.</param>
		/// <param name="port">The port to listen on.</param>
		public Listener(string ip, int port)
		{
			m_readyToListen = false;
			try
			{
				// Set up the bindings
				serverIP = IPAddress.Parse(ip);
				serverReceive = new IPEndPoint(serverIP, 2302);
				server = new UdpClient(2302);
				m_readyToListen = true; // Upon successful binding, report as ready
			}
			catch
			{
				System.Diagnostics.Trace.WriteLine("ERROR: Could not bind UDP listener port!");
			}
		}
		private void StartListening()
		{
			if (!m_readyToListen)
			{
				System.Diagnostics.Trace.WriteLine("Socket was not bound - cannot start listener.");
				return;
			}

			// Set up the listener thread
			m_listenerThread = new Thread(new ThreadStart(TListen));
		}
		private void StopListening()
		{
			m_listenerThread.Suspend();
		}

		private void TListen()
		{
			byte[] cPacket;
			while (true)
			{
				cPacket = server.Receive(ref serverReceive);
				string s = System.Text.Encoding.ASCII.GetString(cPacket);
				// This string will need to be forwarded to the managed command parser	
				Thread.Sleep(1000);
			}
		}
	}
}
