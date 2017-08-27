using System;

namespace HaloServerTools
{
	/// <summary>
	/// Summary description for SpamFilter.
	/// </summary>
	public class SpamFilter
	{
		private SpamTracker[] _trackers;

    public class SpamTracker
    {
      private GameState.HaloPlayer _player;
      private SpamInfo[] _info;
      public SpamTracker(GameState.HaloPlayer player)
      {
        _player = player;
        _info = new SpamInfo[10]; // We will store 10 samples to compare against;
      }
    }
    public class SpamInfo
    {
      private DateTime _timeStamp;
      private string _text;
    }
    
    public SpamFilter(GameState.PlayerArrayList players)
    {
      _trackers = new SpamTracker[players.Count];
      for (int x=0; x<players.Count; x++)
      {
        _trackers[x] = new SpamTracker(players[x]);
      } 
      HaloServer server = (HaloServer)ServerManager.ServerList[0];
      server._serverExtensions._logInterpreter
		}
	}
}
