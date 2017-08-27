using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using ScreenGrabber.ScreenUtils;
using Microsoft.CSharp;
using System.IO;
using System.Threading;

namespace HaloServerTools
{
	/// <summary>
	/// Load and compile the server interaction scripts
	/// </summary>
  public class Scripting
  {
    // Conatins our compiled assembly.
    private CompilerResults compresult;
    private object instance;
    private GameStatus _gameStatus;
    private ConsoleWindow _consoleWindow;
    private System.Timers.Timer t;
    private System.Collections.Hashtable DefaultGameTypes;

    public Vote currentVote;
    public int allPurposeCounter;
    public Random r;

    #region "Class Constructor"
    public Scripting(string scriptsFolder, ref GameStatus gStatus, ref ConsoleWindow cWindow)
    {
      _gameStatus = gStatus;
      _consoleWindow = cWindow;
      r = new Random(DateTime.Now.Millisecond);
			
      InitializeDefaultGameTypes();

      currentVote = new Vote(false);
      currentVote.status = Vote.VoteStatus.Idle;

      // Initialize the event timer function
      t = new System.Timers.Timer(1000);
      t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
      t.Start();

      return; // **TEMP

      string sourceCode="";
      string classHeader = "namespace HaloServerTools {\r\n" + 
        "\tpublic class hstScript {\r\n";
      string classFooter = "\r\n}\r\n}";
      StreamReader sr;

      // Read in all of the hst scripts
      foreach (string filename in Directory.GetFiles(scriptsFolder, ".hcs"))
      {
        sr = new StreamReader(filename);
        sourceCode += sr.ReadToEnd();
        sourceCode += "\r\n";
        sr.Close();
      }
			
      // Combine the header/footer with the source from the script files
      sourceCode = classHeader + sourceCode + classFooter;

      // Compile the assembly
      CSharpCodeProvider cscp = new CSharpCodeProvider();

      CompilerParameters compparams = new CompilerParameters();
      compparams.GenerateExecutable = false;
      compparams.OutputAssembly="hstScript.dll";
      compparams.ReferencedAssemblies.Add("System.dll");

      ICodeCompiler cscompiler = cscp.CreateCompiler();
      compresult = cscompiler.CompileAssemblyFromSource(compparams, sourceCode);
      instance = compresult.CompiledAssembly.CreateInstance("HaloServerTools.hstScript");
    }
    private void InitializeDefaultGameTypes()
    {
      DefaultGameTypes = new System.Collections.Hashtable();
      DefaultGameTypes.Add("classic slayer",null);
      DefaultGameTypes.Add("classic slayer pro",null);
      DefaultGameTypes.Add("classic elimination",null);
      DefaultGameTypes.Add("classic phantoms",null);
      DefaultGameTypes.Add("classic endurance",null);
      DefaultGameTypes.Add("classic rockets",null);
      DefaultGameTypes.Add("classic snipers",null);
      DefaultGameTypes.Add("classic team slayer",null); 
      DefaultGameTypes.Add("classic oddball",null);
      DefaultGameTypes.Add("classic team oddball",null);
      DefaultGameTypes.Add("classic reverse tag",null);
      DefaultGameTypes.Add("classic accumulation",null);
      DefaultGameTypes.Add("classic juggernaut",null);
      DefaultGameTypes.Add("classic stalker",null);
      DefaultGameTypes.Add("classic king",null);
      DefaultGameTypes.Add("classic king pro",null); 
      DefaultGameTypes.Add("classic crazy king",null);
      DefaultGameTypes.Add("classic team king",null);
      DefaultGameTypes.Add("classic ctf",null);
      DefaultGameTypes.Add("classic ctf pro",null); 
      DefaultGameTypes.Add("classic invasion",null);
      DefaultGameTypes.Add("classic iron ctf",null);
      DefaultGameTypes.Add("classic race",null);
      DefaultGameTypes.Add("classic rally",null);
      DefaultGameTypes.Add("classic team race",null);
      DefaultGameTypes.Add("classic team rally",null);
      DefaultGameTypes.Add("team slayer",null);
      DefaultGameTypes.Add("team race",null);
      DefaultGameTypes.Add("team oddball",null);
      DefaultGameTypes.Add("team king",null);
      DefaultGameTypes.Add("slayer",null);
      DefaultGameTypes.Add("race",null);
      DefaultGameTypes.Add("oddball",null);
      DefaultGameTypes.Add("king",null);
      DefaultGameTypes.Add("juggernaut",null);
      DefaultGameTypes.Add("ctf",null);
      DefaultGameTypes.Add("crazy king",null);
      DefaultGameTypes.Add("assault",null);
    }
    #endregion

    private object ExecuteScript(string scriptName, object[] parameters)
    {
      Type clsType = compresult.CompiledAssembly.GetType("HaloServerTools.hstScript");
      MethodInfo methodinf = clsType.GetMethod("scriptname");
      return methodinf.Invoke(instance, parameters);
    }

    public void ExecuteLocal(string scriptName, object[] parameters, int playerID)
    {
      try
      {
        Type t = Type.GetType("HaloServerTools.Scripting");
        MethodInfo m = t.GetMethod(scriptName.ToUpper());
        if (parameters == null) parameters = new object[0];
        object[] passedParams;
        if (parameters.Length > 0)
        {
          passedParams = new object[2];
          passedParams[0] = parameters;
          passedParams[1] = playerID;
        }
        else
        {
          passedParams = new object[2];
          passedParams[0] = new object[1];
          passedParams[1] = playerID;
        }
        m.Invoke(this, passedParams);
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
      }
    }

    #region "[ Command: !keyhash ]"

    public void KEYHASH(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      string name = (string)parameters[0];
      bool playerFound = false;
      foreach (Player p in _gameStatus.players)
      {
        string baseName = p.name.ToUpper();
        name = name.ToUpper();
			
        // If the strings are equal, return the value
        if ((baseName == name) ||
          // Do a LIKE comparison
          (StringFunctions.Like(baseName, name, true)))
        {
          ServerSay("[KeyHash] " + p.name + " - [ " + p.keyHash + " ]");
          playerFound = true;
        }
      }
      if (!playerFound)
      {
        ServerSay ("No matching players found.");
      }
    }
    #endregion

    #region "[ Command: !ip ]"
    public void IP(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      string name = (string)parameters[0];
      bool playerFound = false;
      foreach (Player p in _gameStatus.players)
      {
        string baseName = p.name.ToUpper();
        name = name.ToUpper();
			
        // If the strings are equal, return the value
        if ((baseName == name) ||
          // Do a LIKE comparison
          (StringFunctions.Like(baseName, name, true)))
        {
          ServerSay("[IP] " + p.name + " - [ " + p.ip + " ]");
          playerFound = true;
        }
      }
      if (!playerFound)
      {
        ServerSay ("No matching players found.");
      }
    }
    #endregion

    #region "[ Command: !idletime ]"
    public void IDLETIME(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      string name = (string)parameters[0];
      bool playerFound = false;
      foreach (Player p in _gameStatus.players)
      {
        string baseName = p.name.ToUpper();
        name = name.ToUpper();
			
        // If the strings are equal, return the value
        if ((baseName == name) ||
          // Do a LIKE comparison
          (StringFunctions.Like(baseName, name, true)))
        {
          int idleSeconds = (int)((TimeSpan)DateTime.Now.Subtract(p.LastActionTime)).TotalSeconds;
          ServerSay("[Idle Time] " + p.name + " - " + Convert.ToString(idleSeconds) + " seconds");
          playerFound = true;
        }
      }
      if (!playerFound)
      {
        ServerSay ("No matching players found.");
      }
    }
    #endregion

    #region "[ Command: !mapnext ]"
    public void MAPNEXT(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      ServerSay("Moving to next map in the cycle.");
      ChangeMap(_gameStatus.MapCycleIndex + 1);
    }
    #endregion

