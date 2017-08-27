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
	/// Defines game types for the <see cref="GameStat.ServerInfo.Query"/> method.
	/// </summary>
	public enum GameType : ushort {
		/// <summary>
		/// QuakeWorld
		/// </summary>
		QuakeWorld=0,
		/// <summary>
		/// Quake 2
		/// </summary>
		Quake2,
		/// <summary>
		/// Quake III Arena
		/// </summary>
		Quake3,
		/// <summary>
		/// Doom 3
		/// </summary>
		Doom3,
		/// <summary>
		/// Half-Life
		/// </summary>
		HalfLife,
		/// <summary>
		/// UnrealEngine 2 (Generic Protocol)
		/// </summary>
		UnrealEngine2,
		/// <summary>
		/// GameSpy 2 (Generic Protocol)
		/// </summary>
		GameSpy2,
		/// <summary>
		/// Battlefield Vietnam
		/// </summary>
		BattlefieldVietnam,
		/// <summary>
		/// Painkiller
		/// </summary>
		Painkiller,
		/// <summary>
		/// Unreal Tournament 2003
		/// </summary>
		UnrealTournament2003,
		/// <summary>
		/// Unreal Tounrmanet 2004
		/// </summary>
		UnrealTournament2004,
		/// <summary>
		/// Unreal 2 XMP
		/// </summary>
		Unreal2XMP,
		/// <summary>
		/// Soldier of Fortune 2
		/// </summary>
		SoldierOfFortune2,
		/// <summary>
		/// Return to Castle Wolfenstein
		/// </summary>
		ReturnToCastleWolfenstein,
		/// <summary>
		/// Wolfenstein: Enemy Territory
		/// </summary>
		WolfensteinEnemyTerritory,
		/// <summary>
		/// Counter-Strike
		/// </summary>
		CounterStrike,
		/// <summary>
		/// Counter-Strike: Condition Zero
		/// </summary>
		CounterStrikeConditionZero,
		/// <summary>
		/// Call of Duty
		/// </summary>
		CallOfDuty,
		/// <summary>
		/// Star Wars Jedi Knight: Jedi Academy
		/// </summary>
		JediKnightJediAcademy,
		/// <summary>
		/// Medal of Honor: Allied Assault
		/// </summary>
		MedalOfHonorAlliedAssault,
		/// <summary>
		/// Medal of Honor: Breakthrough
		/// </summary>
		MedalOfHonorBreakthrough,
		/// <summary>
		/// Medal of Honor: Spearhead
		/// </summary>
		MedalOfHonorSpearhead,
		/// <summary>
		/// Day of Defeat
		/// </summary>
		DayOfDefeat,
		/// <summary>
		/// Soldier of Fortune
		/// </summary>
		SoldierOfFortune,
		/// <summary>
		/// GameSpy (Generic Protocol)
		/// </summary>
		GameSpy,
		/// <summary>
		/// Unreal Tournament
		/// </summary>
		UnrealTournament,
		/// <summary>
		/// Star Wars Jedi Knight 2: Jedi Outcast
		/// </summary>
		JediKnightJediOutcast,
		/// <summary>
		/// Descent 3
		/// </summary>
		Descent3,
		/// <summary>
		/// America's Army
		/// </summary>
		AmericasArmy,
		/// <summary>
		/// Battlefield 1942
		/// </summary>
		Battlefield1942,
		/// <summary>
		/// Halo: Combat Evolved
		/// </summary>
		HaloCombatEvolved,
		/// <summary>
		/// Postal 2
		/// </summary>
		Postal2,
		/// <summary>
		/// Star Trek: Voyager - Elite Force
		/// </summary>
		StarTrekVoyagerEliteForce,
		/// <summary>
		/// Star Trek: Voyager - Elite Force 2
		/// </summary>
		StarTrekVoyagerEliteForce2,
		/// <summary>
		/// Deus Ex
		/// </summary>
		DeusEx,
		/// <summary>
		/// IL-2 Sturmovik
		/// </summary>
		IL2Sturmovik,
		/// <summary>
		/// IL-2 Sturmovik: Forgotten Battles
		/// </summary>
		IL2SturmovikForgottenBattles,
		/// <summary>
		/// Source Engine (Generic Protocol)
		/// </summary>
		Source,
		/// <summary>
		/// Half-Life 2
		/// </summary>
		HalfLife2,
		/// <summary>
		/// Counter-Strike: Source
		/// </summary>
		CounterStrikeSource,
		/// <summary>
		/// All-Seeing Eye (Generic Protocol)
		/// </summary>
		AllSeeingEye,
		/// <summary>
		/// Multi-Theft Auto
		/// </summary>
		MultiTheftAuto,
		/// <summary>
		/// Multi-Theft Auto: Vice City
		/// </summary>
		MultiTheftAutoViceCity,
		/// <summary>
		/// Heretic 2
		/// </summary>
		Heretic2,
		/// <summary>
		/// IGI-2: Covert Strike
		/// </summary>
		IGI2CovertStrike,
		/// <summary>
		/// Gore
		/// </summary>
		Gore,
		/// <summary>
		/// Vietcong
		/// </summary>
		Vietcong,
		/// <summary>
		/// Neverwinter Nights
		/// </summary>
		NeverwinterNights,
		/// <summary>
		/// Serious Sam
		/// </summary>
		SeriousSam,
		/// <summary>
		/// Serious Sam: The Second Encounter
		/// </summary>
		SeriousSamTheSecondEncounter,
		/// <summary>
		/// Age of Empires 2
		/// </summary>
		AgeOfEmpires2,
		/// <summary>
		/// Age of Empires 2: The Conquerors
		/// </summary>
		AgeOfEmpires2TheConquerors,
		/// <summary>
		/// Aliens vs. Predators 2
		/// </summary>
		AliensVsPredators2,
		/// <summary>
		/// Chaser
		/// </summary>
		Chaser,
		/// <summary>
		/// Chrome
		/// </summary>
		Chrome,
		/// <summary>
		/// No One Lives Forever
		/// </summary>
		NoOneLivesForever,
		/// <summary>
		/// No One Lives Forever 2
		/// </summary>
		NoOneLivesForever2,
		/// <summary>
		/// Shogo
		/// </summary>
		Shogo,
		/// <summary>
		/// Soldat
		/// </summary>
		Soldat,
		/// <summary>
		/// Codename: Eagle
		/// </summary>
		CodenameEagle,
		/// <summary>
		/// Devastation
		/// </summary>
		Devastation,
		/// <summary>
		/// Giants: Citizen Kabuto
		/// </summary>
		GiantsCitizenKabuto,
		/// <summary>
		/// Global Operations
		/// </summary>
		GlobalOperations,
		/// <summary>
		/// Nerf ArenaBlast
		/// </summary>
		NerfArenaBlast,
		/// <summary>
		/// Purge Jihad
		/// </summary>
		PurgeJihad,
		/// <summary>
		/// RalliSport Challenge
		/// </summary>
		RalliSportChallenge,
		/// <summary>
		/// Rally Masters
		/// </summary>
		RallyMasters,
		/// <summary>
		/// Command and Conquer: Renegade
		/// </summary>
		CommandAndConquerRenegade,
		/// <summary>
		/// Rune
		/// </summary>
		Rune,
		/// <summary>
		/// Sin
		/// </summary>
		Sin,
		/// <summary>
		/// Tactical Ops
		/// </summary>
		TacticalOps,
		/// <summary>
		/// Universal Combat
		/// </summary>
		UniversalCombat,
		/// <summary>
		/// Unreal
		/// </summary>
		Unreal,
		/// <summary>
		/// Wheel of Time
		/// </summary>
		WheelOfTime,
		/// <summary>
		/// Xpand Rally
		/// </summary>
		XpandRally,
		/// <summary>
		/// Deadly Dozen: Pacific Theater
		/// </summary>
		DeadlyDozenPacificTheater,
		/// <summary>
		/// Dirt Track Racing 2
		/// </summary>
		DirtTrackRacing2,
		/// <summary>
		/// Drakan: Order of the Flame
		/// </summary>
		DrakanOrderOfTheFlame,
		/// <summary>
		/// F1 2002
		/// </summary>
		F12002,
		/// <summary>
		/// Gunman Chronicles
		/// </summary>
		GunmanChronicles,
		/// <summary>
		/// Iron Storm
		/// </summary>
		IronStorm,
		/// <summary>
		/// James Bond: Nightfire
		/// </summary>
		JamesBondNightfire,
		/// <summary>
		/// Kingpin
		/// </summary>
		Kingpin,
		/// <summary>
		/// Need for Speed: Hot Pursuit 2
		/// </summary>
		NeedForSpeedHotPursuit2,
		/// <summary>
		/// Operation Flashpoint
		/// </summary>
		OperationFlashpoint,
		/// <summary>
		/// Redline
		/// </summary>
		Redline,
		/// <summary>
		/// Terminus
		/// </summary>
		Terminus,
		/// <summary>
		/// Turok 2
		/// </summary>
		Turok2,
		/// <summary>
		/// Tron 2.0
		/// </summary>
		Tron2,
		/// <summary>
		/// Starsiege: Tribes
		/// </summary>
		StarsiegeTribes,
		/// <summary>
		/// Tribes 2
		/// </summary>
		Tribes2,
		/// <summary>
		/// Tony Hawk's Pro Skater 3
		/// </summary>
		TonyHawksProSkater3,
		/// <summary>
		/// Tony Hawk's Pro Skater 4
		/// </summary>
		TonyHawksProSkater4,
		/// <summary>
		/// V8 Supercar Challenge
		/// </summary>
		V8SupercarChallenge,
		/// <summary>
		/// Team Factor
		/// </summary>
		TeamFactor,
		/// <summary>
		/// Rainbow Six
		/// </summary>
		RainbowSix,
		/// <summary>
		/// Rainbow Six: Rogue Spear
		/// </summary>
		RainbowSixRogueSpear,
		/// <summary>
		/// Nitro Family
		/// </summary>
		NitroFamily,
		/// <summary>
		/// Far Cry
		/// </summary>
		FarCry,
		/// <summary>
		/// Rise of Nations
		/// </summary>
		RiseOfNations,
		/// <summary>
		/// Contract J.A.C.K.
		/// </summary>
		ContractJACK,
		/// <summary>
		/// Homeworld 2
		/// </summary>
		Homeworld2,
		/// <summary>
		/// Daikatana
		/// </summary>
		Daikatana,
		/// <summary>
		/// Savage
		/// </summary>
		Savage,
		/// <summary>
		/// Ghost Recon
		/// </summary>
		GhostRecon,
		/// <summary>
		/// Breed
		/// </summary>
		Breed,
		/// <summary>
		/// Medal of Honor: Pacific Assault
		/// </summary>
		MedalOfHonorPacificAssault,
		/// <summary>
		/// Operation Flashpoint: Resistance
		/// </summary>
		OperationFlashPointResistance,
		/// <summary>
		/// Sum of all Fears
		/// </summary>
		SumOfAllFears,
		/// <summary>
		/// Star Trek: Bridge Commander
		/// </summary>
		StarTrekBridgeCommander,
		/// <summary>
		/// Call of Duty: United Offensive
		/// </summary>
		CallOfDutyUnitedOffensive,
		/// <summary>
		/// Star Wars: Battlefront
		/// </summary>
		StarWarsBattlefront,
		/// <summary>
		/// Tribes: Vengeance
		/// </summary>
		TribesVengeance,
		/// <summary>
		/// Tony Hawk's Underground 2
		/// </summary>
		TonyHawksUnderground2
	}
}
