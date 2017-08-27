using System;
using System.Collections;
using System.Threading;
using HaloServerTools;
using HaloServerTools.CoreMemory;

namespace HaloServerTools.GameState
{

  public class GameStateManager
  {
    public const int Server = -1;
  }

  public class HaloPlayer
	{

    #region Classes, Structs, and Enums
    public struct Coordinate
    {
      public float X;
      public float Y;
      public float Z;
    }

    public enum AccessRights
    {
      Vote,
      CallVote,
      ManageVote,
      ChangeMap,
      GetInfo,
      Kick,
      Ban,
      AddUsers,
      AddAdmins,
      RemoveAdmins
    }
    #endregion

    #region Events
    public event EventArgs ScoreChanged;
    public event EventArgs KillsChanged;
    public event EventArgs DeathsChanged;
    public event EventArgs TeamKillsChanged;
    public event EventArgs AssistsChanged;
    public event EventArgs TeamChanged;
    #endregion

    #region Member Variables
    private string _name;
    private string _cdKeyHash;
    private string _ipAddress;
    private short _index;
    private short _machineIndex;

    private AccessRights _access;

    private short _kills;
    private short _score;
    private short _teamKills;
    private short _deaths;
    private short _assists;
    private short _team;

    private short _respawnTimeRemaining;
    private short _overshieldTimeRemaining;
    private short _activeCammoTimeRemaining;

    private Coordinate _position;
    private Coordinate _otherPosition;
    #endregion

    #region Public Properties
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    public string CdKeyHash
    {
      get { return _cdKeyHash; }
      set { _cdKeyHash = value; }
    }
    public string IpAddress
    {
      get { return _ipAddress; }
      set { _ipAddress = value; }
    }
    public short Index
    {
      get { return _index; }
      set { _index = value; }
    }
    public short MachineIndex
    {
      get { return _machineIndex; }
      set { _machineIndex = value; }
    }

    public AccessRights Access
    {
      get { return _access; }
      set { _access = value; }
    }

    public short Kills
    {
      get { return _kills; }
      set { _kills = value; }
    }
    public short TeamKills
    {
      get { return _teamKills; }
      set { _teamKills = value; }
    }
    public short Deaths
    {
      get { return _deaths; }
      set { _deaths = value; }
    }
    public short Score
    {
      get { return _score; }
      set { _score = value; }
    }
    public short Assists
    {
      get { return _assists; }
      set { _assists = value; }
    }
    public short Team
    {
      get { return _team; }
      set { _team = value; }
    }

    public short RespawnTimeRemaining
    {
      get { return _respawnTimeRemaining; }
      set { _respawnTimeRemaining = value; }
    }
    public short OvershieldTimeRemaining
    {
      get { return _overshieldTimeRemaining; }
      set { _overshieldTimeRemaining = value; }
    }
    public short ActiveCammoTimeRemaining
    {
      get { return _activeCammoTimeRemaining; }
      set { _activeCammoTimeRemaining = value; }
    }

    public Coordinate Position
    {
      get { return _position; }
      set { _position = value; }
    }
    public Coordinate OtherPosition
    {
      get { return _otherPosition; }
      set { _otherPosition = value; }
    }
    #endregion

    #region Public Methods
    public void UpdatePlayerFromCore(HaloCoreMemoryInterface.PlayerData data)
    {
      if ((Kills != data.Kills) && (KillsChanged != null)) KillsChanged();
      if ((Deaths != data.Deaths) && (DeathsChanged != null)) DeathsChanged();
      if ((TeamKills != data.TKs) && (TeamKillsChanged != null)) TeamKillsChanged();
      if ((Score != data.Score) && (ScoreChanged != null)) ScoreChanged();
      if ((Assists != data.Assists) && (AssistsChanged != null)) AssistsChanged();
      if ((Team != data.Team) && (TeamChanged != null)) TeamChanged();

      Kills = data.Kills;
      Deaths = data.Deaths;
      Name = data.PlayerName;
      Assists = data.Assists;
      TeamKills = data.TKs;
      Score = data.Score;
      Team = data.Team;
      OvershieldTimeRemaining = data.OvershieldTimeLeft;
      RespawnTimeRemaining = data.RespawnTime;
      Position.X = data.X;
      Position.Y = data.Y;
      Position.Z = data.Z;
      OtherPosition.X = data.PredictedX;
      OtherPosition.Y = data.PredictedY;
      OtherPosition.Z = data.PredictedZ;
    }
    