    #region "[ Command: !banlist ]"
    public void BANLIST(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return; // Make sure we have parameters

      string keyword = ((string)parameters[0]).ToLower();
			
      if (keyword == "search")
      {
        if (parameters.Length < 2) // Make sure we have the second parameter
        {
          ServerSay("No criteria was specified.");
          ServerSay("USAGE: !banlist search playername");
          return;
        }
				
        string criteria = ((string)parameters[1]).ToLower();
        HaloServer hs = (HaloServer)ServerManager.ServerList[0];
				
        ArrayList players = hs._serverExtensions.banlist.FindPlayerByName(criteria);
        if (players.Count < 1)
        {
          ServerSay ("No matching players found.");
          return;
        }
        foreach (Banlist.BannedPlayer b in players)
        {
          // The player does exist in the banlist - return the info
          ServerSay("[" + Convert.ToString(b.banlistIndex) + "] " + b.name);
        }
      }
      if (keyword == "remove")
      {
        if (parameters.Length < 2) // Make sure we have the second parameter
        {
          ServerSay("You must supply a banlist ID number.");
          ServerSay("USAGE: !banlist remove #");
          return;
        }
        HaloServer hs = (HaloServer)ServerManager.ServerList[0];
        int banIndex = Convert.ToInt32(((string)parameters[1]).ToLower());
        if ((banIndex > -1) && (banIndex < hs._serverExtensions.banlist.Entries.Length))
        {
          // Unban this player
          ServerSay("Unbanning player: " + hs._serverExtensions.banlist.Entries[banIndex].name);
          _consoleWindow.QueueCommand("sv_unban " + Convert.ToString(banIndex));
        }
      }
    }
    #endregion

    #region "[ Command: !show ]"
    public void SHOW(object[] parameters, int playerID)
    {
      if (parameters.Length < 1) return; // Make sure we have parameters
      if (playerID > -1) _gameStatus.players[playerID].commandsUsed++;
      if ((_gameStatus.players[playerID].commandsUsed >= 2) && (!IsAdmin(playerID))) return;
      if (((string)parameters[0]).ToLower() == "nextmap")
      {
        int i = _gameStatus.MapCycleIndex + 1;
        if (i > (_gameStatus.mapCycle.Count-1)) i = 0;
        ServerSay("Next map up: " + (string)_gameStatus.mapCycle[i]);	
      }
    }
    #endregion

    #region "[ Command: !taunt ]"
    public void TAUNT(object[] parameters, int playerID)
    {
      //if (!tauntsEnabled) return;
      if (parameters.Length < 1) return; // Make sure we have parameters
      if ((_gameStatus.players[playerID].commandsUsed > 2) && (!IsAdmin(playerID))) return;
			
      int killee = _gameStatus.players.LocateByName(_gameStatus.players[playerID].lastKillee);
      if ((killee < 0) && (parameters.Length < 2) && (!IsAdmin(playerID))) return;

      string dead = "";
      string killer = "";
			
      try
      {
        // If the name was specified, use it instead
        //** make sure that the killee index isn't -1, which it will be if you haven't killed someone
        if (killee > -1) 
        {
          dead = _gameStatus.players[killee].name;
        }
        if ((object)parameters[0] != null) 
        {
          if (((string)parameters[0]).Length > 1) 
          {
            dead = (string)parameters[0];
          }
        }
        killer = _gameStatus.players[playerID].name;
      }
      catch
      {
        return;
      }

			
      if (killer=="") return;
      if (dead=="") return;
			
      // Open the taunts file and read in all available taunts
      string startupFolder = ((HaloServer)ServerManager.ServerList[0])._startupFolder;
      if (!File.Exists(startupFolder + "\\taunts.txt")) 
      {
        ServerSay("Taunts file was not found - taunts disabled!");
        return;
      }
      StreamReader sr = new StreamReader(
        new FileStream(
        startupFolder + "\\taunts.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        ,System.Text.Encoding.Default);
      string tmp = sr.ReadToEnd();
      sr.Close();
      tmp = tmp.Replace("\r\n","\n");
      string[] tauntList = tmp.Split('\n');
      int numberOfTaunts = tauntList.Length;
      if (numberOfTaunts < 1) 
      {
        ServerSay("No taunts found in tauntlist file - taunts disabled!");
        return;
      }
      int tauntNumber = r.Next(numberOfTaunts-1);
      string catchPhrase = tauntList[tauntNumber];
      catchPhrase = catchPhrase.Replace("\"","'");

      // Make sure that the player isn't taunting MonoxideC, because he pwns you.
      if (killee > -1) 
      {
        if (((Player)_gameStatus.players[killee]).keyHash == "db26f410896043617842d54cbfd4535f")
          catchPhrase = "%d pwns you, %k";
      }

      string s = catchPhrase.Replace("%k",killer);
      s = s.Replace("%d",dead);
      if (playerID > -1) _gameStatus.players[playerID].commandsUsed++;
      try 
      {
        // Make sure this wasn't a free taunt
        if ((parameters.Length  == 2))
        {
          if ((string)parameters[1] == "tauntforfree") 
          {
            if (playerID > -1) _gameStatus.players[playerID].commandsUsed--;
          }
        }
      }
      catch 
      {
      }
      ServerSay(s);
    }
    #endregion

    /*#region "[ Command: !say ]"
    public void SAY(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return; // Make sure we have parameters
      ServerSay((string)parameters[0]);	
    }
      #endregion*/

    #region "[ Command: !say ]"
    public void SAY(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return; // Make sure we have parameters

      string sayText = "";

      for(int i = 0; i < parameters.Length; i++)
      {
        sayText += ((string)parameters[i]) + " ";
      }

      ServerSay(sayText);	
    }
    #endregion

    #region "[ Command: !kill ]"
    public void KILL(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;

      // Get the player name/criteria
      string name = (string)parameters[0];
			
      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        HaloCoreMemoryInterface.PlayerData[] mplayers = new HaloCoreMemoryInterface.PlayerData[1];// GetScriptPlayersFromMemory();
        for (int x=0; x<mplayers.Length; x++)
        {
          string playerName = LogfileMonitor.DeUnicode(new string(mplayers[x].UnicodePlayerName));
          if (playerName.StartsWith(p.name))
          {
            _consoleWindow.QueueCommand("if (!= (unit_get_health (unit (list_get (players) " + x.ToString() + "))) -1) (unit_kill (unit (list_get (players) " + (string)parameters[0] + "))) (sv_say \"Player Not Found\")");            
            ServerSay(p.name + " was pwned by the server.");
          }
        }
      }
    }
    #endregion


    #region "[ Command: !cmd ]"
    public void CMD(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return; // Make sure we have parameters

      string scriptText = "";

      for(int i = 0; i < parameters.Length; i++)
      {
        scriptText += ((string)parameters[i]) + " ";
      }
      _consoleWindow.QueueCommand(scriptText);
    }
    #endregion

    #region "[ Command: !eject ]"
    public void EJECT(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      _consoleWindow.QueueCommand("unit_exit_vehicle (unit (list_get (players) " + (string)parameters[0] + "))");
			
      ServerSay("Player " + (string)parameters[0] + " was forced to exit vehicle");
			
    }
    #endregion

    #region "[ Command: !freeze ]"
    public void FREEZE(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      _consoleWindow.QueueCommand("unit_suspended (unit (list_get (players) " + (string)parameters[0] + ")) true");
			
      ServerSay("Player " + (string)parameters[0] + " can no longer move - point and laugh!");		
			
    }
    #endregion

    #region "[ Command: !unfreeze ]"
    public void UNFREEZE(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      _consoleWindow.QueueCommand("unit_suspended (unit (list_get (players) " + (string)parameters[0] + ")) false");
			
      ServerSay("Player " + (string)parameters[0] + " can move again");
			
    }
    #endregion

    #region "[ Command: !all ]"
    public void ALL(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      //if (parameters.Length < 1) return;

      switch (((string)parameters[0]).ToLower())
      {
          /*case "kill":

            for(int i = 0; i < 16; i++)
            {
              object[] o = new object[2];
              o[0] = (object)(i.ToString());
              o[1] = (object)"silent";
              KILL(o,-1);
            }				

            break;
            */
          /*case "eject":

            for(int i = 0; i < 16; i++)
            {
              object[] o = new object[2];
              o[0] = (object)(i.ToString());
              o[1] = (object)"silent";;
              EJECT(o,-1);
            }				

            break;*/
        case "freeze":
          //for(int i = 0; i < 16; i++)
          //{
          _consoleWindow.QueueCommand("player_enable_input false");
          ServerSay ("Freezing All Players");
          //}
          break;
        case "unfreeze":
          //for(int i = 0; i < 16; i++)
          //{
          _consoleWindow.QueueCommand("player_enable_input true");
          ServerSay ("Unfreezing All Players");
          //}
          break;
        default:
          ServerSay("Invalid Use: freeze and unfreeze supported");
          return;
      }

				
    }
    #endregion

    #region "[ Command: !vehicles ]"
    public void VEHICLES(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      ServerSay("Spawning Vehicles");
      _consoleWindow.QueueCommand("cheat_all_vehicles");
    }
    #endregion

    #region "[ Command: !warthog ]"
    public void WARTHOG(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      ServerSay("Spawning Warthog");
      _consoleWindow.QueueCommand("cheat_spawn_warthog");
    }
    #endregion

    #region "[ Command: !weapons ]"
    public void WEAPONS(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      ServerSay("Spawning Weapons");
      _consoleWindow.QueueCommand("cheat_all_weapons");
    }
    #endregion

    #region "[ Command: !destroy ]"
    public void DESTROY(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      ServerSay("Destroying All Scenery and Vehicles");
      _consoleWindow.QueueCommand("object_destroy_all");
      ServerSay("You may now walk through rocks");
    }
    #endregion

    #region "[ Command: !deathless ]"
    public void DEATHLESS(object[] parameters, int playerID)
    {
      string cheatState = "";
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (((string)parameters[0]).ToLower() == "on") 
      {
        cheatState = "true";				
      }
      else if (((string)parameters[0]).ToLower() == "off") 
      {
        cheatState = "false";				
      }
      _consoleWindow.QueueCommand("cheat_deathless_player " + cheatState);
      ServerSay("Deathless Player is now " + ((string)parameters[0]).ToLower());				

    }
    #endregion

    #region "[ Command: !instantkill ]"
    public void INSTANTKILL(object[] parameters, int playerID)
    {
      string cheatState = "";
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (((string)parameters[0]).ToLower() == "on") 
      {
        cheatState = "true";				
      }
      else if (((string)parameters[0]).ToLower() == "off") 
      {
        cheatState = "false";				
      }
      _consoleWindow.QueueCommand("cheat_omnipotent " + cheatState);
      ServerSay("Instant Kill is now " + ((string)parameters[0]).ToLower());				

    }
    #endregion

    #region "[ Command: !falldamage ]"
    public void FALLDAMAGE(object[] parameters, int playerID)
    {
      string cheatState = "";
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (((string)parameters[0]).ToLower() == "on") 
      {
        cheatState = "false";				
      }
      else if (((string)parameters[0]).ToLower() == "off") 
      {
        cheatState = "true";				
      }
      _consoleWindow.QueueCommand("cheat_jetpack " + cheatState);
      ServerSay("Fall damage is now " + ((string)parameters[0]).ToLower());				

    }
    #endregion

    #region "[ Command: !superjump ]"
    public void SUPERJUMP(object[] parameters, int playerID)
    {
      string cheatState = "";
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (((string)parameters[0]).ToLower() == "on") 
      {
        cheatState = "true";				
      }
      else if (((string)parameters[0]).ToLower() == "off") 
      {
        cheatState = "false";				
      }
      _consoleWindow.QueueCommand("cheat_super_jump " + cheatState);
      ServerSay("Super Jump is now " + ((string)parameters[0]).ToLower());				

    }
    #endregion

    #region "[ Command: !infammo ]"
    public void INFAMMO(object[] parameters, int playerID)
    {
      string cheatState = "";
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (((string)parameters[0]).ToLower() == "on") 
      {
        cheatState = "true";				
      }
      else if (((string)parameters[0]).ToLower() == "off") 
      {
        cheatState = "false";				
      }
      _consoleWindow.QueueCommand("cheat_infinite_ammo " + cheatState);
      ServerSay("Infinite Ammo is now " + ((string)parameters[0]).ToLower());				

    }
    #endregion

    #region "[ Command: !givetaunts ]"
    public void GIVETAUNTS(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 2) 
      {
        ServerSay("Invalid Syntax!");
        ServerSay("Ex: !givetaunts *mono* 4");
        return; // Make sure we have parameters
      }
      // Get the player name/criteria
      string name = (string)parameters[0];
      // Get the number of commands
      int n = Convert.ToInt32((string)parameters[1]);
      string ns = (string)parameters[1];

      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1) 
      {
        ServerSay ("No matching players found.");
        return;
      }

