using RDR2.Native;
using RDR2.Math;

namespace RDR2
{
	public sealed class Volume : PoolObject, ISpatial
	{
		public Volume(int handle) : base(handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Volume"/>.
		/// </summary>
		public Vector3 Position
		{
			get => VOLUME.GET_VOLUME_COORDS(Handle);
			set => VOLUME.SET_VOLUME_COORDS(Handle, value);
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Volume"/>.
		/// </summary>
		public Vector3 Rotation
		{
			get => VOLUME.GET_VOLUME_ROTATION(Handle);
			set => VOLUME.SET_VOLUME_ROTATION(Handle, value);
		}

		/// <summary>
		/// Gets or sets the scale of this <see cref="Volume"/>.
		/// </summary>
		public Vector3 Scale
		{
			get => VOLUME.GET_VOLUME_SCALE(Handle);
			set => VOLUME.SET_VOLUME_SCALE(Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Volume"/> is aggregate.
		/// </summary>
		public bool IsAggregateVolume => VOLUME._IS_AGGREGATE_VOLUME(Handle);

		/// <summary>
		/// Gets or sets the relationship group hash of this <see cref="Volume"/>.
		/// </summary>
		public uint RelationshipGroup
		{
			get => VOLUME._GET_VOLUME_RELATIONSHIP(Handle);
			set => VOLUME._SET_VOLUME_RELATIONSHIP(Handle, value);
		}


		public override void Delete()
		{
			VOLUME.DELETE_VOLUME(Handle);
		}

		public override bool Exists()
		{
			return VOLUME.DOES_VOLUME_EXIST(Handle);
		}

		public static bool Exists(Volume vol)
		{
			return vol != null && vol.Exists();
		}

		public bool Equals(Volume vol)
		{
			return !(vol is null) && Handle == vol.Handle;
		}

		public override bool Equals(object vol)
		{
			return !(vol is null) && vol.GetType() == GetType() && Equals((Volume)vol);
		}

		public static bool operator ==(Volume left, Volume right)
		{
			return left is null ? right is null : left.Equals(right);
		}

		public static bool operator !=(Volume left, Volume right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
