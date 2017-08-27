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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hive.Node.Core.ServerInterfaces
{
	/// <summary>
	/// Provides a means for interfacing with a Teamspeak server.
	/// </summary>
  public class TeamspeakServerInterface : IDisposable
  {
    private Socket querySocket;
    private string host;

    private bool isDisposed = false;

    private TeamspeakServerInfo serverInfo;

    /// <summary>
    /// The host address of the server.
    /// </summary>
    public string Host
    {
      get { return host; }
    }

    /// <summary>
    /// Contains the results of a successful query.
    /// </summary>
    public TeamspeakServerInfo ServerInfo
    {
      get { return serverInfo; }
    }

    #region Constructor / Destructor
    /// <summary>
    /// Creates a new instance of the object based on the given parameters.
    /// </summary>
    /// <param name="host">The host address of the server.</param>
    public TeamspeakServerInterface(string host)
    {
      this.host = host;
      querySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
      OpenConnection();
    }

    ~TeamspeakServerInterface()
    {
      Dispose();
    }
    #endregion

    //TODO: Verify that a current server is selected before issuing certain commands.
    //TODO: Verify that the user is logged in the the appropriate permissions before issuing certain commands.

    /// <summary>
    /// Sends a command to the server on the active socket, appending a line feed character.
    /// </summary>
    /// <param name="data">The string data to be sent.</param>
    private void SendCommand(string data)
    {
      try
      {
        querySocket.Send(Encoding.Default.GetBytes(data + "\n"));
      }
      catch (Exception ex)
      {
        throw ex;      
      }
    }

    /// <summary>
    /// Receives a response from the server on the active socket.  This is a blocking method.
    /// </summary>
    /// <param name="size"></param>
    /// <returns>A string array containing the response (split into lines) received from the server.</returns>
    private string[] ReceiveResponse(int size)
    {
      byte[] bin = new byte[size];
      querySocket.Receive(bin);
      string s = Encoding.Default.GetString(bin);
      s = s.Replace("\r", "");
      s = s.Replace("\0", "");
      string[] items = s.Split('\n');
      return items;
    }

    /// <summary>
    /// Opens a connection to the Teamspeak server.
    /// </summary>
    private void OpenConnection()
    {
      // Connect on port 51234, which is the default Teamspeak master service port.
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.host), 51234);
      querySocket.Connect(remoteEP);

      // Ensure that this is a valid Teamspeak server.
      if (ReceiveResponse(128)[0] != "[TS]")
        throw new Exception("The server returned an invalid response. It is most likely not a valid Teamspeak 2 server.");
    }

    /// <summary>
    /// Closes the currently active connection to the Teamspeak server.
    /// </summary>
    private void CloseConnection()
    {
      SendCommand("quit");
      querySocket.Close();
    }

    /// <summary>
    /// Retreives basic information about the server.
    /// </summary>
    private void GetServerInfo()
    {
      SendCommand("si");
      string[] s = ReceiveResponse(2048);
      serverInfo = new TeamspeakServerInfo(s);
    }

    /// <summary>
    /// Selects an individual Teamspeak server on the host as the active server.
    /// </summary>
    /// <param name="port">The port of the server to select.</param>
    public void SelectServer(int port)
    {
      SendCommand("sel " + port.ToString());
      if (ReceiveResponse(128)[0] != "OK")
        throw new Exception("Invalid response received from Teamspeak server.");
    }

    /// <summary>
    /// Retreives a list of channels on the server.
    /// </summary>
    private int GetChannelList()
    {
      SendCommand("cl");
      string[] s = ReceiveResponse(32000);
      if (s[0] == "error")
        throw new Exception("Invalid response received from Teamspeak server.");

      int channelCount = s.Length-3; // Don't count the first set, or the last 2



      // Add all returned channels to the list.
      this.serverInfo.Channels = new TeamspeakChannel[channelCount];
      for (int x=0; x<channelCount; x++)
      {
        string[] parts = s[x+1].Split('\t');
        this.serverInfo.Channels[x] = new TeamspeakChannel(parts);
      }
      return channelCount;
    }

    /// <summary>
    /// Retreives a list of players on the server.
    /// </summary>
    private int GetPlayerList()
    {
      SendCommand("pl");
      string[] s = ReceiveResponse(32000);
      if (s[0] == "error")
        throw new Exception("Invalid response received from Teamspeak server.");

      int playerCount = s.Length-3; // Don't count the first set, or the last 2

      // Add all returned players to the list.
      this.serverInfo.Players = new TeamspeakPlayer[playerCount];
      for (int x=0; x<playerCount; x++)
      {
        string[] parts = s[x+1].Split('\t');
        this.serverInfo.Players[x] = new TeamspeakPlayer(parts);
      }
      return playerCount;
    }

    /// <summary>
    /// Contacts the specified server and updates all information.
    /// </summary>
    /// <param name="port">The port of the server to query.</param>
    public void Query(int port)
    {
      SelectServer(port);
      GetServerInfo();
      GetChannelList();
      GetPlayerList();
    }

    /// <summary>
    /// Logs into the server as a SuperAdmin.
    /// </summary>
    /// <param name="username">A valid SuperAdmin username.</param>
    /// <param name="password">A valid SuperAdmin password.</param>
    public void SuperAdminLogin(string username, string password)
    {
      SendCommand("slogin " + username + " " + password);
      if (ReceiveResponse(128)[0] != "OK")
        throw new Exception("Invalid username/password specified.");
    }

    /// <summary>
    /// Logs into the server as a normal user.
    /// </summary>
    /// <param name="username">A valid username.</param>
    /// <param name="password">A valid password.</param>
    public void NormalLogin(string username, string password)
    {
      SendCommand("login " + username + " " + password);
      if (ReceiveResponse(128)[0] != "OK")
        throw new Exception("Invalid username/password specified.");
    }

    /// <summary>
    /// Returns an array containing the ports of all Teamspeak 2 servers running in the current process.
    /// </summary>
    /// <returns>An array of server ports.</returns>
    public int[] GetServerPortList()
    {
      SendCommand("sl");
      string[] s = ReceiveResponse(32000);
      if (s[0] == "error")
        throw new Exception("Invalid response received from Teamspeak server.");
      
      int serverCount = s.Length-2; // Don't count the the last 2
      int[] results = new int[serverCount]; 
      for (int x=0; x<serverCount; x++)
      {
        results[x] = Convert.ToInt32(s[x]);
      }
      return results;
    }
    
    /// <summary>
    /// Retreives a list of all virtual teamspeak servers in the database.
    /// </summary>
    public TeamspeakServer[] GetServerList()
    {
      SendCommand("dbserverlist");
      string[] s = ReceiveResponse(32000);
      if (s[0] == "error")
        throw new Exception("Invalid response received from Teamspeak server.");
      
      int serverCount = s.Length-3; // Don't count the first line, or the last 2

      TeamspeakServer[] results = new TeamspeakServer[serverCount]; 
      for (int x=0; x<serverCount; x++)
      {
        results[x] = new TeamspeakServer(s[x+1].Split('\t'));
      }
      return results;
    }

    /// <summary>
    /// Gets the database identifier of a server on the specified port.
    /// </summary>
    /// <param name="port">The port of the teamspeak server you are trying to locate in the database.</param>
    /// <returns>The database identifier of the server matching the supplied port.</returns>
    public int GetDatabaseID(int port)
    {
      TeamspeakServer[] servers = GetServerList();
      foreach (TeamspeakServer server in servers)
      {
        if (server.Port == port)
          return server.DatabaseID;
      }
      return -1;
    }

    /// <summary>
    /// Sends a global message to all players on the currently selected server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void ServerSay(string message)
    {
      string cmd = "msg \"" + message + "\"";
      SendCommand(cmd);
      string[] s = ReceiveResponse(1024);
      if (s[0] != "OK")
        throw new Exception("Error sending global message: " + s[0]);
    }

    /// <summary>
    /// Moves the specified player to the specified channel.
    /// </summary>
    /// <param name="playerID">The id of the player to move.</param>
    /// <param name="channelID">The id of the channel to move the player to.</param>
    public void MovePlayerToChannel(int playerID, int channelID) 
    {
      string command = "mptc " + channelID.ToString() + " " +  playerID.ToString() + "\n";
      SendCommand(command);
      string response = ReceiveResponse(512)[0];
      if (response != "OK")
        Console.WriteLine("Error moving player to channel - server returned: " + response);
    }

    /// <summary>
    /// Grants server admin status to a player.
    /// </summary>
    /// <param name="playerID">A valid player ID.</param>
    public void GrantServerAdmin(int playerID)
    {
      SendCommand("sppriv " + playerID.ToString() + " privilege_serveradmin 1");
      string response = ReceiveResponse(512)[0];
      if (response != "OK")
        Console.WriteLine("Error granting server admin status - server returned: " + response);
    }

    /// <summary>
    /// Revokes server admin status from a player.
    /// </summary>
    /// <param name="playerID">A valid player ID.</param>
    public void RevokeServerAdmin(int playerID)
    {
      SendCommand("sppriv " + playerID.ToString() + " privilege_serveradmin 0");
      string response = ReceiveResponse(512)[0]; 
      if (response != "OK")
        Console.WriteLine("Error revoking server admin status - server returned: " + response);
    }

    /// <summary>
    /// Gets user's attention (toggle's server admin status a specified number of times very quickly).
    /// </summary>
    public void GetAttention(int id, int length)
    {
      foreach (TeamspeakPlayer p in serverInfo.Players)
      {
        if (p.PlayerID == id)
        {
          for (int x=0; x<length; x++)
          {
            if (p.GetUserStatus(PlayerUserStatus.ServerAdmin))
            {
              RevokeServerAdmin(p.PlayerID);
              GrantServerAdmin(p.PlayerID);
            }
            else
            {
              GrantServerAdmin(p.PlayerID);
              RevokeServerAdmin(p.PlayerID);        
            }
          }
        }
      }
    }

    /// <summary>
    /// Starts the Teamspeak server that is configured on the specified port.
    /// </summary>
    /// <param name="port">A valid UDP port.</param>
    public void StartServer(int port)
    {
      int id = GetDatabaseID(port);
      if (id == -1)
      {
        throw new Exception("Unable to start server: The specified port was not found in the database. (" + port.ToString() + ")");
      }
      SendCommand("serverstart " + id.ToString());
      string response = ReceiveResponse(512)[0]; 
      if (response != "OK")
        throw new Exception("Error starting server on port" + port.ToString() + ": " + response);
    }

    /// <summary>
    /// Stop sthe specified Teamspeak server.
    /// </summary>
    /// <param name="port">A valid UDP port.</param>
    public void StopServer(int port)
    {
      SelectServer(port);
      SendCommand("serverstop " + port.ToString());
      string response = ReceiveResponse(512)[0]; 
      if (response != "OK")
        throw new Exception("Error stopping server on port" + port.ToString() + ": " + response);
    }

    /// <summary>
    /// Quickly moves user to random channels the specified number of times.
    /// </summary>
    public void Slap(int id, int length)
    {
      Random r = new Random(DateTime.Now.Millisecond);
      foreach (TeamspeakPlayer p in serverInfo.Players)
      {
        if (p.PlayerID == id)
        {
          for (int x=0; x<length; x++)
          {
            int channelIndex = r.Next(0, this.serverInfo.Channels.Length-1);
            int channelID = this.serverInfo.Channels[channelIndex].ID;
            MovePlayerToChannel(p.PlayerID, channelID);
          }
        }
      }
    }

    /// <summary>
    /// Slaps all users in the server.
    /// </summary>
    public void WreakHavok(int length)
    {
      Random r = new Random(DateTime.Now.Millisecond);
      for (int x=0; x<length; x++)
      {
        foreach (TeamspeakPlayer p in serverInfo.Players)
        {
          int channelIndex = r.Next(0, this.serverInfo.Channels.Length-1);
          int channelID = this.serverInfo.Channels[channelIndex].ID;
          MovePlayerToChannel(p.PlayerID, channelID);
        }
      }
    }

    public void Dispose()
    {
      if (!this.isDisposed)
      {
        CloseConnection();
        this.isDisposed = true;
      }
    }
  }
  
  /// <summary>
  /// Represents a TeamspeakServer and it's basic identity information.
  /// </summary>
  public class TeamspeakServer
  {
    
    private int databaseID;
    private int port;
    private string name;
    private int type;

    /// <summary>
    /// The database ID of the server.
    /// </summary>
    public int DatabaseID
    {
      get { return databaseID; }
    }

    /// <summary>
    /// The UDP port that the server is running on.
    /// </summary>
    public int Port
    {
      get { return port; }
    }

    /// <summary>
    /// The tname of the server.
    /// </summary>
    public string Name
    {
      get { return name; }
    }

    /// <summary>
    /// The type of server: 1 = public, 2 = clan.
    /// </summary>
    public int Type
    {
      get { return type; }
    }

    /// <summary>
    /// Creates the object from a pre-split string response to a 'dbserverlist' command.
    /// </summary>
    /// <param name="data">The pre-split response received from the server.</param>
    public TeamspeakServer(string[] data)
    {
      this.databaseID = Convert.ToInt32(data[0]);
      this.port = Convert.ToInt32(data[1]);
      this.name = data[2];
      this.type = Convert.ToInt32(data[3]);
    }
  }

  /// <summary>
  /// Represents a player on a Teamspeak server.
  /// </summary>
  public class TeamspeakPlayer
  {
    private int playerID;
    private int channelId;
    private int receivedPackets;
    private int receivedBytes;
    private int sentPackets;
    private int sentBytes;
    private int packetLoss;
    private int pingTime;
    private int totalTime;
    private int idleTime;
    private int privileges;
    private int userStatus;
    private int attributes;
    private string ip;
    private string playerName;
    private string loginName;
    
    /// <summary>
    /// The ID of the player on the server.
    /// </summary>
    public int PlayerID
    {
      get { return playerID; }
    }

    public int ChannelId
    {
      get { return channelId; }
    }

    public int ReceivedPackets
    {
      get { return receivedPackets; }
    }

    public int ReceivedBytes
    {
      get { return receivedBytes; }
    }

    public int SentPackets
    {
      get { return sentPackets; }
    }

    public int SentBytes
    {
      get { return sentBytes; }
    }

    public int PacketLoss
    {
      get { return packetLoss; }
    }

    public int PingTime
    {
      get { return pingTime; }
    }

    public int TotalTime
    {
      get { return totalTime; }
    }

    public int IdleTime
    {
      get { return idleTime; }
    }

    public int Privileges
    {
      get { return privileges; }
    }

    public int UserStatus
    {
      get { return userStatus; }
    }

    public int Attributes
    {
      get { return attributes; }
    }

    public string Ip
    {
      get { return ip; }
    }

    /// <summary>
    /// The visible name of the player on the server.
    /// </summary>
    public string PlayerName
    {
      get { return playerName; }
    }

    /// <summary>
    /// The name used by the player to login to the server.  Usually different from the visible name.
    /// </summary>
    public string LoginName
    {
      get { return loginName; }
    }

    /// <summary>
    /// Creates the object from a pre-split string response to a 'pl' command.
    /// </summary>
    /// <param name="data">The pre-split response received from the server.</param>
    public TeamspeakPlayer(string[] data)
    {
      this.playerID = Convert.ToInt32(data[0]);
      this.channelId = Convert.ToInt32(data[1]);
      this.receivedPackets = Convert.ToInt32(data[2]);
      this.receivedBytes = Convert.ToInt32(data[3]);
      this.sentPackets = Convert.ToInt32(data[4]);
      this.sentBytes = Convert.ToInt32(data[5]);
      this.packetLoss = Convert.ToInt32(data[6]) / 100;
      this.pingTime = Convert.ToInt32(data[7]);
      this.totalTime = Convert.ToInt32(data[8]);
      this.idleTime = Convert.ToInt32(data[9]);
      this.privileges = Convert.ToInt32(data[10]);
      this.userStatus = Convert.ToInt32(data[11]);
      this.attributes = Convert.ToInt32(data[12]);
      this.ip = data[13].Replace("\"", "");
      this.playerName = data[14].Replace("\"", "");
      this.loginName = data[15].Replace("\"", "");;
    }

    public bool GetPrivilege(PlayerPrivileges privileges)
    {
      return ((this.privileges & (int)privileges) == (int)privileges);
    }
    public bool GetUserStatus(PlayerUserStatus userStatus)
    {
      return ((this.userStatus & (int)userStatus) == (int)userStatus);
    }
    public bool GetAttribute(PlayerAttributes attributes)
    {
      return ((this.attributes & (int)attributes) == (int)attributes);
    }
  }

  public enum PlayerPrivileges
  {
    ChannelAdmin = 1,
    Operator = 2,
    Voice = 4,
    AutoOperator = 8,
    AutoVoice = 16,
  }
  
  public enum PlayerUserStatus
  {
    ServerAdmin = 1,
    Registered = 4
  }

  public enum PlayerAttributes
  {
    Recording = 64
  }

  /// <summary>
  /// Represents a channel on Teamspeak server.
  /// </summary>
  public class TeamspeakChannel
  {
    private int id;
    
    /// <summary>
    /// The channel ID.
    /// </summary>
    public int ID
    {
      get { return this.id; }
    }
    
    /// <summary>
    /// Creates the object from a pre-split string response to a 'cl' command.
    /// </summary>
    /// <param name="data">The pre-split response received from the server.</param>
    public TeamspeakChannel(string[] data)
    {
      this.id = Convert.ToInt32(data[0]);
    }
  }

  /// <summary>
  /// Contains basic global information about a Teamspeak server.
  /// </summary>
  public class TeamspeakServerInfo
  {
    private TeamspeakPlayer[] players;
    private TeamspeakChannel[] channels;
    private int maxPlayers;
    private string serverName;

    /// <summary>
    /// The active players on the server as of the last query.
    /// </summary>
    public TeamspeakPlayer[] Players
    {
      get { return this.players; }
      set { this.players = value; }
    }

    /// <summary>
    /// The channels on this server as of the last query.
    /// </summary>
    public TeamspeakChannel[] Channels
    {
      get { return this.channels; }
      set { this.channels = value; }
    }

    /// <summary>
    /// The maximum number of players allows on this server.
    /// </summary>
    public int MaxPlayers
    {
      get { return maxPlayers; }
    }

    /// <summary>
    /// The display name of this server.
    /// </summary>
    public string ServerName
    {
      get { return serverName; }
    }

    /// <summary>
    /// Creates the object from a pre-split string response to a 'si' command.
    /// </summary>
    /// <param name="data">The pre-split response received from the server.</param>
    public TeamspeakServerInfo(string[] data)
    {
      Hashtable values = ParseResults(data);
      this.maxPlayers = Convert.ToInt32(values["server_maxusers"]);
      this.serverName = (string)values["server_name"];
    }
   
    private Hashtable ParseResults(string[] results)
    {
      Hashtable values = new Hashtable();
      for (int x=0; x< results.Length-1; x++)
      {
        string[] s = results[x].Split('=');
        if (s.Length > 1)
        {
          string name = s[0].Replace("\"", "");
          string val = s[1].Replace("\"", "");
          values.Add(name, val);
        }
      }
      return values;
    }
  }
}