      // Cycle through results and act accordingly
      foreach (Player p in players) 
      {
				
        p.commandsUsed -= n;
        ServerSay("Giving " + ns + " free taunts to " + p.name);
        ServerSay("You now have " + Convert.ToString(2 - p.commandsUsed) + " taunts remining.");
      }
    }
    #endregion

    #region "[ Command: !reset ]"
    public void RESET(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      _consoleWindow.QueueCommand("sv_map_reset");
    }
    #endregion

    #region "[ Command: !tkkick ]"
    public void TKKICK(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      // See if someone has tked you
      if (_gameStatus.players[playerID].lastTKer == "")
      {
        ServerSay ("You have not been tked!");
        return;
      }
      ServerSay("Admin teamkiller found: " + _gameStatus.players[playerID].lastTKer);
      // Kick this player
      object[] o = new object[1];
      o[0] = (object)_gameStatus.players[playerID].lastTKer;
      KICK(o,playerID);
      // Reset tker value since this person is no longer in the game
      _gameStatus.players[playerID].lastTKer = "";
    }
    #endregion

    #region "[ Command: !tkban ]"
    public void TKBAN(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      // See if someone has tked you
      if (_gameStatus.players[playerID].lastTKer == "")
      {
        ServerSay ("You have not been tked!");
        return;
      }
      // Determine if a length of time parameter was specified
      string p = "";
      if ( parameters.Length > 0) 
      {
        p = (string)parameters[0];
      }
      else
      {
        p = "";
      }
      if (p == "1d")
      {
        ServerSay(_gameStatus.players[playerID].lastTKer + 
          " - Default tkban time of 1 day enforced.");
      }
      else
      {
        ServerSay(_gameStatus.players[playerID].lastTKer + 
          " - TKBan enforced.");
      }
      // Find the player number of the one we want to get rid of
      int tkerIndex = _gameStatus.players.LocateByName(_gameStatus.players[playerID].lastTKer);
      // Actually... probably wont need this...			

      // Ban the player
      object[] o = new object[2];
      o[0] = (object)_gameStatus.players[playerID].lastTKer;
      o[1] = (object)p;
      BAN(o,playerID);
      // Reset tker value since this person is no longer in the game
      _gameStatus.players[playerID].lastTKer = "";
    }
    #endregion

    #region "[ Command: !map ]"
    public void MAP(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      if (parameters.Length < 2)
      {
        ServerSay("Map: Invalid parameters.");
        return;
      }
			
      string map = (string)parameters[0];
      string gametype = (string)parameters[1];
			
      // Make sure that the map and gametype are valid
      //1. Look for maps in the \MAPS folder
      HaloServer h = (HaloServer)ServerManager.ServerList[0];
      bool bMapFound = false;
      if (File.Exists(h._startupFolder + "\\MAPS\\" + map + ".map")) bMapFound = true;
      //2. Look for game types in the folder and in the hashtable
      bool bGameTypeFound = false;
      if (File.Exists(h._serverExtensions._dataFolder + "\\savegames\\" + gametype + "\\" + gametype)) bGameTypeFound = true;
      if (DefaultGameTypes.Contains(gametype)) bGameTypeFound = true;
      if (!bMapFound)
      {
        ServerSay("Invalid map specified: " + map);
        return;
      }
      if (!bGameTypeFound)
      {
        ServerSay("Invalid gametype specified: " + gametype);
        return;
      }
      _consoleWindow.QueueCommand("sv_map " + map + " " + "\"" + gametype + "\"");
      _gameStatus.CycleInterrupted = true; // We are interrupting the mapcycle
    }
    #endregion

