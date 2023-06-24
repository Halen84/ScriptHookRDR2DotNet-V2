//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using RDR2.Native;

namespace RDR2
{
	public sealed class Player : INativeValue
	{
		private Ped _pedPlayer;

		public Player(int handle)
		{
			Handle = handle;
		}

		public int Handle { get; private set; }
		public int ID => Handle;

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		/// <summary>
		/// Gets the Social Club name of this <see cref="Player"/>.
		/// </summary>
		public string Name => PLAYER.GET_PLAYER_NAME(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is dead.
		/// </summary>
		public bool IsDead => PLAYER.IS_PLAYER_DEAD(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is alive.
		/// </summary>
		public bool IsAlive => !IsDead;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is aiming with a weapon.
		/// </summary>
		public bool IsAiming => PLAYER.IS_PLAYER_FREE_AIMING(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is climbing.
		/// </summary>
		public bool IsClimbing => PLAYER.IS_PLAYER_CLIMBING(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is riding a train.
		/// </summary>
		public bool IsRidingTrain => PLAYER.IS_PLAYER_RIDING_TRAIN(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is targetting something.
		/// </summary>
		public bool IsTargettingAnything => PLAYER.IS_PLAYER_TARGETTING_ANYTHING(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is free focusing on something.
		/// </summary>
		public bool IsFreeFocusing => PLAYER._IS_PLAYER_FREE_FOCUSING(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is currently in Dead Eye.
		/// </summary>
		public bool IsInDeadEye => PLAYER._IS_SPECIAL_ABILITY_ACTIVE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is currently in Eagle Eye.
		/// </summary>
		public bool IsInEagleEye => PLAYER._IS_SECONDARY_SPECIAL_ABILITY_ACTIVE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is currently wanted by police/lawmen.
		/// </summary>
		public bool IsWanted => LAW.IS_LAW_INCIDENT_ACTIVE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is playing.
		/// </summary>
		public bool IsPlaying => PLAYER.IS_PLAYER_PLAYING(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> can start a story mission.
		/// </summary>
		public bool CanStartMission => PLAYER.CAN_PLAYER_START_MISSION(Handle);

		/// <summary>
		/// Gets the <see cref="Vehicle"/> that this <see cref="Player"/> last entered.
		/// </summary>
		public Vehicle LastVehicle => (Vehicle)Vehicle.FromHandle(PLAYER.GET_PLAYERS_LAST_VEHICLE());

		/// <summary>
		/// Gets the <see cref="Player"/>'s currently active horse
		/// </summary>
		public Ped ActiveHorse => (Ped)Ped.FromHandle(PLAYER._GET_SADDLE_HORSE_FOR_PLAYER(Handle));

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> can control himself.
		/// </summary>
		/// <remarks>Use <see cref="SetPlayerControl(bool, eSetPlayerControlFlags, bool)"/> to change this value.</remarks>
		public bool IsControlOn => PLAYER.IS_PLAYER_CONTROL_ON(Handle);

		/// <summary>
		/// Gets the amount of time since last seen by the law / left the wanted radius
		/// </summary>
		public float TimeSinceLastSeenByLaw => LAW._GET_TIME_SINCE_LAST_SEEN_BY_LAW(Handle);

		/// <summary>
		/// Gets or sets the current mood of the player
		/// </summary>
		public ePedMood Mood
		{
			get => (ePedMood)PLAYER._GET_PLAYER_MOOD(Handle);
			set => PLAYER._SET_PLAYER_MOOD(Handle, (int)value);
		}

		/// <summary>
		/// Gets the <see cref="RDR2.Ped"/> this <see cref="RDR2.Player"/> is controlling.
		/// </summary>
		public Ped Character
		{
			get
			{
				int ped = PLAYER.PLAYER_PED_ID();
				if (_pedPlayer == null || ped != _pedPlayer.Handle)
				{
					_pedPlayer = new Ped(ped);
				}
				return _pedPlayer;
			}
		}

		/// <summary>
		/// Gets the <see cref="RDR2.Ped"/> this <see cref="RDR2.Player"/> is controlling.
		/// </summary>
		/// <remarks>This is an alias for <see cref="Character"/></remarks>
		public Ped Ped => Character;

		/// <summary>
		/// Gets or sets how much money this <see cref="Player"/> has.
		/// </summary>
		public int Money
		{
			get => MONEY._MONEY_GET_CASH_BALANCE();
			set
			{
				var source = Money;
				var target = value;
				if (target < source)
				{
					MONEY._MONEY_DECREMENT_CASH_BALANCE(source - target);
				}
				else
				{
					MONEY._MONEY_INCREMENT_CASH_BALANCE(target - source, (uint)eAddItemReason.Default);
				}
			}
		}

		/// <summary>
		/// Gets or sets how much money this <see cref="Player"/> has.
		/// </summary>
		/// <remarks>This is an alias for <see cref="Money"/></remarks>
		public int Cash
		{
			get => Money;
			set => Money = value;
		}

		/// <summary>
		/// Gets or sets this <see cref="Player"/>'s bounty amount.
		/// </summary>
		public int Bounty
		{
			get => LAW.GET_BOUNTY(Handle);
			set => LAW.SET_BOUNTY(Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Player"/> is invincible.
		/// </summary>
		public bool Invincible
		{
			get => PLAYER.GET_PLAYER_INVINCIBLE(Handle);
			set => PLAYER.SET_PLAYER_INVINCIBLE(Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> is ignored by everyone.
		/// </summary>
		/// <remarks>This should be set every frame (if possible) to take full effect</remarks>
		public bool IgnoredByEveryone
		{
			set
			{
				PLAYER.SET_EVERYONE_IGNORE_PLAYER(Handle, value);
				PED.SET_PED_CONFIG_FLAG(Ped.Handle, (int)ePedScriptConfigFlags.PCF_DisableShockingEvents, value);
				PED.SET_PED_CONFIG_FLAG(Ped.Handle, (int)ePedScriptConfigFlags.PCF_SupressShockingEvents, value);
				PED.SET_PED_RESET_FLAG(Ped.Handle, (int)ePedScriptResetFlags.PRF_CannotBeTargetedByAI, value);
				EVENT.SUPPRESS_SHOCKING_EVENTS_NEXT_FRAME();
				EVENT.REMOVE_ALL_SHOCKING_EVENTS(value);
			}
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Player"/> can use cover.
		/// </summary>
		public bool CanUseCover
		{
			set => PLAYER.SET_PLAYER_CAN_USE_COVER(Handle, value);
		}

		/// <summary>
		/// Set whether this <see cref="Player"/> can control himself or not.
		/// </summary>
		/// <param name="bEnabled">Whether control is enabled or not.</param>
		/// <param name="iFlags">Flags that affect how player control is handled</param>
		/// <param name="bPreventHeadingChange">Whether the player can still rotate his <see cref="Character"/>.</param>
		public void SetPlayerControl(bool bEnabled, eSetPlayerControlFlags iFlags = eSetPlayerControlFlags.None, bool bPreventHeadingChange = true)
		{
			PLAYER.SET_PLAYER_CONTROL(Handle, bEnabled, (int)iFlags, bPreventHeadingChange);
		}

		// TODO: Fix this.
		/// <summary>
		/// Attempts to change the <see cref="Model"/> of this <see cref="Player"/>.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> to change this <see cref="Player"/> to.</param>
		/// <returns><see langword="true" /> if the change was successful; otherwise, <see langword="false" />.</returns>
		public bool ChangeModel(Model model)
		{
			if (!model.IsInCdImage || !model.IsPed || !model.Request(1000))
			{
				return false;
			}

			PLAYER.SET_PLAYER_MODEL(Handle, (uint)model.Hash, false);
			STREAMING.SET_MODEL_AS_NO_LONGER_NEEDED(model);
			return true;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Player"/> is aiming at the passed <see cref="Entity"/>.
		/// </summary>
		public bool IsAimingAtEntity(Entity entity)
		{
			return PLAYER.IS_PLAYER_FREE_AIMING_AT_ENTITY(Handle, entity.Handle);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> that this <see cref="Player"/> is aiming at.
		/// </summary>
		public unsafe Entity GetFreeAimEntity()
		{
			int entity = 0;
			if (!PLAYER.GET_ENTITY_PLAYER_IS_FREE_AIMING_AT(Handle, &entity))
			{
				return null;
			}
			return Entity.FromHandle(entity);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> that this <see cref="Player"/> is locking onto via interaction.
		/// </summary>
		public unsafe Entity GetInteractionLockonTargetEntity()
		{
			int entity = 0;
			if (PLAYER.GET_PLAYER_INTERACTION_TARGET_ENTITY(Handle, &entity, false, false))
			{
				return null;
			}
			return Entity.FromHandle(entity);
		}

		/// <summary>
		/// Prevents this <see cref="Player"/> from firing for one frame.
		/// </summary>
		public void DisableFiringThisFrame(bool toggle)
		{
			PLAYER.DISABLE_PLAYER_FIRING(Handle, toggle);
		}

		/// <summary>
		/// Allows this <see cref="Player"/> jump really high for one frame.
		/// </summary>
		public void SetSuperJumpThisFrame()
		{
			MISC.SET_SUPER_JUMP_THIS_FRAME(Handle);
		}

		public bool Exists()
		{
			// IHandleable forces us to implement this unfortunately,
			// so we'll implement it explicitly and return true
			return true;
		}

		public bool Equals(Player obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Player)obj);
		}

		public static bool operator ==(Player left, Player right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}

		public static implicit operator int(Player e) => e.Handle;
		public static implicit operator Player(int handle) => Game.Player;

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}


	[Flags]
	public enum eSetPlayerControlFlags : int
	{
		None = 0,
		// Where's 1?
		AmbientScript= (1 << 1),
		ClearTasks = (1 << 2),
		RemoveFires = (1 << 3),
		RemoveExplosives = (1 << 4),
		RemoveProjectiles = (1 << 5),
		DeactiveGadgets = (1 << 6),
		ReenableControlOnDeath = (1 << 7),
		LeaveCameraControlOn = (1 << 8),
		AllowPlayerDamage = (1 << 9),
		DontStopOtherCarsAroundPlayer= (1 << 10),
		PreventEverybodyBackoff= (1 << 11),
		AllowPadShake = (1 << 12),
	}
}
