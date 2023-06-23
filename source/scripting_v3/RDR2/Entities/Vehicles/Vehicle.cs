//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public sealed class Vehicle : Entity
	{
		public Vehicle(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is xxx.
		/// </summary>
		public bool IsDraftVehicle => VEHICLE.IS_DRAFT_VEHICLE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is on all of its wheels.
		/// </summary>
		public bool IsOnAllWheels => VEHICLE.IS_VEHICLE_ON_ALL_WHEELS(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is stopped.
		/// </summary>
		public bool IsStopped => VEHICLE.IS_VEHICLE_STOPPED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is wrecked.
		/// </summary>
		public bool IsWrecked => VEHICLE.IS_VEHICLE_WRECKED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Vehicle"/> is on fire.
		/// </summary>
		public new bool IsOnFire => VEHICLE._IS_VEHICLE_ON_FIRE(Handle);

		/// <summary>
		/// Sets a value indicating whether this <see cref="Vehicle"/> is stolen.
		/// </summary>
		public bool IsStolen
		{
			set => VEHICLE.SET_VEHICLE_IS_STOLEN(Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Vehicle"/> can provide cover.
		/// </summary>
		public bool ProvidesCover
		{
			set => VEHICLE.SET_VEHICLE_PROVIDES_COVER(Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/>s body health.
		/// </summary>
		public float BodyHealth
		{
			get => VEHICLE.GET_VEHICLE_BODY_HEALTH(Handle);
			set => VEHICLE.SET_VEHICLE_BODY_HEALTH(Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Vehicle"/>s speed.
		/// </summary>
		public new float Speed
		{
			get => ENTITY.GET_ENTITY_SPEED(Handle);
			set
			{
				if (Model.IsTrain)
				{
					VEHICLE.SET_TRAIN_SPEED(Handle, value);
					VEHICLE.SET_TRAIN_CRUISE_SPEED(Handle, value);
				}
				else
				{
					VEHICLE.SET_VEHICLE_FORWARD_SPEED(Handle, value);
				}
			}
		}

		/// <summary>
		/// Repair all damage to this <see cref="Vehicle"/> instantaneously.
		/// </summary>
		public void Repair()
		{
			VEHICLE.SET_VEHICLE_FIXED(Handle);
		}

		/// <summary>
		/// Explode this <see cref="Vehicle"/> instantaneously.
		/// </summary>
		public void Explode()
		{
			VEHICLE.EXPLODE_VEHICLE(Handle, true, false, 0, 0);
		}


		#region Styling

		public bool HasPropSetAttached => VEHICLE._GET_VEHICLE_IS_PROP_SET_APPLIED(Handle);

		public bool IsExtraOn(int extra)
		{
			return VEHICLE.IS_VEHICLE_EXTRA_TURNED_ON(Handle, extra);
		}

		public bool ExtraExists(int extra)
		{
			return VEHICLE.DOES_EXTRA_EXIST(Handle, extra);
		}

		public void ToggleExtra(int extra, bool toggle)
		{
			VEHICLE.SET_VEHICLE_EXTRA(Handle, extra, !toggle);
		}

		#endregion

		#region Damaging

		public bool IsDamaged
		{
			get
			{
				int health = ENTITY.GET_ENTITY_HEALTH(Handle);
				int maxHealth = ENTITY.GET_ENTITY_MAX_HEALTH(Handle, false);

				if (health < maxHealth)
				{
					return true;
				}
				if (health > maxHealth)
				{
					return false;
				}
				return false;
			}
		}
		public bool IsDriveable
		{
			get => VEHICLE.IS_VEHICLE_DRIVEABLE(Handle, true, false);
			set => VEHICLE.SET_VEHICLE_UNDRIVEABLE(Handle, !value);
		}

		public bool IsAxlesStrong
		{
			set => VEHICLE.SET_VEHICLE_HAS_STRONG_AXLES(Handle, value);
		}

		public bool CanWheelsBreak
		{
			set => VEHICLE.SET_VEHICLE_WHEELS_CAN_BREAK(Handle, value);
		}

		public bool CanBeVisiblyDamaged
		{
			set => VEHICLE.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED(Handle, value);
		}


		public void ApplyDamage(Vector3 loc, float damageAmount, float radius)
		{
			VEHICLE.SET_VEHICLE_DAMAGE(Handle, loc.X, loc.Y, loc.Z, damageAmount, radius, true);
		}

		#endregion

		#region Occupants

		/// <summary>
		/// Gets the driver of this <see cref="Vehicle"/>
		/// </summary>
		public Ped Driver => GetPedInSeat(eVehicleSeat.Driver);

		/// <summary>
		/// Gets the <see cref="Ped"/> thats in the specified <see cref="eVehicleSeat"/>
		/// </summary>
		public Ped GetPedInSeat(eVehicleSeat seat)
		{
			return (Ped)FromHandle(VEHICLE.GET_PED_IN_VEHICLE_SEAT(Handle, (int)seat));
		}

		/// <summary>
		/// Returns an <see cref="Array"/> of all occupants in this <see cref="Vehicle"/>, INCLUDING the driver
		/// </summary>
		public Ped[] Occupants
		{
			get
			{
				Ped driver = Driver;

				if (PassengerCount == 0 && !Ped.Exists(driver))
				{
					return Array.Empty<Ped>();
				}

				Ped[] peds = new Ped[PassengerCount + 1];
				int pedIndex = 1;
				peds[0] = driver;

				for (int i = 0; i < (int)eVehicleSeat.NumSeats; i++)
				{
					Ped ped = GetPedInSeat((eVehicleSeat)i);

					if (!Ped.Exists(ped)) { continue; }

					peds[pedIndex] = ped;
					pedIndex++;

					if (pedIndex >= peds.Length)
					{
						return peds;
					}
				}

				return peds;
			}
		}

		/// <summary>
		/// Returns an <see cref="Array"/> of all passengers in this <see cref="Vehicle"/>, EXCLUDING the driver
		/// </summary>
		public Ped[] Passengers
		{
			get
			{
				if (PassengerCount == 0)
				{
					return Array.Empty<Ped>();
				}

				Ped[] peds = new Ped[PassengerCount];
				int pedIndex = 0;

				for (int i = 0; i < (int)eVehicleSeat.NumSeats; i++)
				{
					Ped ped = GetPedInSeat((eVehicleSeat)i);

					if (!Ped.Exists(ped)) { continue; }

					peds[pedIndex] = ped;
					pedIndex++;

					if (pedIndex >= peds.Length)
					{
						return peds;
					}
				}

				return peds;
			}
		}

		/// <summary>
		/// Gets the current number of passengers in this <see cref="Vehicle"/>
		/// </summary>
		public int PassengerCount => VEHICLE.GET_VEHICLE_NUMBER_OF_PASSENGERS(Handle);

		/// <summary>
		/// Gets the max number of passengers this <see cref="Vehicle"/> can have
		/// </summary>
		public int PassengerSeats => VEHICLE.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS(Handle);

		public Ped CreatePedOnSeat(eVehicleSeat seat, Model model)
		{
			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return (Ped)FromHandle(PED.CREATE_PED_INSIDE_VEHICLE(Handle, (uint)model.Hash, (int)seat, true, true, false));
		}


		public bool IsSeatFree(eVehicleSeat seat)
		{
			return VEHICLE.IS_VEHICLE_SEAT_FREE(Handle, (int)seat);
		}

		#endregion

		#region Positioning

		public bool PlaceOnGround()
		{
			return VEHICLE.SET_VEHICLE_ON_GROUND_PROPERLY(Handle, false);
		}

		public void PlaceOnNextStreet()
		{
			Vector3 pos = Position;

			for (int i = 0; i < 40; i++)
			{
				float heading;
				ulong unk;
				Vector3 outPos = Vector3.Zero;
				Vector3 newPos = outPos;
				unsafe
				{
					if (PATHFIND.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING(pos, i, &outPos, &heading, &unk, 1, 3.0f, 0.0f))
					{
						Position = newPos;
						PlaceOnGround();
						Heading = heading;
						break;
					}
				}
			}
		}

		#endregion

	}
}
