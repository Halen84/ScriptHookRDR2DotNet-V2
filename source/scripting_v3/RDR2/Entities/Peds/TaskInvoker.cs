//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public class TaskInvoker
	{
		Ped _ped;

		internal TaskInvoker(Ped ped)
		{
			_ped = ped;
		}

		public void Pause(int ms)
		{
			TASK.TASK_PAUSE(_ped, ms);
		}

		public void StandStill(int time)
		{
			TASK.TASK_STAND_STILL(_ped, time);
		}

		public void Jump()
		{
			TASK.TASK_JUMP(_ped, true);
		}

		public void Cower(int duration, Ped pedToCowerFrom = null)
		{
			TASK.TASK_COWER(_ped, duration, pedToCowerFrom == null ? 0 : pedToCowerFrom, "");
		}

		public void HandsUp(int duration, Ped pedToFace = null, int timeToFacePed = -1)
		{
			TASK.TASK_HANDS_UP(_ped, duration, pedToFace == null ? 0 : pedToFace, timeToFacePed, false);
		}

		public void KnockedOut(float angle, bool permanently)
		{
			TASK.TASK_KNOCKED_OUT(_ped, angle, permanently);
		}

		public void KnockedOutAndHogtied(float angle, bool immediately)
		{
			TASK.TASK_KNOCKED_OUT_AND_HOGTIED(_ped, angle, immediately ? 1 : 0);
		}

		public void Duck(int time)
		{
			TASK.TASK_DUCK(_ped, time);
		}

		public void EnterVehicle(Vehicle vehicle, int timeout, eVehicleSeat seat, float speed, eEnterExitVehicleFlags flags = eEnterExitVehicleFlags.ResumeIfInterrupted)
		{
			TASK.TASK_ENTER_VEHICLE(_ped, vehicle, timeout, (int)seat, speed, (int)flags, 0);
		}

		public void LeaveVehicle(Vehicle vehicle, eEnterExitVehicleFlags flags = 0)
		{
			TASK.TASK_LEAVE_VEHICLE(_ped, vehicle, (int)flags, 0);
		}

		public void MountAnimal(Ped mount, int time, eVehicleSeat seat, float speed, eEnterExitVehicleFlags flags = eEnterExitVehicleFlags.ResumeIfInterrupted)
		{
			TASK.TASK_MOUNT_ANIMAL(_ped, mount, time, (int)seat, speed, (int)flags, 0, 0);
		}

		public void DismountAnimal(eEnterExitVehicleFlags flags = 0)
		{
			TASK.TASK_DISMOUNT_ANIMAL(_ped, (int)flags, 0, 0, 0, 0);
		}

		public void HitchAnimal(Ped mount, float hitchingPostSearchRadius)
		{
			int scenario = TASK.FIND_SCENARIO_OF_TYPE_HASH(mount.Position, MISC.GET_HASH_KEY("PROP_HITCHINGPOST"), hitchingPostSearchRadius, 0, false);
			TASK.TASK_HITCH_ANIMAL(mount, scenario, 0);
		}

		public void HitchAnimal(Ped mount, Vector3 coords)
		{
			PHYSICS._HITCH_HORSE(mount, coords);
		}

		public void DriveToCoord(Vehicle vehicle, Vector3 coords, float speed, uint vehicleModel, eDrivingFlags drivingMode, float targetRadius, float straightLineDist)
		{
			TASK.TASK_VEHICLE_DRIVE_TO_COORD(_ped, vehicle, coords, speed, 0, vehicleModel, (int)drivingMode, targetRadius, straightLineDist);
		}

		public void DriveWander(Vehicle vehicle, float speed, eDrivingFlags drivingMode)
		{
			TASK.TASK_VEHICLE_DRIVE_WANDER(_ped, vehicle, speed, (int)drivingMode);
		}

		public void FollowToOffsetOfEntity(Entity entity, Vector3 offset, float speed, int time = -1, float radius = 0.1f, bool relativeOffset = true)
		{
			TASK.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(_ped, entity, offset, speed, time, radius, relativeOffset, true, false, false, true, false);
		}

		public void GoStraightTo(Vector3 coords, float speed, int time, float finalHeading, float radius = 0.5f)
		{
			TASK.TASK_GO_STRAIGHT_TO_COORD(_ped, coords, speed, time, finalHeading, radius, 0);
		}

		public void FollowRoad(float speed, Vector3 destination = default)
		{
			TASK.TASK_MOVE_FOLLOW_ROAD_USING_NAVMESH(_ped, speed, destination, 0);
		}

		public void GoTo(Entity entity, float speed, int duration, Vector2 offset = default)
		{
			TASK.TASK_GOTO_ENTITY_OFFSET_XY(_ped, entity, duration, 1.0f, offset.X, offset.Y, speed, false);
		}

		public void GoTo(Vector3 coords, float speed, int timeout)
		{
			TASK.TASK_FOLLOW_NAV_MESH_TO_COORD(_ped, coords, speed, timeout, 1.0f, 4, 40000.0f);
		}

		public void GoToWhistle(Ped whistlerPed)
		{
			TASK.TASK_GO_TO_WHISTLE(_ped, whistlerPed, 0);
		}

		public void LeadHorse(Ped horse)
		{
			TASK.TASK_LEAD_HORSE(_ped, horse);
		}

		public void StopLeadingHorse()
		{
			TASK.TASK_STOP_LEADING_HORSE(_ped);
		}

		public void Flee(Vector3 fleeFrom, int duration = -1, eFleeStyle fleeStyle = eFleeStyle.MajorThreat)
		{
			TASK.TASK_FLEE_COORD(_ped, fleeFrom, (int)fleeStyle, 0, -1.0f, duration, 0);
		}

		public void Flee(Ped pedToFleeFrom, int duration = -1, eFleeStyle fleeStyle = eFleeStyle.MajorThreat)
		{
			TASK.TASK_FLEE_PED(_ped, pedToFleeFrom, (int)fleeStyle, 0, -1.0f, duration, 0);
		}

		public void FlyAway(Ped pedToFlyFrom)
		{
			TASK.TASK_FLY_AWAY(_ped, pedToFlyFrom);
		}

		public void FlyToCoord(Vector3 coords, float speed)
		{
			TASK.TASK_FLY_TO_COORD(_ped, speed, coords, true, false);
		}

		public void WalkAway(Entity entityToWalkFrom)
		{
			TASK.TASK_WALK_AWAY(_ped, entityToWalkFrom);
		}

		public void React(int shockingEventHandle)
		{
			TASK.TASK_SHOCKING_EVENT_REACT(_ped, (ulong)shockingEventHandle, 0);
		}

		public void React(Ped pedToReactTo, string reactionName)
		{
			TASK.TASK_REACT(_ped, pedToReactTo, default(Vector3), reactionName, 2.0f, 0.5f, 4);
		}

		public void React(Vector3 reactTowards, string reactionName)
		{
			TASK.TASK_REACT(_ped, 0, reactTowards, reactionName, 2.0f, 0.5f, 4);
		}

		public void WanderAround(Vector3 position, float radius)
		{
			TASK.TASK_WANDER_IN_AREA(_ped, position, radius, 3.0f, 6.0f, 0);
		}

		public void WanderAround()
		{
			TASK.TASK_WANDER_STANDARD(_ped, 0, 0);
		}

		public void PlantBomb(Vector3 coords, float heading)
		{
			TASK.TASK_PLANT_BOMB(_ped, coords, heading);
		}

		public void DoHorseAction(int action)
		{
			TASK.TASK_HORSE_ACTION(_ped, action, 0, 0);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration, eScriptedAnimFlags animFlags = eScriptedAnimFlags.None, eIkControlFlags ikFlags = eIkControlFlags.None, string filter = "")
		{
			if (!STREAMING.HAS_ANIM_DICT_LOADED(animDict)) {
				STREAMING.REQUEST_ANIM_DICT(animDict);
			}

			var end = DateTime.UtcNow.AddMilliseconds(1000);
			while (!STREAMING.HAS_ANIM_DICT_LOADED(animDict)) {
				if (DateTime.UtcNow >= end) {
					return;
				}
			}
			
			TASK.TASK_PLAY_ANIM(_ped, animDict, animName, blendInSpeed, blendOutSpeed, duration, (int)animFlags, 0.0f, false, (int)ikFlags, false, filter, false);
		}

		public void PlayAnimation(string animDict, string animName, int duration)
		{
			PlayAnimation(animDict, animName, 8f, -8f, duration);
		}
		
		public void PlayAnimation(string animDict, string animName, int duration, float speed = 8.0f)
		{
			PlayAnimation(animDict, animName, speed, -speed, duration);
		}

		public void PlayAnimation(string animDict, string animName, int duration, float speed = 8.0f, eScriptedAnimFlags animFlags = eScriptedAnimFlags.None, eIkControlFlags ikflags = eIkControlFlags.None)
		{
			PlayAnimation(animDict, animName, speed, -speed, duration, animFlags, ikflags);
		}

		public void StopAnimation(string animDict, string animName)
		{
			TASK.STOP_ANIM_TASK(_ped, animDict, animName, -8.0f);
		}

		public void StartItemInteraction(uint itemHash, uint interactionAnimHash)
		{
			TASK.START_TASK_ITEM_INTERACTION(_ped, itemHash, interactionAnimHash, 1, 0, -1.0f);
		}

		public void LookAt(Vector3 coords, int duration)
		{
			TASK.TASK_LOOK_AT_COORD(_ped, coords, duration, 0, 51, 0);
		}

		public void LookAt(Entity entity, int duration)
		{
			TASK.TASK_LOOK_AT_ENTITY(_ped, entity, duration, 0, 51, 0);
		}

		public void ClearLookAt()
		{
			TASK.TASK_CLEAR_LOOK_AT(_ped);
		}

		public void LeaveAnyVehicle(eEnterExitVehicleFlags flags = eEnterExitVehicleFlags.None)
		{
			TASK.TASK_LEAVE_ANY_VEHICLE(_ped, 0, (int)flags);
		}

		public void UseRandomScenarioInGroup(uint scenarioGroupHash)
		{
			TASK.TASK_USE_RANDOM_SCENARIO_IN_GROUP(_ped, scenarioGroupHash, 0, 0 ,0);
		}

		public void AimAt(Entity entity, int duration)
		{
			TASK.TASK_AIM_GUN_AT_ENTITY(_ped, entity, duration, false, 1);
		}

		public void AimAt(Vector3 coords, int duration)
		{
			TASK.TASK_AIM_GUN_AT_COORD(_ped, coords, duration, false, false);
		}

		public void ShootAt(Vector3 coords, int duration, eFiringPattern firingPattern)
		{
			TASK.TASK_SHOOT_AT_COORD(_ped, coords, duration, (uint)firingPattern, 0);
		}

		public void ShootAt(Entity entity, int duration, eFiringPattern firingPattern)
		{
			TASK.TASK_SHOOT_AT_ENTITY(_ped, entity, duration, (uint)firingPattern, true);
		}

		public void ShuffleToNextVehicleSeat(Vehicle vehicle)
		{
			TASK.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT(_ped, vehicle);
		}

		public void ClearTasks(bool immediately)
		{
			if (immediately) {
				TASK.CLEAR_PED_TASKS_IMMEDIATELY(_ped, true, true);
			}
			else {
				TASK.CLEAR_PED_TASKS(_ped, true, true);
			}
		}

		public void ClearSecondaryTasks()
		{
			TASK.CLEAR_PED_SECONDARY_TASK(_ped);
		}

		public void TurnTo(Entity entity, int duration)
		{
			TASK.TASK_TURN_PED_TO_FACE_ENTITY(_ped, entity, duration, 0, 0, 0);
		}

		public void TurnTo(Vector3 coords, int duration)
		{
			TASK.TASK_TURN_PED_TO_FACE_COORD(_ped, coords, duration);
		}

		public void Climb()
		{
			TASK.TASK_CLIMB(_ped, false);
		}

		public void ClimbLadder()
		{
			TASK.TASK_CLIMB_LADDER(_ped, 2.0f, 0, 0);
		}

		public void SlideTo(Vector3 coords, float heading, float speed = 0.75f)
		{
			TASK.TASK_PED_SLIDE_TO_COORD(_ped, coords, heading, speed);
		}

		public void SlideTo(Entity entity, float heading, float speed = 0.75f)
		{
			TASK.TASK_PED_SLIDE_TO_COORD(_ped, entity.Position, heading, speed);
		}

		public void Combat(Ped ped, int duration)
		{
			TASK.TASK_COMBAT_PED_TIMED(_ped, ped, duration, 0);
		}

		public void SeekCoverFrom(Vector3 coords, int time)
		{
			TASK.TASK_SEEK_COVER_FROM_POS(_ped, coords, time, 0, 0, 0);
		}

		public void SeekCoverFrom(Ped ped, int time)
		{
			TASK.TASK_SEEK_COVER_FROM_PED(_ped, ped, time, 0, 0, 1);
		}

		public void GuardCurrentPosition(float patrolProximity, float radius)
		{
			TASK.TASK_GUARD_CURRENT_POSITION(_ped, patrolProximity, radius, false);
		}

		public void StandGuard(Vector3 coords)
		{
			TASK.TASK_STAND_GUARD(_ped, coords, _ped.Heading, "DEFEND");
		}

		public void StartScenario(uint scenarioHash, Vector3 position, int duration, bool teleportToScenario, bool isSittingScenario)
		{
			TASK.TASK_START_SCENARIO_AT_POSITION(_ped, scenarioHash, position, _ped.Heading, duration, isSittingScenario, teleportToScenario, "", -1.0f, false);
		}

		public void FightAgainstHatedTargets(float radius)
		{
			TASK.TASK_COMBAT_HATED_TARGETS_AROUND_PED(_ped, radius, 0, 0);
		}

		public void FightAgainstHatedTargets(float radius, float duration)
		{
			TASK.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED(_ped, radius, duration, 0);
		}

		public void Reload()
		{
			TASK.TASK_RELOAD_WEAPON(_ped, true);
		}

		public void ForceMotionState(uint motionStateHash)
		{
			TASK.TASK_FORCE_MOTION_STATE(_ped, motionStateHash, false);
		}

		public void PickupCarriableEntity(Entity entity)
		{
			TASK.TASK_PICKUP_CARRIABLE_ENTITY(_ped, entity);
		}

		public void HogtiePed(Ped target)
		{
			TASK.TASK_HOGTIE_TARGET_PED(_ped, target);
		}

		public void CutHogtiedPedFree(Ped target)
		{
			TASK._TASK_CUT_FREE_HOGTIED_TARGET_PED(_ped, target);
		}

		public void LootEntity(Entity entity)
		{
			TASK.TASK_LOOT_ENTITY(_ped, entity);
		}

		public void LassoPed(Ped ped)
		{
			TASK.TASK_LASSO_PED(_ped, ped);
		}

		public void EnterCover()
		{
			AICOVERPOINT.TASK_ENTER_COVER(_ped);
		}

		public void ExitCover()
		{
			AICOVERPOINT.TASK_EXIT_COVER(_ped);
		}

		public void PerformSequence(TaskSequence sequence)
		{
			if (!sequence.IsClosed) {
				sequence.Close();
			}

			ClearTasks(false);

			_ped.BlockPermanentEvents = true;

			TASK.TASK_PERFORM_SEQUENCE(_ped, sequence.Handle);
		}
	}

	public enum eAudPedWhistleType : uint
	{
		WHISTLEHORSERESPONSIVE = 0xFBB22B86,
		WHISTLEHORSETALK = 0x2628D6E0,
		WHISTLEHORSELONG = 0x33D023F4,
		WHISTLEHORSEDOUBLE = 0x3EAC666C,
		WHISTLEHORSESHORT = 0x659F956D,
		WHISTLEANIMALNOISES = 0x6BBE62DD,
	}

	public enum EventReaction
	{
		TaskCombatHigh = 1103872808,
		TaskCombatMedium = 623557147,
		TaskCombatReact = -1342511871,
		TaskCombatPanic = -996719768,
		DefaultShocked = -372548123,
		DefaultPanic = 1618376518,
		DefaultCurious = -1778605437,
		DefaultBrave = 1781933509,
		DefaultAngry = 1345150177,
		DefaultDefuse = -1675652957,
		DefaultScared = -1967172690,
		FleeHumanMajorThreat = -2111647205,
		FleeScared = 759577278
	}
}
