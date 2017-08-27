using System;

namespace Core.Interfaces
{
	/// <summary>
	/// Summary description for CommandInterface.
	/// </summary>
	public class CommandInterface
	{
		public delegate void CommandDelegate(int player_id, params object[] args);

		/// <summary>
		/// Holds info about a single command
		/// </summary>
		public class CommandInfo
		{
			#region Name
			private string name;
			public string Name
			{
				get { return name; }
			}
			#endregion

			#region Command
			private CommandDelegate command;
			public CommandDelegate Command
			{
				get { return command; }
			}
			#endregion

			#region Flags
			private uint flags;
			public uint Flags
			{
				get { return flags; }
			}
			#endregion

			public CommandInfo(string _name, CommandDelegate _command, uint _flags)
			{
				name = _name;
				command = _command;
				flags = _flags;
			}
		};

		/// <summary>
		/// Commands executeable via what the player says in the server log
		/// </summary>
		public static CommandInfo[] PlayerCommands = new CommandInfo[] {null};
	};
}