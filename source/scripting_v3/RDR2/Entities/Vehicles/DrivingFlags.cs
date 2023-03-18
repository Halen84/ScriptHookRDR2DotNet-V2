using System;

namespace RDR2
{
	[Flags]
	public enum eDrivingFlags
	{
		None = 0,

		DF_StopForCars = 1,
		DF_StopForPeds = 2,
		DF_SwerveAroundAllCars = 4,
		DF_SteerAroundStationaryCars = 8,
		DF_SteerAroundPeds = 16,
		DF_SteerAroundObjects = 32,
		DF_DontSteerAroundPlayerPed = 64,
		DF_StopAtLights = 128,
		DF_GoOffRoadWhenAvoiding = 256,
		DF_DriveIntoOncomingTraffic = 512,
		DF_DriveInReverse = 1024,
		DF_UseWanderFallbackInsteadOfStraightLine = 2048,
		DF_AvoidRestrictedAreas = 4096,
		DF_PreventBackgroundPathfinding = 8192,
		DF_AdjustCruiseSpeedBasedOnRoadSpeed = 16384,
		DF_UseShortCutLinks = 262144,
		DF_ChangeLanesAroundObstructions = 524288,
		DF_UseSwitchedOffNodes = 2097152,
		DF_PreferNavmeshRoute = 4194304,
		DF_PlaneTaxiMode = 8388608,
		DF_ForceStraightLine = 16777216,
		DF_UseStringPullingAtJunctions = 33554432,
		DF_AvoidHighways = 536870912,
		DF_ForceJoinInRoadDirection = 1073741824,

		DRIVINGMODE_STOPFORCARS = DF_StopForCars | DF_StopForPeds | DF_SteerAroundObjects | DF_SteerAroundStationaryCars | DF_StopAtLights | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
		DRIVINGMODE_STOPFORCARS_STRICT = DF_StopForCars | DF_StopForPeds | DF_StopAtLights | DF_UseShortCutLinks,
		DRIVINGMODE_AVOIDCARS = DF_StopForCars | DF_SwerveAroundAllCars | DF_SteerAroundObjects | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
		DRIVINGMODE_AVOIDCARS_RECKLESS = DF_SwerveAroundAllCars | DF_SteerAroundObjects | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
		DRIVINGMODE_PLOUGHTHROUGH = DF_UseShortCutLinks,
		DRIVINGMODE_STOPFORCARS_IGNORELIGHTS = DF_StopForCars | DF_StopForPeds | DF_SteerAroundStationaryCars | DF_SteerAroundObjects | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
		DRIVINGMODE_AVOIDCARS_OBEYLIGHTS = DF_StopForCars | DF_SwerveAroundAllCars | DF_SteerAroundObjects | DF_StopAtLights | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
		DRIVINGMODE_AVOIDCARS_STOPFORPEDS_OBEYLIGHTS = DF_StopForCars | DF_StopForPeds | DF_SwerveAroundAllCars | DF_SteerAroundObjects | DF_StopAtLights | DF_UseShortCutLinks | DF_ChangeLanesAroundObstructions,
	}
}
