using System;
using System.Collections;
using System.IO;

namespace Core.Managers
{
	/// <summary>
	/// Summary description for PlayerManager.
	/// </summary>
	public class PlayerManager
	{
		[Flags()]
		public enum AccessRights : int
		{
			Vote = 1,
			CallVote = 2,
			ManageVote = 4,
			ChangeMap = 8,
			GetInfo = 16,
			Kick = 32,
			Ban = 64,
			AddUsers = 128,
			AddAdmins = 256,
			RemoveAdmins = 512,

			Full = Vote | CallVote | ManageVote | ChangeMap | GetInfo | Kick | Ban | AddUsers | AddAdmins | RemoveAdmins,
		};

		[Flags()]
		public enum PlayerFlags : int
		{
			CalledVoteThisRound = 1,
			Voted = 2,
			WarnedIdleFirst = 4,
			WarnedIdleSecond = 8,
		};
		
		/// <summary>
		/// 
		/// </summary>
		public class Player
		{
			#region Events
			public event System.EventHandler ScoreChanged;
			public event System.EventHandler KillsChanged;
			public event System.EventHandler DeathsChanged;
			public event System.EventHandler TeamKillsChanged;
			public event System.EventHandler AssistsChanged;
			public event System.EventHandler TeamChanged;
			#endregion

			#region Members
			public PlayerFlags Flags;
			public AccessRights Rights;

			public int ConsecutiveKills;
			public int AveragePing;
			public int TalkingSpree;

			public string LastTKer;
			public string LastKillee;
			public DateTime JoinTime;

			#region LastActionTime
			private DateTime m_lastActionTime;
			public DateTime LastActionTime
			{
				get { return m_lastActionTime; }
				set
				{
					m_lastActionTime = value;
					Idle = false;
					Warned_idle_first = false;
					Warned_idle_second = false;
				}
			}
			#endregion

			public int TotalPlayTime
			{
				get
				{
					TimeSpan sp = DateTime.Now - m_lastActionTime;
					return (int)sp.TotalSeconds;
				}
			}

			public DateTime LastChatTime;
			public int CommandsUsed;

			#region Name
			private string _name;
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}
			#endregion

			#region CdKeyHash
			private string _cdKeyHash;
			public string CdKeyHash
			{
				get { return _cdKeyHash; }
				set { _cdKeyHash = value; }
			}
			#endregion

			#region IpAddress
			private string _ipAddress;
			public string IpAddress
			{
				get { return _ipAddress; }
				set { _ipAddress = value; }
			}
			#endregion

			#region Index
			private short _index;
			public short Index
			{
				get { return _index; }
				set { _index = value; }
			}
			#endregion

			#region MachineIndex
			private short _machineIndex;
			public short MachineIndex
			{
				get { return _machineIndex; }
				set { _machineIndex = value; }
			}
			#endregion

			#region Access
			private AccessRights _access;
			public AccessRights Access
			{
				get { return _access; }
				set { _access = value; }
			}
			#endregion

			#region Kills
			private short _kills;
			public short Kills
			{
				get { return _kills; }
				set { _kills = value; }
			}
			#endregion

			#region TeamKills
			private short _teamKills;
			public short TeamKills
			{
				get { return _teamKills; }
				set { _teamKills = value; }
			}
			#endregion

			#region Deaths
			private short _deaths;
			public short Deaths
			{
				get { return _deaths; }
				set { _deaths = value; }
			}
			#endregion

			#region Score
			private short _score;
			public short Score
			{
				get { return _score; }
				set { _score = value; }
			}
			#endregion

			#region Assists
			private short _assists;
			public short Assists
			{
				get { return _assists; }
				set { _assists = value; }
			}
			#endregion

			#region Team
			private short _team;
			public short Team
			{
				get { return _team; }
				set { _team = value; }
			}
			#endregion

			#region RespawnTimeRemaining
			private short _respawnTimeRemaining;
			public short RespawnTimeRemaining
			{
				get { return _respawnTimeRemaining; }
				set { _respawnTimeRemaining = value; }
			}
			#endregion

			#region OvershieldTimeRemaining
			private short _overshieldTimeRemaining;
			public short OvershieldTimeRemaining
			{
				get { return _overshieldTimeRemaining; }
				set { _overshieldTimeRemaining = value; }
			}
			#endregion

			#region ActiveCammoTimeRemaining
			private short _activeCammoTimeRemaining;
			public short ActiveCammoTimeRemaining
			{
				get { return _activeCammoTimeRemaining; }
				set { _activeCammoTimeRemaining = value; }
			}
			#endregion

			#region Position
			private Coordinate _position;
			public Coordinate Position
			{
				get { return _position; }
				set { _position = value; }
			}
			#endregion

			#region OtherPosition
			private Coordinate _otherPosition;
			public Coordinate OtherPosition
			{
				get { return _otherPosition; }
				set { _otherPosition = value; }
			}
			#endregion
            
			public float KillDeathRatio 
			{
				get 
				{ 
					if (_kills < 1) 
						return (float)0;

					return (float)((_kills + _deaths) / (float)_kills);
				}
			}

			public string KillDeathRatioFormatted 
			{
				get 
				{
					System.Globalization.CultureInfo c = new System.Globalization.CultureInfo("en-US");
					c.NumberFormat.CurrencyDecimalDigits = 2;
					c.NumberFormat.CurrencyDecimalSeparator = ".";
					c.NumberFormat.CurrencyGroupSeparator = "";
					return KillDeathRatio.ToString("F", c);
				}
			}
			#endregion

			public Player()
			{
				_team = -1;
				AveragePing = 0;
				LastActionTime = DateTime.Now;
				LastChatTime = DateTime.Now;
				//Admin = false;
				LastTKer = "";
				LastKillee = "";
				CommandsUsed = 0;
			}
		};

		/// <summary>
		/// Type safe array of players
		/// </summary>
		public class PlayerArray : ArrayList
		{
			public int Add(Player value)
			{
				return base.Add (value);
			}

			public new Player this[int index]
			{
				get { return base[index] as Player; }
				set { base[index] = value; }
			}
		};
	};
}