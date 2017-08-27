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

namespace GameStat {
	/// <summary>
	/// Defines master types for the <see cref="GameStat.ServerInfo.QueryMaster"/> method.
	/// </summary>
	public enum MasterType : ushort {
		/// <summary>
		/// Doom 3
		/// </summary>
		Doom3=0,
		/// <summary>
		/// Quake III Arena
		/// </summary>
		Quake3,
		/// <summary>
		/// Quake 2
		/// </summary>
		Quake2,
		/// <summary>
		/// Return to Castle Wolfenstein
		/// </summary>
		ReturnToCastleWolfenstein,
		/// <summary>
		/// Wolfenstein: Enemy Territory
		/// </summary>
		WolfensteinEnemyTerritory,
		/// <summary>
		/// Star Wars Jedi Knight 2: Jedi Outcast
		/// </summary>
		JediKnightJediOutcast,
		/// <summary>
		/// Half-Life
		/// </summary>
		HalfLife,
		/// <summary>
		/// Counter-Strike
		/// </summary>
		CounterStrike,
		/// <summary>
		/// Counter-Strike: Condition Zero
		/// </summary>
		CounterStrikeConditionZero,
		/// <summary>
		/// Counter-Strike: Source
		/// </summary>
		CounterStrikeSource,
		/// <summary>
		/// Day of Defeat
		/// </summary>
		DayOfDefeat,
		/// <summary>
		/// Soldier of Fortune 2
		/// </summary>
		SoldierOfFortune2,
		/// <summary>
		/// Star Wars Jedi Knight: Jedi Academy
		/// </summary>
		JediKnightJediAcademy,
		/// <summary>
		/// Star Trek: Voyager - Elite Force
		/// </summary>
		StarTrekVoyagerEliteForce,
		/// <summary>
		/// Star Trek: Voyager - Elite Force 2
		/// </summary>
		StarTrekVoyagerEliteForce2
	}
}
