using System;
using RDR2.Native;
using RDR2.Math;

namespace RDR2
{
	public static class Raycasting
	{
		public static RaycastResult Raycast(Vector3 source, Vector3 dest, RaycastLOSOptions losFlags = RaycastLOSOptions.Map, Entity entityToIgnore = null, RaycastOptions options = RaycastOptions.Default)
		{
			return new RaycastResult(SHAPETEST.START_SHAPE_TEST_LOS_PROBE(source.X, source.Y, source.Z,
				dest.X, dest.Y, dest.Z, (int)losFlags, entityToIgnore == null ? 0 : entityToIgnore.Handle, (int)options));
		}

		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDist, RaycastLOSOptions losFlags = RaycastLOSOptions.Map, Entity entityToIgnore = null, RaycastOptions options = RaycastOptions.Default)
		{
			var target = source + direction * maxDist;
			return new RaycastResult(SHAPETEST.START_SHAPE_TEST_LOS_PROBE(source.X, source.Y, source.Z,
				target.X, target.Y, target.Z, (int)losFlags, entityToIgnore == null ? 0 : entityToIgnore.Handle, (int)options));
		}

		public static RaycastResult CenterScreenRaycast(float maxDist, RaycastLOSOptions losFlags = RaycastLOSOptions.Map, Entity entityToIgnore = null)
		{
			var source = GameplayCamera.Position;
			var rotation = (float)(System.Math.PI / 180.0) * GameplayCamera.Rotation;
			var forward = Vector3.Normalize(new Vector3(
				(float)-System.Math.Sin(rotation.Z) * (float)System.Math.Abs(System.Math.Cos(rotation.X)),
				(float)System.Math.Cos(rotation.Z) * (float)System.Math.Abs(System.Math.Cos(rotation.X)),
				(float)System.Math.Sin(rotation.X)));
			var target = source + forward * maxDist;
			return Raycast(source, target, losFlags, entityToIgnore);
		}
	}

	public struct RaycastResult
	{
		public Entity HitEntity { get; }
		public Vector3 HitPosition { get; }
		public Vector3 SurfaceNormal { get; }
		public bool DidHit { get; }
		public bool DidHitEntity { get; }
		public int Result { get; }

		public RaycastResult(int handle)
		{
			bool didHit = false;
			Vector3 hitCoords = Vector3.Zero;
			Vector3 surfaceNormal = Vector3.Zero;
			int entityHit = 0;

			unsafe { Result = SHAPETEST.GET_SHAPE_TEST_RESULT(handle, &didHit, &hitCoords, &surfaceNormal, &entityHit); }

			HitPosition = hitCoords;
			HitEntity = entityHit == 0 || HitPosition == default ? null : Entity.FromHandle(entityHit);
			DidHitEntity = HitEntity != null && HitPosition != default && HitEntity.EntityType != 0;
			DidHit = didHit;
			SurfaceNormal = surfaceNormal;
		}
	}

	[Flags]
	public enum RaycastLOSOptions
	{
		None = 0,
		Map = (1 << 0),
		Vehicles = (1 << 1),
		Peds = (1 << 2),
		Ragdoll = (1 << 3),
		Objects = (1 << 4),
		Pickups = (1 << 5),
		Glass = (1 << 6),
		Water = (1 << 7),
		Foilage = (1 << 8),
		All = 511, // 511 is how it's defined in script header
				   // Add this in case "All" doesn't actually work
		Everything = Map | Vehicles | Peds | Ragdoll | Objects | Pickups | Glass | Water | Foilage,
	}

	[Flags]
	public enum RaycastOptions
	{
		IgnoreGlass = (1 << 0),
		IgnoreSeeThrough = (1 << 1),
		IgnoreNoCollision = (1 << 2),
		Default = IgnoreGlass | IgnoreSeeThrough | IgnoreNoCollision,
	}
}
