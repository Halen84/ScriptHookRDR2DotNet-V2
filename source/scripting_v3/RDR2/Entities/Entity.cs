//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public abstract class Entity : PoolObject, ISpatial
	{
		internal Entity(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Creates a new instance of an <see cref="Entity"/> from the given handle.
		/// </summary>
		/// <param name="handle">The entity handle.</param>
		/// <returns>Returns a <see cref="Ped"/> if this handle corresponds to a Ped.
		/// Returns a <see cref="Vehicle"/> if this handle corresponds to a Vehicle.
		/// Returns a <see cref="Prop"/> if this handle corresponds to a Prop.
		/// Returns <see langword="null"/> otherwise.</returns>
		public static Entity FromHandle(int handle)
		{
			switch ((eEntityType)ENTITY.GET_ENTITY_TYPE(handle))
			{
				case eEntityType.Ped: return new Ped(handle);
				case eEntityType.Vehicle: return new Vehicle(handle);
				case eEntityType.Object: return new Prop(handle);
				default: return null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is dead.
		/// </summary>
		public bool IsDead => ENTITY.IS_ENTITY_DEAD(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is alive.
		/// </summary>
		public bool IsAlive => !IsDead;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is a ped.
		/// </summary>
		public bool IsPed => ENTITY.IS_ENTITY_A_PED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is an object.
		/// </summary>
		public bool IsObject => ENTITY.IS_ENTITY_AN_OBJECT(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is a vehicle.
		/// </summary>
		public bool IsVehicle => ENTITY.IS_ENTITY_A_VEHICLE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is an animal.
		/// </summary>
		public bool IsAnimal => ENTITY.GET_IS_ANIMAL(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is occluded.
		/// </summary>
		public bool IsOccluded => ENTITY.IS_ENTITY_OCCLUDED(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is on fire.
		/// </summary>
		public bool IsOnFire => FIRE.IS_ENTITY_ON_FIRE(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is currently on screen.
		/// </summary>
		public bool IsOnScreen => ENTITY.IS_ENTITY_ON_SCREEN(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is in the upright position.
		/// </summary>
		public bool IsUpright => ENTITY.IS_ENTITY_UPRIGHT(Handle, 30.0f);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is upsidedown.
		/// </summary>
		public bool IsUpsideDown => ENTITY.IS_ENTITY_UPSIDEDOWN(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is in the air.
		/// </summary>
		public bool IsInAir => ENTITY.IS_ENTITY_IN_AIR(Handle, 0);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is in water.
		/// </summary>
		public bool IsInWater => ENTITY.IS_ENTITY_IN_WATER(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is  underwater.
		/// </summary>
		public bool IsUnderwater => ENTITY._IS_ENTITY_UNDERWATER(Handle, true);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/>'s position is frozen.
		/// </summary>
		public bool IsFrozenInPlace => ENTITY._IS_ENTITY_FROZEN(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is owned by this script.
		/// </summary>
		public bool IsOwnedByThisScript => ENTITY.DOES_ENTITY_BELONG_TO_THIS_SCRIPT(Handle, true) || ENTITY._DOES_THREAD_OWN_THIS_ENTITY(Handle);

		/// <summary>
		/// Attempts to request script ownership of this <see cref="Entity"/> if not already owned.
		/// </summary>
		public bool RequestOwnership()
		{
			if (!IsOwnedByThisScript) {
				IsMissionEntity = true;
			}
			return IsMissionEntity == true;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is static.
		/// </summary>
		public bool IsStatic => ENTITY.IS_ENTITY_STATIC(Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is dynamic.
		/// </summary>
		public bool IsDynamic => !IsStatic;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is inside/in a interior.
		/// </summary>
		public bool IsInside => INTERIOR.IS_VALID_INTERIOR(INTERIOR.GET_INTERIOR_FROM_ENTITY(Handle));

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> is visible.
		/// </summary>
		public bool IsVisible
		{
			get => ENTITY.IS_ENTITY_VISIBLE(Handle);
			set => ENTITY.SET_ENTITY_VISIBLE(Handle, value);
		}

		/// <summary>
		/// Gets the model of the current <see cref="Entity"/>.
		/// </summary>
		public Model Model => new Model(ENTITY.GET_ENTITY_MODEL(Handle));

		/// <summary>
		/// Gets or sets how opaque this <see cref="Entity"/> is.
		/// </summary>
		/// <value>
		/// 0 for completely see through, 255 for fully opaque
		/// </value>
		public int Alpha
		{
			get => ENTITY.GET_ENTITY_ALPHA(Handle);
			set => ENTITY.SET_ENTITY_ALPHA(Handle, value, false);
		}

		/// <summary>
		/// Gets or sets how opaque this <see cref="Entity"/> is.
		/// <remarks>This is an alias for <see cref="Alpha"/></remarks>
		/// </summary>
		/// <value>
		/// 0 for completely see through, 255 for fully opaque
		/// </value>
		public int Opacity
		{
			get => Alpha;
			set => Alpha = value;
		}

		/// <summary>
		/// Gets the type of the current <see cref="Entity"/>.
		/// </summary>
		public eEntityType EntityType => (eEntityType)ENTITY.GET_ENTITY_TYPE(Handle);

		/// <summary>
		/// Gets or sets the level of detail distance of this <see cref="Entity"/>.
		/// </summary>
		public int LodDistance
		{
			get => ENTITY.GET_ENTITY_LOD_DIST(Handle);
			set => ENTITY.SET_ENTITY_LOD_DIST(Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is a mission entity.
		/// </summary>
		public bool IsMissionEntity
		{
			get => ENTITY.IS_ENTITY_A_MISSION_ENTITY(Handle);
			set => ENTITY.SET_ENTITY_AS_MISSION_ENTITY(Handle, value, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is persistent.
		/// </summary>
		public bool IsPersistent
		{
			get => IsMissionEntity;
			set => IsPersistent = value;
		}

		/// <summary>
		/// Gets or sets the health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// </summary>
		public int Health
		{
			get => ENTITY.GET_ENTITY_HEALTH(Handle);
			set => ENTITY.SET_ENTITY_HEALTH(Handle, value, 0);
		}

		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Entity"/> as an <see cref="int"/>.
		/// </summary>
		public virtual int MaxHealth
		{
			get => ENTITY.GET_ENTITY_MAX_HEALTH(Handle, false);
			set => ENTITY.SET_ENTITY_MAX_HEALTH(Handle, value);
		}

		#region Positioning

		/// <summary>
		/// Gets or sets the position of this <see cref="Entity"/>.
		/// If the <see cref="Entity"/> is <see cref="Ped"/> and the <see cref="Ped"/> is in a <see cref="Vehicle"/>, the <see cref="Vehicle"/>'s position will be returned or changed.
		/// </summary>
		public virtual Vector3 Position
		{
			get => ENTITY.GET_ENTITY_COORDS(Handle, true, true);
			set => ENTITY.SET_ENTITY_COORDS(Handle, value.X, value.Y, value.Z, true, true, true, true);
		}

		/// <summary>
		/// Sets the position of this <see cref="Entity"/> without any offset.
		/// </summary>
		public Vector3 PositionNoOffset
		{
			set => ENTITY.SET_ENTITY_COORDS_NO_OFFSET(Handle, value.X, value.Y, value.Z, true, true, true);
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Entity"/>.
		/// </summary>
		public virtual Vector3 Rotation
		{
			get => ENTITY.GET_ENTITY_ROTATION(Handle, 2);
			set => ENTITY.SET_ENTITY_ROTATION(Handle, value.X, value.Y, value.Z, 2, true);
		}

		/// <summary>
		/// Gets or sets the heading or "yaw" of this <see cref="Entity"/>.
		/// </summary>
		public float Heading
		{
			get => ENTITY.GET_ENTITY_HEADING(Handle);
			set => ENTITY.SET_ENTITY_HEADING(Handle, value);
		}

		/// <summary>
		/// Gets a value indicating how submersed this <see cref="Entity"/> is, 1.0f means the whole entity is submerged.
		/// </summary>
		public float SubmergedLevel => ENTITY.GET_ENTITY_SUBMERGED_LEVEL(Handle);

		/// <summary>
		/// Gets how high above ground this <see cref="Entity"/> is.
		/// </summary>
		public float HeightAboveGround => ENTITY.GET_ENTITY_HEIGHT_ABOVE_GROUND(Handle);

		/// <summary>
		/// Gets the quaternion of this <see cref="Entity"/>.
		/// </summary>
		public Quaternion Quaternion
		{
			set => ENTITY.SET_ENTITY_QUATERNION(Handle, value.X, value.Y, value.Z, value.W);
		}

		/// <summary>
		/// Gets the vector that points above this <see cref="Entity"/>.
		/// </summary>
		public Vector3 UpVector
		{
			get => Vector3.Cross(RightVector, ForwardVector);
		}

		/// <summary>
		/// Gets the vector that points to the right of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				const double D2R = 0.01745329251994329576923690768489;
				double num1 = System.Math.Cos(Rotation.Y * D2R);
				double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
				double y = num1 * System.Math.Sin(Rotation.Z * D2R);
				double z = System.Math.Sin(-Rotation.Y * D2R);
				return new Vector3((float)x, (float)y, (float)z);
			}
		}

		/// <summary>
		/// Gets the vector that points in front of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 ForwardVector
		{
			get => ENTITY.GET_ENTITY_FORWARD_VECTOR(Handle);
		}

		/// <summary>
		/// Gets the position in world coordinates of an offset relative this <see cref="Entity"/>.
		/// </summary>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			return ENTITY.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS(Handle, offset.X, offset.Y, offset.Z);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="Entity"/> from a world coordinates position.
		/// </summary>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			return ENTITY.GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS(Handle, worldCoords.X, worldCoords.Y, worldCoords.Z);
		}

		/// <summary>
		/// Gets or sets the velocity of this <see cref="Entity"/>.
		/// </summary>
		public Vector3 Velocity
		{
			get => ENTITY.GET_ENTITY_VELOCITY(Handle, -1);
			set => ENTITY.SET_ENTITY_VELOCITY(Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Gets or sets this <see cref="Entity"/>s speed.
		/// </summary>
		/// <value>
		/// The speed in m/s.
		/// </value>
		public float Speed
		{
			get => ENTITY.GET_ENTITY_SPEED(Handle);
			set => Velocity = Velocity.Normalized * value;
		}

		#endregion

		#region Invincibility & Damage

		/// <summary>
		/// Sets a value indicating whether this <see cref="Entity"/> is invincible.
		/// </summary>
		public bool IsInvincible
		{
			set => ENTITY.SET_ENTITY_INVINCIBLE(Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> can only be damaged by <see cref="Player"/>s.
		/// </summary>
		public bool IsOnlyDamagedByPlayer
		{
			set => ENTITY.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER(Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> can be damaged.
		/// </summary>
		public bool CanBeDamaged
		{
			set => ENTITY.SET_ENTITY_CAN_BE_DAMAGED(Handle, value);
			get => ENTITY._GET_ENTITY_CAN_BE_DAMAGED(Handle);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is bullet proof.
		/// </summary>
		public bool IsBulletProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 1) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 1, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is flame proof.
		/// </summary>
		public bool IsFlameProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 2) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 2, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is explosion proof.
		/// </summary>
		public bool IsExplosionProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 4) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 4, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is collision proof.
		/// </summary>
		public bool IsCollisionProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 8) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 8, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is melee proof.
		/// </summary>
		public bool IsMeleeProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 16) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 16, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is steam proof.
		/// </summary>
		public bool IsSteamProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 32) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 32, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is smoke proof.
		/// </summary>
		public bool IsSmokeProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 64) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 64, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is headshot proof.
		/// </summary>
		public bool IsHeadshotProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 128) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 128, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> is projectile proof.
		/// </summary>
		public bool IsProjectileProof
		{
			get => (ENTITY._GET_ENTITY_PROOFS(Handle) & 256) != 0;
			set => ENTITY.SET_ENTITY_PROOFS(Handle, 256, value);
		}

		#endregion

		#region Collision

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has collision.
		/// </summary>
		public bool HasCollision
		{
			get => ENTITY.GET_ENTITY_COLLISION_DISABLED(Handle);
			set => ENTITY.SET_ENTITY_COLLISION(Handle, value, false);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has collision.
		/// <remarks>This is an alias for <see cref="HasCollision"/></remarks>
		/// </summary>
		public bool IsCollisionEnabled
		{
			get => HasCollision;
			set => HasCollision = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Entity"/> has recently collided with anything.
		/// </summary>
		public bool HasCollidedWithAnything => ENTITY.HAS_ENTITY_COLLIDED_WITH_ANYTHING(Handle);

		/// <summary>
		/// Sets the collision between this <see cref="Entity"/> and another <see cref="Entity"/>
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to set collision with</param>
		/// <param name="toggle">if set to <see langword="true" /> the 2 <see cref="Entity"/>s wont collide with each other.</param>
		public void SetNoCollision(Entity entity, bool toggle)
		{
			ENTITY.SET_ENTITY_NO_COLLISION_ENTITY(Handle, entity.Handle, !toggle);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified area
		/// </summary>
		public bool IsInArea(Vector3 minBounds, Vector3 maxBounds)
		{
			return ENTITY.IS_ENTITY_IN_AREA(Handle, minBounds.X, minBounds.Y, minBounds.Z, maxBounds.X, maxBounds.Y, maxBounds.Z, false, true, 0);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area
		/// </summary>
		public bool IsInArea(Vector3 pos1, Vector3 pos2, float angle)
		{
			return IsInAngledArea(pos1, pos2, angle);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in a specified angled area
		/// </summary>
		public bool IsInAngledArea(Vector3 origin, Vector3 edge, float angle)
		{
			return ENTITY.IS_ENTITY_IN_ANGLED_AREA(Handle, origin.X, origin.Y, origin.Z, edge.X, edge.Y, edge.Z, angle, false, true, 0);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is in range of a specified position
		/// </summary>
		public bool IsInRangeOf(Vector3 position, float distance)
		{
			return Vector3.Subtract(Position, position).Length() < distance;
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is near a specified <see cref="Entity"/>.
		/// </summary>
		public bool IsNearEntity(Entity entity, Vector3 distance)
		{
			return ENTITY.IS_ENTITY_AT_ENTITY(Handle, entity.Handle, distance.X, distance.Y, distance.Z, false, true, 0);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is near a specified <see cref="Entity"/>.
		/// <remarks>This is an alias for <see cref="IsNearEntity(Entity, Vector3)"/></remarks>
		/// </summary>
		public bool IsAtEntity(Entity entity, Vector3 distance)
		{
			return ENTITY.IS_ENTITY_AT_ENTITY(Handle, entity.Handle, distance.X, distance.Y, distance.Z, false, true, 0);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is touching an <see cref="Entity"/> with the <see cref="Model"/> <paramref name="model"/>.
		/// </summary>
		public bool IsTouching(Model model)
		{
			return ENTITY.IS_ENTITY_TOUCHING_MODEL(Handle, (uint)model.Hash);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is touching the <see cref="Entity"/> <paramref name="entity"/>.
		/// </summary>
		public bool IsTouching(Entity entity)
		{
			return ENTITY.IS_ENTITY_TOUCHING_ENTITY(Handle, entity.Handle);
		}

		#endregion

		#region Blips

		/// <summary>
		/// Creates a <see cref="Blip"/> on this <see cref="Entity"/>.
		/// </summary>
		public Blip AddBlip(BlipType blipType)
		{
			return new Blip(MAP.BLIP_ADD_FOR_ENTITY((uint)blipType, Handle));
		}

		/// <summary>
		/// Gets the <see cref="Blip"/> on this <see cref="Entity"/>.
		/// </summary>
		public Blip GetBlip => new Blip(MAP.GET_BLIP_FROM_ENTITY(Handle));

		#endregion

		#region Attaching

		/// <summary>
		/// Detaches this <see cref="Entity"/> from any <see cref="Entity"/> it may be attached to.
		/// </summary>
		public void Detach()
		{
			ENTITY.DETACH_ENTITY(Handle, true, true);
		}

		/// <summary>
		/// Attach this <see cref="Entity"/> to another <see cref="Entity"/>
		/// </summary>
		public void AttachTo(Entity entity, int boneIndex)
		{
			AttachTo(entity, boneIndex, Vector3.Zero, Vector3.Zero);
		}

		/// <summary>
		/// Attach this <see cref="Entity"/> to another <see cref="Entity"/>
		/// </summary>
		public void AttachTo(Entity entity, int boneIndex, Vector3 position, Vector3 rotation)
		{
			ENTITY.ATTACH_ENTITY_TO_ENTITY(Handle, entity.Handle, boneIndex, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, false, false, false, false, 2, true, false, false);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to any other <see cref="Entity"/>.
		/// </summary>
		public bool IsAttached()
		{
			return ENTITY.IS_ENTITY_ATTACHED(Handle);
		}

		/// <summary>
		/// Determines whether this <see cref="Entity"/> is attached to the specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to check if this <see cref="Entity"/> is attached to.</param>
		/// <returns>
		///   <see langword="true" /> if this <see cref="Entity"/> is attached to <paramref name="entity"/>; otherwise, <see langword="false" />.
		/// </returns>
		public bool IsAttachedTo(Entity entity)
		{
			return ENTITY.IS_ENTITY_ATTACHED_TO_ENTITY(Handle, entity.Handle);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Entity"/> is attached to.
		/// </summary>
		public Entity GetEntityAttachedTo()
		{
			return Entity.FromHandle(ENTITY.GET_ENTITY_ATTACHED_TO(Handle));
		}

		#endregion

		#region Forces

		public void ApplyForce(Vector3 direction)
		{
			ApplyForce(direction, Vector3.Zero, 1);
		}
		public void ApplyForce(Vector3 direction, Vector3 rotation)
		{
			ApplyForce(direction, rotation, 1);
		}
		public void ApplyForce(Vector3 direction, Vector3 rotation, int forceType)
		{
			ENTITY.APPLY_FORCE_TO_ENTITY(Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, false, true, true, false, true);
		}
		public void ApplyForceRelative(Vector3 direction)
		{
			ApplyForceRelative(direction, Vector3.Zero, 1);
		}
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation)
		{
			ApplyForceRelative(direction, Rotation, 1);
		}
		public void ApplyForceRelative(Vector3 direction, Vector3 rotation, int forceType)
		{
			ENTITY.APPLY_FORCE_TO_ENTITY(Handle, (int)forceType, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, true, true, true, false, true);
		}

		#endregion

		/// <summary>
		/// Instantly delete this <see cref="Entity"/> from the game world.
		/// </summary>
		public override void Delete()
		{
			int handle = Handle;

			// Request ownership of entity if we do not have it
			if (!IsOwnedByThisScript) {
				this.RequestOwnership();
			}
			
			unsafe { ENTITY.DELETE_ENTITY(&handle); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Entity"/> exists in the game world.
		/// </summary>
		public override bool Exists()
		{
			return ENTITY.DOES_ENTITY_EXIST(Handle);
		}

		/// <summary>
		/// Gets a value indicating whether an <see cref="Entity"/> is not <see langword="null"/>, and exists in the game world.
		/// </summary>
		/// <returns><see langword="true"/> if <see cref="Entity"/> is not <see langword="null"/> and exists in the game world; otherwise, <see langword="false"/>.</returns>
		public static bool Exists(Entity entity)
		{
			return entity != null && entity.Exists();
		}

		public bool Equals(Entity obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Entity)obj);
		}

		public static bool operator ==(Entity left, Entity right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Entity left, Entity right)
		{
			return !(left == right);
		}

		public static implicit operator Entity(int handle) => Entity.FromHandle(handle);
		public static implicit operator int(Entity e) => e.Handle;

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
