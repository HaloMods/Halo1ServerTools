using System;
using System.IO;

namespace Core.Halo
{
	/// <summary>
	/// Game Type Definition
	/// </summary>
	public class GameTypeDefinition
	{
		public enum Type : byte
		{
		};

		#region Name
		private string name;
		public string Name
		{
			get	{ return name; }
		}
		#endregion

		#region GameType
		private Type gameType;
		public Type GameType
		{
			get { return gameType; }
		}
		#endregion
		
		public void Load(BinaryReader br)
		{
			byte[] _name = br.ReadBytes(0x30);
			for(int x = 0; x < 0x30; x += 2)
				if( _name[x] != 0 ) name += (char)_name[x];
			gameType = (Type)br.ReadInt32();
			br.ReadInt32();
			br.ReadInt32(); // parameters
			br.ReadInt32(); // objectives indicator
			br.ReadInt32(); // odd man out
			br.ReadInt32(); // respawn time growth
			br.ReadInt32(); // respawn time
			br.ReadInt32(); // suicide penalty
			br.ReadInt32(); // # of lives
			br.ReadSingle(); // ?
			br.ReadInt32(); // score limit
			br.ReadInt32(); // weapon set
			br.ReadInt32(); // red vehicles
			br.ReadInt32(); // blue vehicles
			br.ReadInt32(); // vehicle respawn time ( / 30 )
			br.ReadInt32(); // friendly fire
			br.ReadInt32(); // friendly fire penalty
			br.ReadInt32(); // tream balance
			br.ReadInt32(); // time limit ( / 30 )

			br.ReadBoolean(); // flag must reset
			br.ReadBoolean(); // flag at home to score
			br.ReadInt32(); // single flag
		}
	};
}