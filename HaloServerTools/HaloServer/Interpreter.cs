using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using HaloServerTools.GameState;

namespace HaloServerTools.LogfileInterface
{
	/// <summary>
	/// Interprets changes to the logfile and sends notifications accordingly.
	/// </summary>
	public class Interpreter
	{

		//public GameStatus gameStatus;
		public Scripting script;

		public Interpreter(ref ConsoleWindow cWindow)
		{
			//gameStatus = new GameStatus();
			//script = new Scripting("", ref gameStatus, ref cWindow);
		}

    #region Events

    public class PlayerEventArgs : EventArgs
    {
      #region Members Variables
      private GameState.HaloPlayer _player;
      private DateTime _timeStamp;
      #endregion

      #region Public Properties
      public GameState.HaloPlayer Player
      {
        get { return _player; }
      }
      public DateTime TimeStamp
      {
        get { return _timeStamp; }
      }
      #endregion

      #region Constructor
      public PlayerEventArgs(GameState.HaloPlayer player)
      {
        _player = player;  
        _timeStamp = DateTime.Now;
      }
      #endregion
    }
    
    /// <summary>
    /// Raised when a player types a message in the game.
    /// </summary>
    public event PlayerEventArgs PlayerSpoke;
		
    #endregion

		protected void ParseLogEntry(string line)
		{
			LogEntry entry = new LogEntry(line);
			int playerIndex = GameStateManager.Server;

			// Take the appropriate action depending on the type of logentry we are processing
			switch (entry.action.ToUpper())
			{

        case "BAN": // A player was banned
					break;
       
        // <global,team>, playerName, text
        case "CHAT": // In game chat
					string playerName = entry.parameters[1].Trim('\"');
          playerIndex = GameStateManager.PlayerManager.LocateByName(playerName);
					
          if (playerIndex != GameStateManager.Server)
					{
						// 
            // Check for spam
						TimeSpan tsp = DateTime.Now.Subtract(gameStatus.players[playerIndex].lastChatTime);
						if (tsp.TotalSeconds < 3)
						{
							gameStatus.players[playerIndex].talkingSpree++;
						}
						else
						{
							int points = (int)tsp.TotalSeconds / 5;
							gameStatus.players[playerIndex].talkingSpree -= points;
							if (gameStatus.players[playerIndex].talkingSpree < 0)
								gameStatus.players[playerIndex].talkingSpree = 0;
						}

						gameStatus.players[playerIndex].lastChatTime = System.DateTime.Now;
						gameStatus.players[playerIndex].LastActionTime = System.DateTime.Now;
											
						// Convert vote keywords to the correct word
						string temp = entry.parameters[2].ToLower();
						if ((temp == "yes") || (temp == "y") || (temp == "1"))
							entry.parameters[2] = "!yes";
						if ((temp == "no") || (temp == "n") || (temp == "0"))
							entry.parameters[2] = "!no";

						// Convert Shortcut commands to the full command name
						temp = entry.parameters[2].ToLower();
						if (temp.StartsWith("!p ")) entry.parameters[2] = entry.parameters[2].Replace("!p ","!player ");
						if (temp.StartsWith("!bl ")) entry.parameters[2] = entry.parameters[2].Replace("!bl ","!banlist ");
						if (temp.StartsWith("!m ")) entry.parameters[2] = entry.parameters[2].Replace("!m ","!map ");
						if (temp.StartsWith(">")) entry.parameters[2] = entry.parameters[2].Replace(">","!say ");
						if (temp.StartsWith("!a ")) entry.parameters[2] = entry.parameters[2].Replace("!a ","!admin ");
						if (temp.StartsWith("!t ")) entry.parameters[2] = entry.parameters[2].Replace("!t ","!taunt ");
						if (temp.StartsWith("!t")) entry.parameters[2] = entry.parameters[2].Replace("!t","!taunt");
						if (temp.StartsWith("!tkkick ")) entry.parameters[2] = entry.parameters[2].Replace("!tk ","!tkkick ");
						if (temp.StartsWith("!tkban ")) entry.parameters[2] = entry.parameters[2].Replace("!tb ","!tkban ");
						
						// Check for command triggers and parse them
						if (entry.parameters[2].StartsWith("!"))
						{
							// This could potentially be a triggered event.
							string parse1 = entry.parameters[2].Trim('!');
							string cmd = "";
							string[] parse2 = StringFunctions.ParseCommand(parse1, out cmd);

							if (parse2.Length == 1)
							{
								script.ExecuteLocal(cmd, null, playerIndex);
							}
							else
							{
								script.ExecuteLocal(cmd, GetElements(parse2, 1), playerIndex);



							}
						}
						//_ircBot.Send(entry.parameters[1].Trim('\"') + ": " + entry.parameters[2]);
						// If remote logging is enabled, send this
						// (ToDo)
					}
					
					break;
				case "CLOSE": // Logfile was closed
					// This file will receive no more entries (and has been renamed)
					// We need to close it, and open the new file of the same name.
					//HaloServer hs = ((HaloServer)ServerManager.ServerList[0]);
					//hs._serverExtensions.ReInitializeLogfileMonitor();
					break;
				case "GAMEINFO": // Game settings, map and mode
					// <settings>, <map>, mapName, <mode>, modeName
					gameStatus.InitializeGameState(entry.parameters[2], entry.parameters[4]);
					gameStatus.CycleInProgress = true;
					gameStatus.matchEnded = false;
					break;
				case "GRIEF": // Teamkill
					// <Added,Removed>, Player #, Player Name, Number of TK's
					//interaction.gameState.ProcessAction(entry);
					switch (entry.parameters[0])
					{
						case "ADDED":
							playerIndex = gameStatus.players.LocateByName(entry.parameters[2].Trim('\"'));
							if (playerIndex > -1)
							{
								gameStatus.players[playerIndex].kills++;
								gameStatus.players[playerIndex].LastActionTime = System.DateTime.Now;
							}
							break;
						case "REMOVED":
							// If we start tracking tk points rather than just tks, this may be used.
							break;
					}
					break;
				case "JOIN": // Player joining server
					// <failure,success>, (opt:if <failed>)reason, playerName, player #
					// machine #, IP, cdkeyHash
					if (entry.parameters[0] == "SUCCESS")
					{
						Trace.WriteLine("Player Joined: " + entry.parameters[1].Trim('\"') + " - Adding to the game.");
						// Player joined - add them to the active player collection
						Player p = new Player();
						p.joinTime = entry.timeStamp;
						p.keyHash = entry.parameters[5].Remove(0, 6);
						p.ip = entry.parameters[4].Substring(1, entry.parameters[4].Length-2); // ex: (192.168.0.55:2303)
						p.name = entry.parameters[1].Trim('\"');
						string s = entry.parameters[2].Replace("player ", "");
						p.slot = Convert.ToInt32(s);
						s = entry.parameters[3].Replace("machine ", "");
						p.machine = Convert.ToInt32(s);
						
						// If a ghost player already exists in the slot, remove him.
						int ghostPlayerIndex = gameStatus.players.LocateByPlayerNumber(p.machine);
						if (ghostPlayerIndex > -1)
						{
							gameStatus.players.Remove(gameStatus.players[ghostPlayerIndex]);
						}
						
						// Determine admin status
						HaloServer hs = (HaloServer)ServerManager.ServerList[0];
						if (hs._serverExtensions.Admins.Contains(p.keyHash))
						{
							p.admin = true;
						}
						if (p.admin)
						{
							//_ircBot.Send("Admin Joined: " + p.name + " (" + p.keyHash + ") at " + p.joinTime.ToString());
						}
						else
						{
							//_ircBot.Send(p.name + " (" + p.keyHash + ") joined the server at " + p.joinTime.ToString());
						}
					
						// Add the player to the game status
						Console.Write("Player set up - adding to the array");
						//p.StartPinging();
						gameStatus.players.Add(p);

						PlayerArrayList players = script.GetRecentPlayersByKeyHash(p.keyHash);
						if (players.Count > 0)
						{
							for (int x=0; x<players.Count; x++)
							{
								if (x == 0)
								{
									if (p.name != players[0].name) 
									{
										script.ServerSay(p.name + " recently played as " + players[0].name);
									}
								}
								else
								{
									if (p.name != players[0].name) 
									{
										script.ServerSay("and " + players[0].name);
									}
								}
							}
						}
						Trace.WriteLine("Done!  Player added to the game");
					}
					break;
				case "TEAMS":
					// <JOIN/LEAVE>, playerName, player#(player x), team #(team x)
					if (entry.parameters[0] == "JOIN")
					{
						playerIndex = gameStatus.players.LocateByName(entry.parameters[1].Trim('\"'));
						if (playerIndex > -1)
						{
							string temp = (entry.parameters[3].Trim('\"')).Trim("team ".ToCharArray());
							gameStatus.players[playerIndex].team = Convert.ToInt32(temp);
							string s = entry.parameters[2].Replace("player ", "");
							gameStatus.players[playerIndex].slot = Convert.ToInt32(s);
							gameStatus.players[playerIndex].LastActionTime = System.DateTime.Now;
						}
					}
					break;
				case "KILL": // Who killed who, including assists
					// killer, killerPlayer #, <killed>, killee, kileePlayer #,
					// (opt:if assist occurred)<assist>, (opt)playerName, (opt)player #
					
					// Killer
					int killerIndex = gameStatus.players.LocateByName(entry.parameters[0].Trim('\"'));
					int killeeIndex = gameStatus.players.LocateByName(entry.parameters[3].Trim('\"'));

					if ((killerIndex > -1) && (killeeIndex > -1))
					{
						// Before we award the kill, we need to make sure that it wasn't a TK
						if ((gameStatus.players[killerIndex].team == -1) ||
							(gameStatus.players[killerIndex].team != gameStatus.players[killeeIndex].team))
						{
							gameStatus.players[killerIndex].lastKillee = gameStatus.players[killeeIndex].name;
							gameStatus.players[killerIndex].kills++;
							gameStatus.players[killerIndex].LastActionTime = System.DateTime.Now;
							
							// Random taunts - sometimes, we will give the player a free taunt that is automatic
							if ((script.r.Next(10)) == 5) {
								string cmd = "TAUNT";
								string[] p = new string[1];
								p[0] = (string)gameStatus.players[killeeIndex].name;
								p[1] = "tauntforfree";
								script.ExecuteLocal(cmd, p, playerIndex);
							}

							// Announce killing sprees and running riots
                            /*if ((gameStatus.players[killerIndex].kills > 0) && 
								((gameStatus.players[killerIndex].kills % 5) == 0))
							{
								string name = gameStatus.players[killerIndex].name;
								int total = gameStatus.players[killerIndex].consecutiveKills / 5;
								if (total == 1) script.ServerSay(name + " is on a killing spree!");
								if (total == 2) script.ServerSay(name + " just started a RUNNING RIOT!");
								if (total == 3) script.ServerSay(name + " has a *DOUBLE* RUNNING RIOT!");
								if (total == 4) script.ServerSay(name + " has a *TRIPLE* RUNNING RIOT!");
								if (total == 5) script.ServerSay("HOLY CRAP! " + name + " has " +
									Convert.ToString(gameStatus.players[killerIndex].kills) + " CONSECUTIVE kills!");
								if (total == 6) script.ServerSay(name + " is pwning with " +
									Convert.ToString(gameStatus.players[killerIndex].kills) + " CONSECUTIVE kills!");
								if (total == 7) script.ServerSay("I've lost count!  " + name + " is DOMINATING this round!");
							}*/
						}
					}

					// Killee
					if ((killerIndex > -1) && (killeeIndex > -1))
					{
						gameStatus.players[killeeIndex].deaths++;

						if (entry.parameters[0].Trim('\"') == entry.parameters[3].Trim('\"'))
						{
							//_ircBot.Send(entry.parameters[0].Trim('\"') + " committed suicide.");
						}
						else
						{
							if (gameStatus.players[killerIndex].team !=
								gameStatus.players[killeeIndex].team)
							{
								//_ircBot.Send(entry.parameters[0].Trim('\"') + " killed " + entry.parameters[3].Trim('\"'));
							}
							else
							{
								// If the teams are -1, this isn't a team game.
								// Otherwise, if they are equal, this is a betrayal.
								if (gameStatus.players[killerIndex].team != -1)
								{
									if (script.IsAdmin(killeeIndex))
									{
										script.ServerSay(gameStatus.players[killerIndex].name + " - Warning - you have betrayed an admin!");
										gameStatus.players[killeeIndex].lastTKer = gameStatus.players[killerIndex].name;
									}
									//_ircBot.Send(entry.parameters[3].Trim('\"') + " was betrayed by " + entry.parameters[0].Trim('\"'));
								}
							}
						}
					}
					// Assister
					if (entry.parameters.Length > 5)
					{
						// We have an assist!
						playerIndex = gameStatus.players.LocateByName(entry.parameters[6].Trim('\"'));
						if (playerIndex > -1)
						{
							gameStatus.players[playerIndex].assists++;
							gameStatus.players[playerIndex].LastActionTime = System.DateTime.Now;
						}
					}

					break;
				case "NOTE": // Note from the sv_log_note command
					// parameters
					break;
				case "OPEN": // Opened the logfile
					// <Log opened>
					break;
				case "PCR_PLAYER": // Postgame carnage report player info
					// rank(Place x), playerNumberAndName(Player x "Name"), team #(Team x), 
					// numOfKills(Kills x), numAssists(Assists x), numDeaths(Deaths x), score(Score x)
					// This will need to be combined with other lines from the log to makeup a PCR entry
					// that will be sent to the stats server if remote stats are enabled.
					if (!gameStatus.matchEnded)
					{
						gameStatus.matchEnded = true;	
						// Call the end of game trigger
						script.MatchEnded();
					}
					break;
				case "PCR_TEAM": // Postgame carnage report team info
					// team(Team x), <Score>, score(x)
					string tmp = entry.parameters[0].Replace("Team ","");
					script.ExecuteLocal("PGCR", new string[] {tmp}, -10);
					break;
				case "QUIT": // Player quit
					// playerName, (opt)player #(player x), machine #(machine x), ip, (opt)cdkeyHash
					//interaction.gameState.ProcessAction(entry);
					
					// If we don't have a name, locate by IP
					if (entry.parameters[0] == "<No Player>")
					{
						playerIndex = gameStatus.players.LocateByIp(
							entry.parameters[1].Substring(1, entry.parameters[1].Length-2)
							);
					}
					else
					{
						playerIndex = gameStatus.players.LocateByName(entry.parameters[0].Trim('\"'));
					}
					
					if (playerIndex > -1)
					{
						// Add player to the RecentPlayers collection - Maximum of 50 elements
						Player p = gameStatus.players[playerIndex];
						if (gameStatus.RecentPlayers.Count > 50) {
							gameStatus.RecentPlayers.Remove(gameStatus.RecentPlayers[0]);
						}
						gameStatus.RecentPlayers.Add(p);
						
						// Remove the player from the game
						gameStatus.players.Remove(p);
						//_ircBot.Send(entry.parameters[0].Trim('\"') + " has left the game.");
						//_ircBot.Send(gameStatus.players[playerIndex].name + " has left the game.");
					}
					break;
				case "RCON": // RCON commands that have been issued
					// <succeeded,failed>, player #(player x), playerName, machine #(machine x),
					// ip, command(COMMAND: commandText)
					if (entry.parameters[0] == "SUCCEEDED")
					{
						// If the map cycle was restarted, update the game status appropriately
						if (entry.parameters[5] == "COMMAND: sv_mapcycle_begin")
						{
							gameStatus.MapCycleIndex = 0;
						}
					}
					break;
				case "SCORE": // A team scored
					// <CTF>, player #(Player x), playerName, team(team x)
					//interaction.gameState.ProcessAction(entry);
					if (entry.parameters[0] == "CTF")
					{
						playerIndex = gameStatus.players.LocateByName(entry.parameters[2].Trim('\"'));
						if (playerIndex > -1) 
						{
							gameStatus.players[playerIndex].score++;
						}
						gameStatus.players[playerIndex].LastActionTime = System.DateTime.Now;
					}
					//_ircBot.Send(entry.parameters[2].Trim('\"') + " scored!");
					break;
			}
		}

		public static string[] GetElements(string[] source, int startElement)
		{
			string[] newArray = new string[(source.Length)-startElement];
			for (int x=0; x<(source.Length)-startElement; x++)
			{
				newArray[x] = source[x+startElement];
			}
			return newArray;
		}

		// Format a log entry and submit it to the stats server
		private void RemoteLog(LogEntry entry)
		{
			// Not yet implemented
		}

	}
}