    #region "[ Command: !mapcycle ]"
    public void MAPCYCLE(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      string keyword = ((string)parameters[0]).ToLower();
      if (keyword == "list")
      {
        int x=0;
        foreach (string entry in _gameStatus.mapCycle)
        {
          ServerSay("[" + Convert.ToString(x) + "] " + entry);
        }
      }
      if (keyword == "restart")
      {
        _gameStatus.MapCycleIndex = 0;
        ServerSay("Restarting map cycle.");
        _consoleWindow.QueueCommand("sv_mapcycle_begin");
      }
    }
    #endregion

    #region "[ Command: !admin ]"
    public void ADMIN(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
      string keyword = ((string)parameters[0]).ToLower();
			
      try
      {
        // Make a temporary local copy so that it can be referenced easier
        ServerExtensions.AdminList admins = ((HaloServer)ServerManager.ServerList[0])._serverExtensions.Admins;
			
        if (keyword == "list")
        {
          // They just want the current list
          int x = 0;
          foreach (System.Collections.DictionaryEntry d in admins)
          {
            x++;
            ServerExtensions.PlayerRecord pr = (ServerExtensions.PlayerRecord)d.Value;
            string playerName = pr.name;
            string playerhash = pr.keyhash;
            int id = pr.id;
            ServerSay("[ ID:" + Convert.ToString(id) + " ] " + playerName);
          }
          return;
        }
        // The player wants to either add or remove someone from the admins list
        // First, let's handle adds
        if (keyword == "add") 
        {
          if (parameters.Length < 2) 
          {
            ServerSay("You must specify a player to add.");
            ServerSay("Ex: !admin add =EP_Mono=TO");
            return;
          }
          string playerCriteria = (string)parameters[1];
          ArrayList p = GetPlayers(playerCriteria);
          if (p.Count == 0) 
          {
            ServerSay("No matching players found.");
            return;
          }
          if (p.Count > 1) 
          {
            ServerSay("More than one player matches the specified criteria.");
          }
          Player playerToAdd = (Player)p[0];

          // Make sure the player is not already an admin
          if (admins.Contains(playerToAdd.name)) 
          {
            ServerSay("Player is already an admin: " + playerToAdd.name);
            return;
          }
          // Add this player's name and keyhash to the database
          String sql = "INSERT INTO haloce_server_admins " + 
            "(server_id, name, key_hash) " +
            "VALUES (1, '" + playerToAdd.name + "','" + playerToAdd.keyHash + "');";
          int recordsAdded = DB.RunCommand(sql);
          if (recordsAdded < 1) 
          {
            Trace.WriteLine("ERROR: Could not insert admin into database.\r\n" + "SQL Command: " + sql);
            ServerSay("Error: Failed to add admin to database.");
            return;
          }
          else 
          {
            playerToAdd.admin = true;
            ServerSay("Player added to admin database: " + playerToAdd.name);
            ((HaloServer)ServerManager.ServerList[0])._serverExtensions.ReadAdmins();
            return;
          }
        }
        if (keyword == "remove") 
        {
          if (parameters.Length < 2) 
          {
            ServerSay("You must specify a valid admin ID number");
            ServerSay("Ex: !admin remove #1");
            ServerSay("Use !admin search <criteria> to get the admin ID.");
            return;
          }
				
          // Make sure that the admin exists in the current admin table
          string playerNumber = ((string)parameters[1]).Replace("#","");
          int slot = Convert.ToInt32(playerNumber);
          ServerExtensions.PlayerRecord pr = admins.LocateById(slot);
          if ((Object)pr == null) 
          {
            ServerSay("Player not found.");
            return;
          }
          ServerSay("Removing " + pr.name	+ "from the admin database");

          // Remove the specified player frm the database
          String sql = "DELETE FROM haloce_server_admins " + 
            "WHERE id = " + Convert.ToString(pr.id);
          int recordsDeleted = DB.RunCommand(sql);
          if (recordsDeleted < 1) 
          {
            Trace.WriteLine("ERROR: Could not remove admin from database.\r\n" + "SQL Command: " + sql);
            ServerSay("Error: Failed to remove admin from database.");
            return;
          }
          else 
          {
            // If the player is in the game currently, revoke their admin status.
            ArrayList pList = GetPlayers(pr.name);
            foreach (Player p in pList) 
            {
              p.admin = false;
            }
            ServerSay("Admin successfully removed.");
            ((HaloServer)ServerManager.ServerList[0])._serverExtensions.ReadAdmins();
            return;
          }
        }
        if (keyword == "refresh") 
        {
          ((HaloServer)ServerManager.ServerList[0])._serverExtensions.ReadAdmins();
        }
      }
      catch (Exception ex)
      {
        ServerSay("There was a critical internal error!");
        ServerSay("Command could not be executed.");
        ServerSay(ex.Message);
        return;
      }
    }
    #endregion

    #region "[ Command: !testmem ]"
    public void TESTMEM(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      int playerNum = Convert.ToInt32((string)parameters[0]);
      HaloCoreMemoryInterface core = new HaloCoreMemoryInterface();
      core.ReadPlayerData();
      ServerSay("Player: " + LogfileMonitor.DeUnicode(new string(core.playerData[playerNum].UnicodePlayerName)));
      ServerSay("Datumized Index: " + core.playerData[playerNum].DatumizedPlayerIndex.ToString());
      ServerSay("Machine Index: " + core.playerData[playerNum].MachineIndex.ToString());
      ServerSay("Kills: " + core.playerData[playerNum].Kills.ToString());
      ServerSay("Deaths: " + core.playerData[playerNum].Deaths.ToString());
      ServerSay("Assists: " + core.playerData[playerNum].Assists.ToString());
      ServerSay("Ping: " + core.playerData[playerNum].Ping.ToString());
      ServerSay("Team: " + core.playerData[playerNum].Team.ToString());
      ServerSay("RespawnTime: " + core.playerData[playerNum].RespawnTime.ToString());
      ServerSay("Score: " + core.playerData[playerNum].Score.ToString());
      ServerSay("PowerupTimeLeft: " + core.playerData[playerNum].PowerupTimeLeft.ToString());
      ServerSay("Player[" + playerNum.ToString() + "] X,Y,Z: " + core.playerData[playerNum].X.ToString() + " " +
        core.playerData[playerNum].Y.ToString() + " " + core.playerData[playerNum].Z.ToString());
      ServerSay("Player[playerNum] Predicted X,Y,Z: " + core.playerData[playerNum].PredictedX.ToString() + " " +
        core.playerData[playerNum].PredictedY.ToString() + " " + core.playerData[playerNum].PredictedZ.ToString());
    }
    #endregion

    #region "[ Command: !info ]"
    public void INFO(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      int playerNum = Convert.ToInt32((string)parameters[0]);
      HaloCoreMemoryInterface core = new HaloCoreMemoryInterface();
      core.ReadPlayerData();
      ServerSay("Player: " + LogfileMonitor.DeUnicode(new string(core.playerData[playerNum].UnicodePlayerName)));
      ServerSay("Datumized Index: " + core.playerData[playerNum].DatumizedPlayerIndex.ToString());
      ServerSay("Machine Index: " + core.playerData[playerNum].MachineIndex.ToString());
    }
    #endregion