    public HaloCoreMemoryInterface.PlayerData UpdateCoreFromPlayer()
    {
      HaloCoreMemoryInterface.PlayerData data = new HaloCoreMemoryInterface.PlayerData();
      data.Kills = Kills;
      data.Deaths = Deaths;
      data.PlayerName = Name;
      data.Assists = Assists;
      data.TKs = TeamKills;
      data.Score = Score;
      data.Team = Team;
      data.OvershieldTimeLeft = OvershieldTimeRemaining;
      data.RespawnTime = RespawnTimeRemaining;
      data.X = Position.X;
      data.Y = Position.Y;
      data.Z = Position.Z;
      data.PredictedX = OtherPosition.X;
      data.PredictedY = OtherPosition.Y;
      data.PredictedZ = OtherPosition.Z;
    }
    #endregion

    public int consecutiveKills;
		public int assists;
		public int tkPoints;
		public int score;
		public int team;
		public int averagePing;

		// For pinging the player and getting an average result
		ArrayList recentPings;

		public int talkingSpree;

		public float ratio {
			get { 
				if (m_kills < 1) {
					return (float)0;
				}
				return (float)((m_kills + m_deaths) / (float)m_kills);
			}
		}
		public string RatioFormatted {
			get {
				System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("en-US");
				c.NumberFormat.CurrencyDecimalDigits = 2;
				c.NumberFormat.CurrencyDecimalSeparator = ".";
				c.NumberFormat.CurrencyGroupSeparator = "";
				return ratio.ToString("F",c);
			}
		}
		public string lastTKer;
		public string lastKillee;
		public DateTime joinTime;
		private DateTime m_lastActionTime;
		private System.Threading.Thread pingTh;

		public DateTime LastActionTime
		{
			get { return m_lastActionTime; }
			set
			{
				m_lastActionTime = value;
				idle = false;
				warned_idle_first = false;
				warned_idle_second = false;
			}
		}
		public int TotalPlayTime
		{
			get
			{
				TimeSpan sp = DateTime.Now - m_lastActionTime;
				return (int)sp.TotalSeconds;
			}
		}
		public DateTime lastChatTime;
		public bool calledVoteThisRound;
		public bool voted;
		public bool idle;
		public bool warned_idle_first;
		public bool warned_idle_second;
		public int commandsUsed;
		public Player()
		{
			team = -1;
			averagePing = 0;
			LastActionTime = DateTime.Now;
			lastChatTime = DateTime.Now;
			admin = false;
			lastTKer = "";
			lastKillee = "";
			commandsUsed = 0;
		}
		public void StartPinging()
		{
			//pingTh = new System.Threading.Thread(new System.Threading.ThreadStart(PingerThread));
			//pingTh.Priority = System.Threading.ThreadPriority.Lowest;
			//pingTh.Start();
		}
		private void PingerThread()
		{
			try
			{
				recentPings = new ArrayList();
				while (true)
				{
					int pingTime = Ping.PingHost(ip.Split(':')[0]);
					if (recentPings.Count >= 10) recentPings.RemoveAt(0);
					recentPings.Add(pingTime);
					// Get the average ping of the player
					int totalTime = 0;
					for (int x=0; x<recentPings.Count-1; x++)
					{
						totalTime += (int)recentPings[x];
					}
					averagePing = totalTime / recentPings.Count;
					Console.WriteLine("Average ping: " + Convert.ToString(averagePing));
					Thread.Sleep(2000);
				}

			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(
					"There was an unexpected error pinging player: " + ip + "\r\n" +
					"Ping aborted on this IP.\r\n" +
					"Error: " + ex.Message);
			}
		}
	}

