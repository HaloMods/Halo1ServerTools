/*
	Copyright (c) 2004 Cory Nelson

	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the
	"Software"), to deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to
	permit persons to whom the Software is furnished to do so, subject to
	the following conditions:

	The above copyright notice and this permission notice shall be included
	in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace GameStat {
	/// <summary>
	/// Holds server status information.
	/// </summary>
	public class ServerInfo {
		/// <summary>
		/// The server's name.
		/// </summary>
		public string Host=string.Empty;

		/// <summary>
		/// The server's current mod, or game type if mods aren't supported.
		/// </summary>
		public string Mod=string.Empty;

		/// <summary>
		/// The current map being played.
		/// </summary>
		public string Map=string.Empty;

		/// <summary>
		/// The number of players currently on the server.
		/// </summary>
		public int NumPlayers=0;

		/// <summary>
		/// The maximum amount of players the server or map can hold.
		/// </summary>
		public int MaxPlayers=0;

		/// <summary>
		/// The time limit of the current map.
		/// </summary>
		public TimeSpan TimeLimit;

		/// <summary>
		/// The score limit of the current map.
		/// </summary>
		public int ScoreLimit=0;

		/// <summary>
		/// Describes if the server is password protected.
		/// </summary>
		public bool Passworded=false;

		/// <summary>
		/// The server's response time to a query.
		/// </summary>
		public TimeSpan Ping;

		private readonly NameValueCollection properties;
		private readonly Collections.PlayerCollection players;

		/// <summary>
		/// Gets miscellaneous properties that dont fit into the <see cref="GameStat.ServerInfo"/>.
		/// </summary>
		public NameValueCollection Properties {
			get {
				return properties;
			}
		}

		/// <summary>
		/// Gets player information, if available.
		/// </summary>
		public Collections.PlayerCollection Players {
			get {
				return players;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.ServerInfo"/> class.
		/// </summary>
		public ServerInfo() {
			properties=new NameValueCollection();
			players=new Collections.PlayerCollection();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.ServerInfo"/> class and queries a server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.GameType"/> values.</param>
		/// <param name="host">The hostname/ip to query.</param>
		/// <param name="port">The game server's query port.  This will likely be different from the game port.</param>
		/// <returns>A new <see cref="GameStat.ServerInfo"/> class initialized with server information, or <c>null</c> if the server didn't return any information.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the server.</exception>
		/// <remarks>If <paramref name="port"/> is set to 0, GameStat will try to guess it.  If <paramref name="type"/> is a generic protocol and <paramref name="port"/> can not be guessed, an <see cref="System.ArgumentException"/> will be thrown.</remarks>
		/// <example>
		/// This example queries a QuakeWorld server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerInfo info=ServerInfo.Query(GameType.QuakeWorld, "quake.sol.net", 27511);
		///    if(info!=null) {
		///       Console.WriteLine("Host: "+info.Host);
		///       Console.WriteLine("Mod: "+info.Mod);
		///       Console.WriteLine("Map: "+info.Map);
		///       Console.WriteLine("Players: {0}/{1}", info.NumPlayers, info.MaxPlayers);
		///    }
		///    else Console.WriteLine("Server didn't return any info.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static ServerInfo Query(GameType type, string host, ushort port) {
			if(host==null) throw new ArgumentNullException("host");

			IntPtr infoptr=GSQueryServer((ushort)type, host, port);
			if(infoptr==IntPtr.Zero) {
				GSErrorCode err=GSLastError();
				switch(err) {
					case GSErrorCode.Ok:
						return null;
					case GSErrorCode.Winsock:
					case GSErrorCode.UnknownHost:
					case GSErrorCode.Connect:
					case GSErrorCode.Timeout:
						throw new System.Net.Sockets.SocketException();
					case GSErrorCode.UnknownType:
						throw new ArgumentOutOfRangeException("type");
					case GSErrorCode.Corrupt:
						throw new CorruptPacketException();
					case GSErrorCode.UnknownPort:
						throw new ArgumentException(GSErrorString(err), "port");
					case GSErrorCode.NotImplemented:
						throw new NotImplementedException(GSErrorString(err));
					case GSErrorCode.InvalidParameter:
						throw new ArgumentException(GSErrorString(err));
				}
			}

			GS_SERVERINFO info=(GS_SERVERINFO)Marshal.PtrToStructure(infoptr, typeof(GS_SERVERINFO));

			ServerInfo si=new ServerInfo();

			if(info.host!=IntPtr.Zero) si.Host=Marshal.PtrToStringAnsi(info.host);
			if(info.mod!=IntPtr.Zero) si.Mod=Marshal.PtrToStringAnsi(info.mod);
			if(info.map!=IntPtr.Zero) si.Map=Marshal.PtrToStringAnsi(info.map);

			si.NumPlayers=info.numplayers;
			si.MaxPlayers=info.maxplayers;
			si.TimeLimit=TimeSpan.FromMinutes((double)info.timelimit);
			si.ScoreLimit=info.scorelimit;
			si.Passworded=(info.passworded==1);
			si.Ping=TimeSpan.FromMilliseconds((double)info.ping);

			for(IntPtr cur=info.properties; cur!=IntPtr.Zero;) {
				GS_PROPERTY p=(GS_PROPERTY)Marshal.PtrToStructure(cur, typeof(GS_PROPERTY));

				if(p.key!=IntPtr.Zero && p.value!=IntPtr.Zero)
					si.Properties[Marshal.PtrToStringAnsi(p.key)]=Marshal.PtrToStringAnsi(p.value);

				cur=p.next;
			}

			for(IntPtr cur=info.players; cur!=IntPtr.Zero;) {
				GS_PLAYERINFO p=(GS_PLAYERINFO)Marshal.PtrToStructure(cur, typeof(GS_PLAYERINFO));
				
				if(p.name!=IntPtr.Zero) {
					Player player=new Player();
					player.Name=Marshal.PtrToStringAnsi(p.name);
					player.Score=p.score;
					player.Ping=TimeSpan.FromMilliseconds((double)p.ping);
					player.Time=TimeSpan.FromSeconds((double)p.time);

					si.Players.Add(player);
				}

				cur=p.next;
			}

			GSFreeServerInfo(infoptr);

			return si;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.ServerInfo"/> class and queries a server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.GameType"/> values.</param>
		/// <param name="host">The hostname/ip to query.</param>
		/// <returns>A new <see cref="GameStat.ServerInfo"/> class initialized with server information, or <c>null</c> if the server didn't return any information.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the server.</exception>
		/// <remarks>If <paramref name="type"/> is a generic protocol a <see cref="System.ArgumentException"/> will be thrown.</remarks>
		/// <example>
		/// This example queries a QuakeWorld server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerInfo info=ServerInfo.Query(GameType.QuakeWorld, "quake.sol.net");
		///    if(info!=null) {
		///       Console.WriteLine("Host: "+info.Host);
		///       Console.WriteLine("Mod: "+info.Mod);
		///       Console.WriteLine("Map: "+info.Map);
		///       Console.WriteLine("Players: {0}/{1}", info.NumPlayers, info.MaxPlayers);
		///    }
		///    else Console.WriteLine("Server didn't return any info.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static ServerInfo Query(GameType type, string host) {
			return Query(type, host, 0);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.Collections.ServerCollection"/> class and queries a master server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.MasterType"/> values.</param>
		/// <param name="filter">One or more of the <see cref="GameStat.MasterFilter"/> values.</param>
		/// <param name="host">The hostname/ip to query.</param>
		/// <param name="port">The master server's query port.</param>
		/// <returns>A new <see cref="GameStat.Collections.ServerCollection"/> class initialized with server information, or <c>null</c> if the master server didn't return any.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the master server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the master server.</exception>
		/// <remarks>
		///    If <paramref name="host"/> is set to null, GameStat will try to guess it.  If <paramref name="host"/> can not be guessed, an <see cref="System.ArgumentException"/> will be thrown.
		///    If <paramref name="port"/> is set to 0, GameStat will try to guess it.  If <paramref name="port"/> can not be guessed, an <see cref="System.ArgumentException"/> will be thrown.
		/// </remarks>
		/// <example>
		/// This example queries the Counter-Strike master server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerCollection servers=ServerInfo.QueryMaster(MasterType.Counter-Strike,
		///       MasterFilter.NotFull|MasterFilter.NotEmpty|MasterFilter.NoPassword, null, 0);
		///    
		///    if(servers!=null) {
		///       foreach(Server s in servers)
		///          Console.WriteLine(s.Host+":"+s.Port.ToString());
		///    }
		///    else Console.WriteLine("Master server didn't return any servers.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static Collections.ServerCollection QueryMaster(MasterType type, MasterFilter filter, string host, ushort port) {
			IntPtr serversptr=GSQueryMaster((ushort)type, host, port, (ushort)filter);
			if(serversptr==IntPtr.Zero) {
				GSErrorCode err=GSLastError();
				switch(err) {
					case GSErrorCode.Ok:
						return null;
					case GSErrorCode.Winsock:
					case GSErrorCode.UnknownHost:
					case GSErrorCode.Connect:
					case GSErrorCode.Timeout:
						throw new System.Net.Sockets.SocketException();
					case GSErrorCode.UnknownType:
						throw new ArgumentOutOfRangeException("type");
					case GSErrorCode.Corrupt:
						throw new CorruptPacketException();
					case GSErrorCode.UnknownPort:
						throw new ArgumentException(GSErrorString(err), "port");
					case GSErrorCode.NotImplemented:
						throw new NotImplementedException(GSErrorString(err));
					case GSErrorCode.InvalidParameter:
						throw new ArgumentException(GSErrorString(err));
				}
			}

			Collections.ServerCollection sc=new Collections.ServerCollection();

			for(IntPtr cur=serversptr; cur!=IntPtr.Zero;) {
				GS_SERVER server=(GS_SERVER)Marshal.PtrToStructure(cur, typeof(GS_SERVER));

				if(server.host!=IntPtr.Zero) {
					Server s=new Server();
					s.Host=Marshal.PtrToStringAnsi(server.host);
					s.Port=server.port;

					sc.Add(s);
				}

				cur=server.next;
			}

			GSFreeServers(serversptr);

			return sc;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.Collections.ServerCollection"/> class and queries a master server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.MasterType"/> values.</param>
		/// <param name="filter">One or more of the <see cref="GameStat.MasterFilter"/> values.</param>
		/// <param name="host">The hostname/ip to query.</param>
		/// <returns>A new <see cref="GameStat.Collections.ServerCollection"/> class initialized with server information, or <c>null</c> if the master server didn't return any.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the master server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the master server.</exception>
		/// <remarks>
		///    If <paramref name="host"/> is set to null, GameStat will try to guess it.  If <paramref name="host"/> can not be guessed, an <see cref="System.ArgumentException"/> will be thrown.
		/// </remarks>
		/// <example>
		/// This example queries the Counter-Strike master server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerCollection servers=ServerInfo.QueryMaster(MasterType.Counter-Strike,
		///       MasterFilter.NotFull|MasterFilter.NotEmpty|MasterFilter.NoPassword, null);
		///    
		///    if(servers!=null) {
		///       foreach(Server s in servers)
		///          Console.WriteLine(s.Host+":"+s.Port.ToString());
		///    }
		///    else Console.WriteLine("Master server didn't return any servers.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static Collections.ServerCollection QueryMaster(MasterType type, MasterFilter filter, string host) {
			return QueryMaster(type, filter, host, 0);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.Collections.ServerCollection"/> class and queries a master server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.MasterType"/> values.</param>
		/// <param name="filter">One or more of the <see cref="GameStat.MasterFilter"/> values.</param>
		/// <returns>A new <see cref="GameStat.Collections.ServerCollection"/> class initialized with server information, or <c>null</c> if the master server didn't return any.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the master server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the master server.</exception>
		/// <example>
		/// This example queries the Counter-Strike master server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerCollection servers=ServerInfo.QueryMaster(MasterType.Counter-Strike,
		///       MasterFilter.NotFull|MasterFilter.NotEmpty|MasterFilter.NoPassword);
		///    
		///    if(servers!=null) {
		///       foreach(Server s in servers)
		///          Console.WriteLine(s.Host+":"+s.Port.ToString());
		///    }
		///    else Console.WriteLine("Master server didn't return any servers.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static Collections.ServerCollection QueryMaster(MasterType type, MasterFilter filter) {
			return QueryMaster(type, filter, null, 0);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="GameStat.Collections.ServerCollection"/> class and queries a master server.
		/// </summary>
		/// <param name="type">One of the <see cref="GameStat.MasterType"/> values.</param>
		/// <returns>A new <see cref="GameStat.Collections.ServerCollection"/> class initialized with server information, or <c>null</c> if the master server didn't return any.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the master server.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The given <paramref name="type"/> is not recognized by GameStat.</exception>
		/// <exception cref="System.ArgumentException">An invalid argument was given.</exception>
		/// <exception cref="System.NotImplementedException">Support for the given <paramref name="type"/> is not implemented yet.</exception>
		/// <exception cref="GameStat.CorruptPacketException">A currupt or unknown packet was recieved from the master server.</exception>
		/// <example>
		/// This example queries the Counter-Strike master server and writes the resulting information to the console.
		/// <code lang="C#">
		/// 
		/// try {
		///    ServerCollection servers=ServerInfo.QueryMaster(MasterType.Counter-Strike);
		///    
		///    if(servers!=null) {
		///       foreach(Server s in servers)
		///          Console.WriteLine(s.Host+":"+s.Port.ToString());
		///    }
		///    else Console.WriteLine("Master server didn't return any servers.");
		/// }
		/// catch(Exception ex) {
		///    Console.Error.WriteLine("An Exception Occured!");
		///    Console.Error.WriteLine(ex.ToString());
		/// }
		/// </code>
		/// </example>
		public static Collections.ServerCollection QueryMaster(MasterType type) {
			return QueryMaster(type, MasterFilter.None, null, 0);
		}

		#region Unmanaged Types
		private struct GS_PLAYERINFO {
			public IntPtr name;

			public int score;
			public int ping;
			public int time; // in seconds.

			public IntPtr next;
		}

		private struct GS_PROPERTY {
			public IntPtr key;
			public IntPtr value;

			public IntPtr next;
		}

		private struct GS_SERVERINFO {
			public IntPtr host;
			public IntPtr mod;
			public IntPtr map;

			public int numplayers;
			public int maxplayers;
			public int timelimit; // in minutes.
			public int scorelimit;

			public IntPtr properties;
			public IntPtr players;

			// added in 1.1
			public int passworded; // 0 or 1
			public int ping; // in milliseconds
		}
		
		private struct GS_SERVER {
			public IntPtr host;
			public ushort port;

			public IntPtr next;
		}

		private enum GSErrorCode : ushort {
			Ok=0,
			Winsock,
			UnknownType,
			UnknownHost,
			Connect,
			Timeout,
			Corrupt,
			UnknownPort,
			NotImplemented,
			InvalidParameter
		}

		[DllImport("gamestat",EntryPoint="#3")]
		private static extern IntPtr GSQueryServer(ushort type, [MarshalAs(UnmanagedType.LPStr)] string host, ushort port);

		[DllImport("gamestat",EntryPoint="#4")]
		private static extern void GSFreeServerInfo(IntPtr info);

		[DllImport("gamestat",EntryPoint="#5")]
		private static extern GSErrorCode GSLastError();

		[DllImport("gamestat",EntryPoint="#6")]
		private static extern string GSErrorString(GSErrorCode err);

		[DllImport("gamestat",EntryPoint="#9")]
		private static extern IntPtr GSQueryMaster(ushort type, [MarshalAs(UnmanagedType.LPStr)] string host, ushort port, ushort filter);

		[DllImport("gamestat",EntryPoint="#10")]
		private static extern void GSFreeServers(IntPtr info);
		#endregion
	}
}