    #region "[ Command: !loc ]"
    public void LOC(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      int playerNum = Convert.ToInt32((string)parameters[0]);
      HaloCoreMemoryInterface core = new HaloCoreMemoryInterface();
      core.ReadPlayerData();
			
      ServerSay("Player[" + playerNum.ToString() + "] X,Y,Z: " + core.playerData[playerNum].X.ToString() + " " +
        core.playerData[playerNum].Y.ToString() + " " + core.playerData[playerNum].Z.ToString());
      //ServerSay("Player[playerNum] Predicted X,Y,Z: " + core.playerData[playerNum].PredictedX.ToString() + " " +
      //	core.playerData[playerNum].PredictedY.ToString() + " " + core.playerData[playerNum].PredictedZ.ToString());
    }
    #endregion

    #region "[ Command: !kills ]"
    public void KILLS(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      string playerName = (string)parameters[0];
      int killsNumber = Convert.ToInt32((string)parameters[1]);
     
      PlayerArrayList players = GetPlayers(playerName);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      HaloCoreMemoryInterface core = new HaloCoreMemoryInterface();
      core.ReadPlayerData();

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        int playerNum = 0;
        for (int x=0; x<16; x++)
        {
          if (LogfileMonitor.DeUnicode(new string(core.playerData[x].UnicodePlayerName)) == p.name)
          {
            playerNum = x;
            break;
          }
        }
       
        core.playerData[playerNum].Kills = (short)killsNumber;

        core.WritePlayerData();
      
        ServerSay((string)parameters[0] + "'s Kills were set to " + Convert.ToInt32((string)parameters[1]));
      }
    }
    #endregion

    #region "[ Command: !forceteam ]"
    public void FORCETEAM(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      string playerName = (string)parameters[0];
      int teamNumber = Convert.ToInt32((string)parameters[1]);
     
      PlayerArrayList players = GetPlayers(playerName);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      HaloCoreMemoryInterface core = new HaloCoreMemoryInterface();
      core.ReadPlayerData();

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        int playerNum = 0;
        for (int x=0; x<16; x++)
        {
          if (LogfileMonitor.DeUnicode(new string(core.playerData[x].UnicodePlayerName)) == p.name)
          {
            playerNum = x;
            break;
          }
        }
       
        core.playerData[playerNum].Team = (short)teamNumber;

        core.WritePlayerData();
      
        ServerSay("Player location was updated to memory.");
      }
    }
    #endregion
		
    #region "[ Command: !kick ]"
    public void KICK(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      // Get the player name/criteria
      string name = (string)parameters[0];
			
      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        if (p.admin) ServerSay("Cannot kick Admin: " + p.name);
        if (p.bProtected) ServerSay("Cannot kick protected player: " + p.name);
        if ((!p.admin) && (!p.bProtected))
        {
          // Player can be kicked
          ServerSay("Kicking " + p.name);
          _consoleWindow.QueueCommand("sv_kick " + Convert.ToString(p.machine));
        }
      }
    }
    #endregion

    #region "[ Command: !kickidle ]"
    public void KICKIDLE(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      // Get the idle time
      int idleTime = Convert.ToInt32((string)parameters[0]);
			
      // Cycle through results and act accordingly
      int y = 0;
      foreach (Player p in _gameStatus.players)
      {
        int idleSeconds = (int)((TimeSpan)DateTime.Now.Subtract(p.LastActionTime)).TotalSeconds;
        if (idleSeconds > idleTime)
        {
          if (p.admin) ServerSay("Cannot kick Admin: " + p.name);
          if (p.bProtected) ServerSay("Cannot kick protected player: " + p.name);
          if ((!p.admin) && (!p.bProtected))
          {
            // Player can be kicked
            y++;
            ServerSay("Kicking " + p.name);
            _consoleWindow.QueueCommand("sv_kick " + Convert.ToString(p.machine));
          }
        }
      }
      ServerSay("Done: " + Convert.ToString(y) + " player(s) kicked.");
    }
    #endregion

    #region "[ Command: !makeroom ]"
    public void MAKEROOM(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      // Make sure our game is full before we 'make room'
      if (_gameStatus.players.Count < _gameStatus.MaxPlayers)
      {
        ServerSay("Server is not full!");
        return;
      }

      // Find the last place non-protected player and kick them to make room for someone
      Player lowKiller = _gameStatus.players[0];
      foreach (Player p in _gameStatus.players)
      {
        if ((p.kills < lowKiller.kills) && (p.score < 1))
        {
          // Make sure this player isn't an admin
          int i = _gameStatus.players.LocateByName(p.name);
          if ((!(i == -1)) && (!IsAdmin(i))) lowKiller = p;
        }
      }
      ServerSay("We are making room for a registered user of this server.");
      ServerSay("If you are kicked, please rejoin when a spot is available.");
      object[] o = new object[1];
      o[0] = (object)("#" + Convert.ToString(lowKiller.slot));
      KICK(o,playerID);
    }
    #endregion

    #region "[ Command: !ban ]"
    public void BAN(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;

      // Get the player name/criteria
      string name = (string)parameters[0];

      // Setup our default ban length to use if one wasn't specified
      string banLength = "4h";
      if (parameters.Length > 1) banLength = (string)parameters[1];
      // In the future, code should go here to verify the banLength validity

      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }
		
      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        if (p.admin) ServerSay("Cannot ban admin: " + p.name);
        if (p.bProtected) ServerSay("Cannot ban protected player: " + p.name);
        if ((!p.admin) && (!p.bProtected))
        {
          // Player can be banned
          ServerSay("Banning " + p.name + " for " + banLength);

          // Due to a bug in the logfile for hce, we cannot ban by number
          //_consoleWindow.QueueCommand("sv_ban \"" + p.name + "\" " + banLength);
          // Hrmmm... Maybe it's not a bug!!
          _consoleWindow.QueueCommand("sv_ban " + Convert.ToString(p.machine) + " " + banLength);
        }
      }
    }
    #endregion

    #region "[ Command: !banrecent ]"
    public void BANRECENT(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;

      // Get the player name/criteria
      string name = (string)parameters[0];

      // Setup our default ban length to use if one wasn't specified
      string banLength = "4h";
      if (parameters.Length > 1) banLength = (string)parameters[1];
      // In the future, code should go here to verify the banLength validity

      PlayerArrayList players = GetRecentPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }
		
      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        if (p.admin) ServerSay("Cannot ban admin: " + p.name);
        if (p.bProtected) ServerSay("Cannot ban protected player: " + p.name);
        if ((!p.admin) && (!p.bProtected))
        {
          // Player can be banned
          ServerSay("Manually adding player to banlist: " + p.name);
					
          HaloServer hs = (HaloServer)ServerManager.ServerList[0];
          if (hs._serverExtensions.banlist.AddPlayer(p)) 
          {
            ServerSay ("Changes will not take effect until server is restarted.");
            return;
          }
          else 
          {
            ServerSay ("An error has occurred - unable to ban player.");
            return;
          }
        }
      }
    }
    #endregion

    #region "[ Command: !player ]"
    public void PLAYER(object[] parameters, int playerID)
    {

      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      // Get the player name/criteria
      string name = (string)parameters[0];
			
      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        ServerSay(p.name + " - [ " + Convert.ToString(p.slot) + " ]");
      }
    }
    #endregion

    #region "[ Command: !ping ]"
    public void PING(object[] parameters, int playerID)
    {
      return; // This command isn't functional as of yet
      if (!IsAdmin(playerID)) return;
      if (parameters.Length < 1) return;
			
      // Get the player name/criteria
      string name = (string)parameters[0];
			
      PlayerArrayList players = GetPlayers(name);

      // If no players were returned, we're finished here
      if (players.Count < 1)
      {
        ServerSay ("No matching players found.");
        return;
      }

      // Cycle through results and act accordingly
      foreach (Player p in players)
      {
        ServerSay("[PING] " + p.name + " - " + Convert.ToString(p.averagePing) + "ms");
      }
    }
    #endregion

    #region "[ Command: !kickteam ]"
    public void KICKTEAM(object[] parameters, int playerID)
    {
      if (!IsAdmin(playerID)) return;
      int x=0;
      if (parameters.Length < 1) return;
      string team = (string)parameters[0];
			
      switch (team.ToUpper())
      {
        case "RED":
          x = 0;
          break;
        case "BLUE":
          x = 1;
          break;
        default:
          ServerSay("Invalid Team: Choose RED or BLUE");
          return;
      }


      ServerSay("Kicking " + team + " team");
      int y = 0;
      foreach (Player p in _gameStatus.players)
      {
        if (p.team == x)
        {
          // Kick this player
          object[] o = new object[1];
          o[0] = (object)p.name;
          KICK(o,-1);
          y++;
        }
      }
      ServerSay("Done: " + Convert.ToString(y) + " player(s) kicked.");
    }
    #endregion

    #region [ Command: !kickteam ]

    public class KickTeam : ServerCommand
    {
      public override bool AdminRequired() { return true; }

      public override void RunCommand()
      {
        KickTeamParameters parameters = (KickTeamParameters)_parameters;
        int teamIndex = 0;
        switch (parameters.TeamColor.ToUpper())
        {
          case "RED":
            teamIndex = 0;
            break;
          case "BLUE":
            teamIndex = 1;
            break;
          default:
            _consoleWindow.ServerSay("Invalid Team: Choose RED or BLUE");
            return;
        }

        _consoleWindow.ServerSay("Kicking " + parameters.TeamColor + " team");
        int totalPlayersKicked = 0;
        foreach (Player p in _gameStatus.players)
        {
          if (p.team == teamIndex)
          {
            // Kick this player
            KillParameters kp = new KillParameters();
            kp.PlayerName = p.name;
            ExecuteCommand("kick", kp);
            totalPlayersKicked++;
          }
        }
        _consoleWindow.ServerSay("Done: " + totalPlayersKicked.ToString() + " player(s) kicked.");
      }
    }

    public class KickTeamParameters : ServerCommandParameters
    {
      public string TeamColor;
      public override static ServerCommandParameters Convert(object[] parameters)
      {
        if (parameters.Length < 1) throw new IncorrectParametersException();
        KickTeamParameters p = new KickTeamParameters();
        try
        {
          p.TeamColor = (string)parameters[0];
        }
        catch
        {
          throw new IncorrectParametersException();
        }
        return p;
      }
    }
    #endregion
		
		#region "[ Command: !vote ]"
		public void VOTE(object[] parameters, int playerID)
		{
			// Make sure that we have parameters
			if (parameters.Length < 1) return;
			
			// See if this is cancel command
			if (((string)parameters[0]).ToLower() == "cancel") {
				// Is the player an admin?
                if (!IsAdmin(playerID)) return;
				// Is there a vote in progress?
				if (currentVote.status == Vote.VoteStatus.InProgress) {
					ServerSay("Cancelling current vote.");
					currentVote.CancelVote();
					return;
				}
				else {
					ServerSay("There is no vote in progress.");
					return;
				}
			}

			// See if this is an end command
			if (((string)parameters[0]).ToLower() == "end") 
			{
				// Is the player an admin?
				if (!IsAdmin(playerID)) return;
				// Is there a vote in progress?
				if (currentVote.status == Vote.VoteStatus.InProgress) 
				{
					ServerSay("Ending current vote.");
					currentVote.EndVote();
					return;
				}
				else 
				{
					ServerSay("There is no vote in progress.");
					return;
				}
			}


			// Make sure that a vote hasn't already been called
			if ((object)currentVote != null)
			{
				if (currentVote.status == Vote.VoteStatus.InProgress)
				{
					if (allPurposeCounter < 1) ServerSay("A vote is already in progress.");
					allPurposeCounter++; // To keep the server from spamming in response
					return;
				}
			}

			// Make sure that the player hasn't already called a vote this round
			if (_gameStatus.players[playerID].calledVoteThisRound) return;
			
			// Make sure this is a valid vote
			if (!IsAdmin(playerID))
			{
				switch (((string)parameters[0]).ToLower())
				{
					case "ban":
						break;
					case "kick":
						break;
					case "reset":
						break;
					case "map":
						break;
					case "mapnext":
						break;
					default:
						ServerSay("Invalid Vote Command");
						return;
				}
			}
			
			// Now they have, unless they are an admin, in which case they are free to call as many as they like.
			if (!_gameStatus.players[playerID].admin) _gameStatus.players[playerID].calledVoteThisRound = true;

			// Setup a new vote
			Vote v = new Vote(false);
			allPurposeCounter = 0;
			v.lengthInSeconds = 30;
			v.parameters = (string[])parameters;
			v.totalEligibleVoters = _gameStatus.players.Count;
			
			// Automatically add a vote for the vote caller
			v.playerList.Add(_gameStatus.players[playerID]);
			
			// Build the parameters string, if the array exists
			string p = String.Empty;
			if (v.parameters.Length > 0)
			{
				for (int x=0; x<v.parameters.Length; x++)
				{
					p += (string)v.parameters[x] + " ";
				}
			}
			ServerSay("Vote started: " + p);
			ServerSay("Type yes, y, or 1 to vote yes");
			
			// Setup and run the vote thread
			currentVote = v;
			currentVote.StartVote();
			currentVote.VoteEnded += new Vote.VoteEventHandler(VoteEnded);
		}
		#endregion

		#region "[ Command: !yes ]"
		public void YES(object[] parameters, int playerID)
		{
			if ((object)currentVote != null)
			{
				currentVote.playerList.Add(_gameStatus.players[playerID]);
			}
		}
		#endregion

		#region "[ Command: !no ]"
		public void NO(object[] parameters, int playerID)
		{
			if ((object)currentVote != null)
			{
				currentVote.playerList.Remove(_gameStatus.players[playerID]);
			}
		}
		#endregion

		#region "[ Event: MatchEnded ]"
		public void MatchEnded()
		{
			// Reset the mapcycle if this is an out of cycle map
			if (_gameStatus.CycleInterrupted) _gameStatus.CycleInProgress = false;
			if ((!_gameStatus.CycleInterrupted) && (!_gameStatus.CycleInProgress))
			{
				ServerSay("Out of cycle map complete - Resuming map cycle.");
				_gameStatus.CycleInProgress = true; // We are back in the cycle now
			}
			_gameStatus.CycleInterrupted = false;
			
			// Advance to the next mapcycle index
			if (_gameStatus.CycleInProgress)
			{
				_gameStatus.MapCycleIndex++;
				if (_gameStatus.MapCycleIndex > (_gameStatus.mapCycle.Count-1))
				{
					_gameStatus.MapCycleIndex = 0;
				}
				string nextMap = (string)_gameStatus.mapCycle[_gameStatus.MapCycleIndex];
				ServerSay("Next map up: #" + Convert.ToString(_gameStatus.MapCycleIndex) + " - "
					+ nextMap);
			}
			ChangeMap(_gameStatus.MapCycleIndex);

			// Reload the admins list
			((HaloServer)ServerManager.ServerList[0])._serverExtensions.ReadAdmins();
			if (_gameStatus.players.Count < 1) return; // Make sure we have players in the game;

			string catchPhrase = "";
			switch (r.Next(7))
			{
				case 0:
					catchPhrase = "Congrats!";
					break;
				case 1:
					catchPhrase = "Awesome Job!";
					break;
				case 2:
					catchPhrase = "All your base are belong to us!";
					break;
				case 3:
					catchPhrase = "Now thats what I call PWNAGE!";
					break;
				case 4:
					catchPhrase = "Holy Crap!!!";
					break;
				case 5:
					catchPhrase = "Nice!";
					break;
				case 6:
					catchPhrase = "Hook us up with your uber h4X!!";
					break;
				default:
					catchPhrase = "Wow.....";
					break;
			}
				
			// Find the top scorer and capper
			Player topCapper = _gameStatus.players[0];
			Player topKiller = _gameStatus.players[0];
			Player topRatio = _gameStatus.players[0];
			foreach (Player p in _gameStatus.players)
			{
				if (p.kills > topKiller.kills) topKiller = p;
				if (p.score > topCapper.score) topCapper = p;
				if (p.ratio > topRatio.ratio) topRatio = p;
			}
			if ((topCapper == topKiller) && (topCapper.team != -1)
				&& (topCapper.score > 0) && (topCapper.kills > 0))
			{
				ServerSay(topCapper.name + " pwned with the most kills AND flag caps!");
				ServerSay(catchPhrase);
			}
			else
			{
				if ((topCapper.team != -1) && (topCapper.score > 0))
				{
					ServerSay(topKiller.name + " is the top killer for this round, and ");
					ServerSay(topCapper.name + " had the most caps.  " + catchPhrase);
				}
				else
				{
					if (topKiller.kills > 0) ServerSay("Top killer for this round is " + topKiller.name + "!");
				}
			}
			ServerSay(topRatio.name + " had the best K/D ratio - " + topRatio.RatioFormatted);
		}
		#endregion
		
		#region "[ Command: !screen ]"
		public void SCREEN(object[] parameters, int playerID)
		{
			if (!IsAdmin(playerID)) return;
			if (parameters.Length < 1) return;
			
			// Get the player name/criteria
			string name = (string)parameters[0];
			
			PlayerArrayList players = GetPlayers(name);

			// If no players were returned, we're finished here
			if (players.Count < 1)
			{
				ServerSay ("No matching players found.");
				return;
			}

			// Cycle through results and act accordingly
			foreach (Player p in players)
			{
				int hWnd = Api.FindWindow(null, "Halo");
				if (hWnd < 1) {
					ServerSay("No instances of Halo were found!");
					return;
				}
				Bitmap b = ScreenCapture.GrabWindow(hWnd);
				b.Save("c:\\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
				ServerSay("Image Saved - hWnd: " + Convert.ToString(hWnd));
			}
		}
		#endregion

		// Go to a specified map in the index
		public void ChangeMap(int mapIndex)
		{
			string nextMap = (string)_gameStatus.mapCycle[_gameStatus.MapCycleIndex];
			string map = "";
			string[] parameters = StringFunctions.ParseCommand(nextMap, out map);
			if (parameters.Length > 1)
			{
				string c = "sv_map " + "\"" + map + "\" \"" + parameters[1] + "\"";
				Trace.WriteLine(c);
				_consoleWindow.QueueCommand(c);
			}
			else
			{
				ServerSay ("There was an error in the mapcycle!");
				ServerSay (nextMap + " is not valid - Current map will repeat.");
			}
		}

    #region [ Command: !kill ]

    public class KillParameters : ServerCommandParameters
    {
      public string PlayerName;
      public override static ServerCommandParameters Convert(object[] parameters)
      {
        if (parameters.Length < 1) throw new IncorrectParametersException();
        KillParameters p = new KillParameters();
        try
        {
          p.PlayerName = (string)parameters[0];
        }
        catch
        {
          throw new IncorrectParametersException();
        }
        return p;
      }
    }

    public class Kill : PlayerTargetedServerCommand
    {
      public override bool AdminRequired() { return true; }
      public override void RunCommand()
      {
        KillParameters parameters = (KillParameters)_parameters;
        this.RunCommandOnPlayerList(GetPlayers(parameters.PlayerName));
      }
      protected override void CommandMethod(HaloPlayer p, int corePlayerIndex, int playerObjectCollectionIndex)
      {
        string command = "if (!= (unit_get_health (unit (list_get (players) " + playerObjectCollectionIndex.ToString() + "))) -1) (unit_kill (unit (list_get (players) " + playerObjectCollectionIndex.ToString() + "))) (sv_say \"Player Not Found\")";
        _consoleWindow.QueueCommand(command);            
        _consoleWindow.ServerSay(p.name + " was pwned by the server.");
      }
    }

    #endregion

    #region Server Command Base Classes

    public abstract class ServerCommandParameters
    {
      public static abstract ServerCommandParameters Convert(object[] parameters);

      public class IncorrectParametersException : Exception {}
    }

    /// <summary>
    /// Provides a base class for implementing server commands.
    /// </summary>
    public abstract class ServerCommand
    {
      protected ServerCommandParameters _parameters;
      protected int _playerID;
      protected bool _admin;
      protected GameStatus _gameStatus;
      protected bool _commandReady;
      protected HaloCoreMemoryInterface _core;
      protected ConsoleWindow _consoleWindow;

      public abstract bool AdminRequired();
      
      public ServerCommand(ServerCommandParameters parameters, int playerID, ref GameStatus gameStatus, ref ConsoleWindow consoleWindow)
      {
         _parameters = parameters;
        _gameStatus = gameStatus;
        _consoleWindow = consoleWindow;
        _core = new HaloCoreMemoryInterface();
        _playerID = playerID;
        _commandReady = false;

        if (IsAdmin(_playerID) == AdminRequired())
        {
          _commandReady = true;
        }
      }

      public abstract void RunCommand();

        /// <summary>
      /// Returns a boolean indicating if the specified player is an admin
      /// </summary>
      protected bool IsAdmin (int playerID)
      {
        bool isAdmin = false;
        if (playerID == -1) isAdmin = true; // We can't check the array if the index is -1
        if (!isAdmin)
        {
          if (_gameStatus.players[playerID].admin) isAdmin = true;
        }
        return isAdmin;
      }
      // Returns a boolean indicating if the specified player is on the Protected list
      public bool IsProtected (int playerID)
      {
        bool isProtected = false;
        if (playerID == -1)
        {
          isProtected = true;
        }
        if (!isProtected)
        {
          if (_gameStatus.players[playerID].admin) isProtected = true;
        }
        return isProtected;
      }
    }

    /// <summary>
    /// Extends the Server command class to simplify commands that are targeted at players.
    /// </summary>
    public abstract class PlayerTargetedServerCommand : ServerCommand
	  {
      protected void RunCommandOnPlayer(Player player)
      {
        _core.ReadPlayerData();
        
        // Locate the target player in the core.
        int corePlayerIndex = 0;
        for (int x=0; x<16; x++)
        {
          if (player.name == _core.playerData[x].PlayerName)
          {
            corePlayerIndex = x;
            break;
          }
        }
        
        // Get the index to use for scripting
        int playerObjectCollectionIndex = 0;
        HaloCoreMemoryInterface.PlayerData[] sortedPlayers = GetScriptPlayersFromMemory();
        for (int x=0; x<16; x++)
        {
          if (player.name == sortedPlayers[x].PlayerName)
          {
            playerObjectCollectionIndex = x;
            break;
          }
        }


        CommandMethod(player, corePlayerIndex, playerObjectCollectionIndex);
      
        _core.WritePlayerData();
      }
    
      protected abstract void CommandMethod(Player p, int corePlayerIndex, int playerObjectCollectionIndex);
      
      protected void RunCommandOnPlayerList(PlayerArrayList list)
      {
        foreach(Player p in list)
        {
          RunCommandOnPlayer(p);
        }
      }
    
      #region Player Sorting for Script Position
      protected class PlayerDataComparer : IComparer
      {
        public int Compare(object x, object y)
        {
          if ((!(x is HaloCoreMemoryInterface.PlayerData)) ||
            (!(y is HaloCoreMemoryInterface.PlayerData)))
            throw new Exception("Incompatible type.");
          HaloCoreMemoryInterface.PlayerData px = (HaloCoreMemoryInterface.PlayerData)x;
          HaloCoreMemoryInterface.PlayerData py = (HaloCoreMemoryInterface.PlayerData)y;
          return px.DatumizedPlayerIndex.CompareTo(py.DatumizedPlayerIndex);
        }
      }

      protected HaloCoreMemoryInterface.PlayerData[] GetScriptPlayersFromMemory()
      {
        _core.ReadPlayerData();
        Array.Sort(_core.playerData, new PlayerDataComparer());
        return _core.playerData;
      }
      #endregion	    

      /// <summary>
      /// Returns a list of players matching the specified criteria.
      /// </summary>
      /// <param name="criteria">Criteria, either a number '#1' or a player wildcard '*test'</param>
      /// <returns></returns>
      public PlayerArrayList GetPlayers(string criteria)
      {
        PlayerArrayList results = new PlayerArrayList();
			
        // Make sure we have criteria
        if (((object)criteria == null) || (criteria == "")) return results;

        // See is this is a ban by number
        if (criteria.StartsWith("#"))
        {
          criteria = criteria.Replace("#","");
          int machine = Convert.ToInt32(criteria);
          foreach (Player p in _gameStatus.players)
          {
            if (p.machine == machine) results.Add(p);
					
          }
          return results;
        }

        // Find all matching players
        foreach (Player p in _gameStatus.players)
        {
          string baseName = p.name.ToUpper();
          criteria = criteria.ToUpper();
				
          // If the strings are equal, return the value
          if ((baseName == criteria) || (StringFunctions.Like(baseName, criteria, true)))
            results.Add(p);
        }
        return results;
      }
	  }

    #endregion

    public int[] GetPlayerCollection()
    {
      return new int[1];  
    }

	public PlayerArrayList GetRecentPlayersByKeyHash(string criteria)
		{
			PlayerArrayList results = new PlayerArrayList();
			
			// Make sure we have criteria
			if (((object)criteria == null) || (criteria == "")) return results;

			// Find all matching players
			foreach (Player p in _gameStatus.RecentPlayers)
			{
				string keyHash = p.keyHash;
						
				// If the strings are equal, return the value
				if (keyHash == criteria) results.Add(p);
			}
			return results;
		}


		#region "[ Class: Vote ]"
		public class Vote
		{
			// Setup events
			public class VoteEventArgs : System.EventArgs 
			{
				public Vote vote;
				public VoteEventArgs(Vote v)
				{
					vote = v;
				}
			}
            public event VoteEventHandler VoteEnded;
			public delegate void VoteEventHandler(object sender, VoteEventArgs e);
			protected virtual void OnVoteEnded(VoteEventArgs e) 
			{
				if (VoteEnded != null) VoteEnded(this, e);
			}

			public string[] parameters;
			public PlayerArrayList playerList;
			public string parameter;
			public int totalEligibleVoters;
			public int totalVotes;
			public int lengthInSeconds;
			public System.Timers.Timer voteTimer;
			public VoteStatus status;
			public Vote(bool autoStart)
			{
				totalVotes = 0;
				parameter = "";
				playerList = new PlayerArrayList();
			}
			public void StartVote()
			{
				voteTimer = new System.Timers.Timer();
				voteTimer.Interval = lengthInSeconds * 1000; // milliseconds
				voteTimer.Elapsed += new System.Timers.ElapsedEventHandler(EndVoteHandler);
				voteTimer.Start();
				status = VoteStatus.InProgress;
			}
			public void EndVote()
			{
				voteTimer.Stop();
				voteTimer = null;

				totalVotes = playerList.Count;
				if (totalVotes >= ((int)((float)totalEligibleVoters / 2) + 1))
				{
					status = VoteStatus.Succeeded;
				}
				else
				{
					status = VoteStatus.Failed;
				}
				OnVoteEnded(new VoteEventArgs(this));
			}
			public void CancelVote()
			{
				voteTimer.Stop();
				voteTimer = null;
				status = VoteStatus.Idle;
			}
			public void EndVoteHandler(object e, System.Timers.ElapsedEventArgs timerEventArgs)
			{
				EndVote();
			}
			public enum VoteStatus { InProgress, Succeeded, Failed, Idle }
		}
		#endregion
		
		#region "Event Handlers"
		
		// Event handler for OnTimerElapsed
		private void t_Elapsed(object o, System.Timers.ElapsedEventArgs e)
		{
			WarnIdlePlayers(); 
			KickIdlePlayers();
			Advertise();
			KickSpammers();
		}
		
		// Event handler for OnVoteEnded
		private void VoteEnded(object sender, Vote.VoteEventArgs e)
		{
			switch (e.vote.status)
			{
				case Vote.VoteStatus.Failed:
					ServerSay("Vote failed (" + e.vote.totalVotes + " out of " + e.vote.totalEligibleVoters + ")");
					ServerSay(Convert.ToString((int)((float)e.vote.totalEligibleVoters / 2) + 1) + " votes were required.");
					break;
				case Vote.VoteStatus.Succeeded:
					// Build the parameters string, if the array exists
					string p = String.Empty;
					if (e.vote.parameters.Length > 0)
					{
						for (int x=0; x<e.vote.parameters.Length; x++)
						{
							p += (string)e.vote.parameters[x] + " ";
						}
					}
					ServerSay("Vote Succeeded - " + p);
					ServerSay("(" + e.vote.totalVotes + " out of " + e.vote.totalEligibleVoters + ")");
					// The people have spoken!  Invoke the desired command
					if (e.vote.parameters.Length == 1)
					{
						ExecuteLocal(e.vote.parameters[0], null, -1);
					}
					else
					{
						string[] s = Interpreter.GetElements(e.vote.parameters, 1);
						ExecuteLocal(e.vote.parameters[0], s, -1);
					}
					break;
			}
		}
		#endregion

		#region "Timer based scripts"
		public void KickIdlePlayers()
		{
			//if (_gameStatus.players.Count < 12) return;  // Don't kick idle players unless we have a few people
			foreach (Player p in _gameStatus.players)
			{
				int idleSeconds = (int)((TimeSpan)DateTime.Now.Subtract(p.LastActionTime)).TotalSeconds;
				if (idleSeconds > 900)
				{
					if (!p.idle)
					{
						// Kick this player
						p.idle = true;
						ServerSay("Idle player detected: " + p.name);
						object[] o = new object[1];
						o[0] = (object)p.name;
						KICK(o,-1);
					}
				}
			}
		}
		public void WarnIdlePlayers()
		{
			if (_gameStatus.players.Count < 8) return;  // Don't kick idle players unless we have a few people
			foreach (Player p in _gameStatus.players)
			{
				int idleSeconds = (int)((TimeSpan)DateTime.Now.Subtract(p.LastActionTime)).TotalSeconds;
				if (idleSeconds > 780)
				{
					if (!p.warned_idle_first)
					{
						// Warn this player
						p.warned_idle_first = true;
						ServerSay(p.name + ": will be kicked in 2 minutes for inactivity.");
					}
					if (idleSeconds > 870)
					{
						if ((p.warned_idle_first) && (!p.warned_idle_second))
						{
							// Warn this player
							p.warned_idle_second = true;
							ServerSay(p.name + ": will be kicked in 30 sec. for inactivity.");
						}
					}
				}
			}
		}
		private int m_advertiseCount=0;
		public void Advertise()
		{
			// This script is called once per second
			// Until we have scripts running on their own timers, this hack will be
			// used to display the advertisement once every 60 seconds.
			m_advertiseCount++;

			if (m_advertiseCount >= 1200)
			{
				m_advertiseCount = 0;
				ServerSay("This server is running HST Server Extensions v0.5a");
			}
			if ((m_advertiseCount == 300) || (m_advertiseCount == 600) || (m_advertiseCount == 900) || (m_advertiseCount == 1200))
			{
				int i = _gameStatus.MapCycleIndex + 1;
				if (i > (_gameStatus.mapCycle.Count-1)) i = 0;
				ServerSay("Next map up: " + (string)_gameStatus.mapCycle[i]);	
			}
		}
		public void KickSpammers()
		{
			foreach (Player p in _gameStatus.players)
			{
				if (p.talkingSpree > 4)
				{
					p.talkingSpree = -200;
					ServerSay("** OMFG SPAMMER GET OUT KTHX!!! ***");
					object[] o = new object[1];
					o[0] = (object)p.name;
					KICK(o,-1);
				}
			}
		}
		#endregion

	}
}