	// An arraylist of HaloPC players
	public class PlayerArrayList : ArrayList
	{
		private bool ready;
		public PlayerArrayList()
		{
			ready = true;
		}
		public void Add(HaloPlayer p) 
		{
			try
			{
				// Set up a timer safety
				DateTime dt = DateTime.Now;
				while (!ready)
				{
					// If its been more than 5 seconds, break - we have an infinite loop
					if (((TimeSpan)DateTime.Now.Subtract(dt)).TotalSeconds > 5) break;
				} // Loop until we have a ready status
				
				// Add the player
				ready = false;
				if (!base.Contains(p)) base.Add(p);
				ready = true;
			}
			catch
			{
				ready = true;
			}
		}
		public override void Clear()
		{
			ready = false;
			base.Clear ();
			ready = true;
		}

		public void Remove(HaloPlayer p) 
		{
			if (base.Contains(p)) base.Remove(p);
		}
		public new HaloPlayer this [int Index] 
		{
			get 
			{
				return (HaloPlayer) base[Index];
			}
			set  
			{
				base[Index] = value;
			}
		}
		public int LocateByName (string name)
		{
			for (int x=0; x<base.Count; x++)
			{
				HaloPlayer p = (HaloPlayer)base[x];
				string baseName = p.Name.ToUpper();
				name = name.ToUpper();
				
				// If the strings are equal, return the value
				if (baseName == name) return x;

				// Do a LIKE comparison
				if (StringFunctions.Like(baseName, name, true)) return x;
			}
			return -1;
		}
		public int LocateByIp (string ip)
		{
			for (int x=0; x<base.Count; x++)
			{
				if (((HaloPlayer)base[x]).IpAddress == ip) return x;
			}
			return -1;
		}
		public int LocateByKeyHash (string keyhash)
		{
			for (int x=0; x<base.Count; x++)
			{
				if (((HaloPlayer)base[x]).CdKeyHash == keyhash) return x;
			}
			return -1;
		}
		public int LocateByPlayerNumber (int p)
		{
			for (int x=0; x<base.Count; x++)
			{
				if (((HaloPlayer)base[x]).MachineIndex == p) return x;
			}
			return -1;
		}
	}

	// The status of a HaloPC game
	public class GameStatus
	{
		public string CurrentMap;
		public string CurrentGameMode;
		public bool CycleInProgress;
		public bool CycleInterrupted;
		public PlayerArrayList players;
		public PlayerArrayList RecentPlayers;
		public int numberOfVotesCalled;
		public ArrayList mapCycle;
		public int MapCycleIndex;
		public int MaxPlayers;
		public bool matchEnded;
		public GameStatus()
		{
			players = new PlayerArrayList();
			RecentPlayers = new PlayerArrayList();
			CycleInProgress = true;
			CycleInProgress = false;
			matchEnded = false;
			
			// If available, read in the map cycle file.  if not, create an empty cycle arraylist
			mapCycle = new ArrayList();
			MapCycleIndex = 0;
			HaloServer h = (HaloServer)ServerManager.ServerList[0];
			string initFile = h._startupFolder + "\\init.txt";
			
			MaxPlayers = 16; // Default, in case a value isn't specified in the init.txt
			try
			{
				if (System.IO.File.Exists(initFile))
				{
					System.IO.StreamReader sr = new System.IO.StreamReader(
						new System.IO.FileStream(initFile, System.IO.FileMode.Open));
					string fullFile = sr.ReadToEnd();
					fullFile = fullFile.Replace("\r\n","\n");
					string[] lines = fullFile.Split('\n');
					
					for (int x=0; x<lines.Length-1; x++)
					{
						if (lines[x].StartsWith("sv_mapcycle_add"))
						{
							string entry = lines[x].Replace("sv_mapcycle_add ","");
							mapCycle.Add(entry);
						}
						if (lines[x].StartsWith("sv_maxplayers"))
						{
							string entry = lines[x].Replace("sv_maxplayers ","");
							MaxPlayers = Convert.ToInt32(MaxPlayers);
						}
					}
				}
				else
				{
					mapCycle.Add("No mapcycle found on this server.");
				}
			}
			catch
			{
				System.Diagnostics.Trace.WriteLine("Error: Failed to read settings from init.txt");
			}
		}
	    public void InitializeGameState(string currentMap, string currentGameType)
		{
			CurrentMap = currentMap;
			CurrentGameMode = currentGameType;
			numberOfVotesCalled = 0;
			players.Clear();
			RecentPlayers.Clear();
		}
	}
}
