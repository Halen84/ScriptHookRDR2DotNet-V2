using System;

namespace RDR2
{
	[Flags]
	public enum eEnterExitVehicleFlags
	{
		None = 0,
		ResumeIfInterrupted = (1 << 0),
		WarpEntryPoint = (1 << 1),
		InterruptDuringGetIn = (1 << 2),
		JackAnyone = (1 << 3),
		WarpPed = (1 << 4),
		InterruptDuringGetOut = (1 << 5),
		DontWaitForVehicleToStop = (1 << 6),
		AllowExteriorEntry = (1 << 7),
		DontCloseDoor = (1 << 8),
		WarpIfDoorIsBlocked = (1 << 9),
		EnterUsingNavmesh = (1 << 10),
		JumpOut = (1 << 12),
		PreferDismountSideWithFewerPeds = (1 << 14),
		DontDefaultWarpIfDoorBlocked = (1 << 16),
		UseLeftEntry = (1 << 17),
		UseRightEntry = (1 << 18),
		JustPullPedOut = (1 << 19),
		BlockSeatShuffling = (1 << 20),
		WarpIfShuffleLinkIsBlocked = (1 << 22),
		DontJackAnyone = (1 << 23),
		WaitForEntryPointToBeClear = (1 << 24),
		UseHitchDismountVariant = (1 << 25),
		ExitSeatOnToVehicle = (1 << 26),
		AllowScriptedTaskAbort = (1 << 27),
		WillShootAtTargetPeds = (1 << 28),
		InterruptAlways = (1 << 29),
		IgnoreEntryFromClosestPoint = (1 << 30),
		AllowJackPlayerPedOnly = (1 << 31),
	}
}
