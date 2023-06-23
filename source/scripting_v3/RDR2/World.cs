#define CPP_SCRIPTHOOKRDR_V2
#undef CPP_SCRIPTHOOKRDR_V2 // Comment this out if you are using keps C++ ScriptHookRDR V2

using RDR2.Math;
using RDR2.Native;
using System;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Security;

namespace RDR2
{
	public static class World
	{
		#region Fields
		static readonly GregorianCalendar calendar = new GregorianCalendar();
		#endregion

		#region Time & Day

		public static DateTime CurrentDate
		{
			get
			{
				int year = CLOCK.GET_CLOCK_YEAR();
				int month = CLOCK.GET_CLOCK_MONTH();
				int day = System.Math.Min(CLOCK.GET_CLOCK_DAY_OF_MONTH(), calendar.GetDaysInMonth(year, month));
				int hour = CLOCK.GET_CLOCK_HOURS();
				int minute = CLOCK.GET_CLOCK_MINUTES();
				int second = CLOCK.GET_CLOCK_SECONDS();

				return new DateTime(year, month, day, hour, minute, second);
			}
			set
			{
				CLOCK.SET_CLOCK_DATE(value.Day, value.Month, value.Year);
				CLOCK.SET_CLOCK_TIME(value.Hour, value.Minute, value.Second);
			}
		}

		public static TimeSpan CurrentDayTime
		{
			get
			{
				int hours = CLOCK.GET_CLOCK_HOURS();
				int minutes = CLOCK.GET_CLOCK_MINUTES();
				int seconds = CLOCK.GET_CLOCK_SECONDS();

				return new TimeSpan(hours, minutes, seconds);
			}
			set => CLOCK.SET_CLOCK_TIME(value.Hours, value.Minutes, value.Seconds);
		}

		#endregion

		#region Weather & Effects


		private static WeatherType _currentWeather;
		public static WeatherType CurrentWeather
		{
			get => GetCurrentWeatherType();
			set
			{
				_currentWeather = value;
				MISC.SET_CURR_WEATHER_STATE((uint)GetCurrentWeatherType(), (uint)value, 1f, true);
			}
		}

		private static WeatherType _nextWeather;
		public static WeatherType NextWeather
		{
			get
			{
				GetCurrentWeatherType();
				return _nextWeather;
			}
		}

		private static WeatherType GetCurrentWeatherType()
		{
			uint currentWeather;
			uint nextWeather;
			float percent;
			unsafe { MISC.GET_CURR_WEATHER_STATE(&currentWeather, &nextWeather, &percent); }
			_currentWeather = (WeatherType)currentWeather;
			_nextWeather = (WeatherType)nextWeather;
			if (percent >= 0.5f)
			{
				return _nextWeather;
			}
			return _currentWeather;
		}


		public static float WeatherTransition
		{
			get
			{
				uint currentWeatherHash, nextWeatherHash;
				float weatherTransition;
				unsafe
				{
					MISC.GET_CURR_WEATHER_STATE(&currentWeatherHash, &nextWeatherHash, &weatherTransition);
				}
				return weatherTransition;
			}
			set => MISC.SET_CURR_WEATHER_STATE(0, 0, value, true);
		}


		public static void SetSnowCoverageType(eSnowCoverageType type)
		{
			GRAPHICS._SET_SNOW_COVERAGE_TYPE((int)type);
		}


		/*public static void SetCascadeShadowType(string type)
		{

		}

		public static void ClearCascadeShadowType()
		{

		}*/

		#endregion

		#region Blips
		public static bool IsWaypointActive => MAP.IS_WAYPOINT_ACTIVE();

		public static Vector3 WaypointPosition => MAP._GET_WAYPOINT_COORDS();

		public static Blip CreateBlip(Vector3 position, BlipType type)
		{
			var blip = MAP.BLIP_ADD_FOR_COORDS((uint)type, position.X, position.Y, position.Z);
			return new Blip(blip);
		}
		public static Blip CreateBlip(Vector3 position, BlipType type, float radius)
		{
			return new Blip(MAP.BLIP_ADD_FOR_RADIUS((uint)type, position.X, position.Y, position.Z, radius));
		}
		#endregion

		#region Entities

		/// <summary>
		/// Gets all of the <see cref="Ped"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Ped"/>s found</returns>
		/// <remarks>Note: This function can return <see cref="Array.Empty{T}"/> if the internal call to worldGetAllPeds() failed</remarks>
		[HandleProcessCorruptedStateExceptions]
		public static Ped[] GetAllPeds()
		{
			int[] peds = new int[1024];
			int count = 0;

			// So for some reason, ScriptHookRDR2 likes to error at random when accessing pools so
			// we'll wrap this in a try catch and return empty if the call fails and prevent a crash.
			// https://github.com/Halen84/ScriptHookRDR2DotNet-V2/issues/2
			try
			{
				count = RDR2DN.NativeMemory.worldGetAllPeds(peds, 1024);
			}
			catch
			{
				return Array.Empty<Ped>();
			}

			List<Ped> Peds = new List<Ped>();
			for (int i = 0; i < count; i++)
				Peds.Add(new Ped(peds[i]));

			return Peds.ToArray();
		}

		/// <summary>
		/// Gets all of the <see cref="Vehicle"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Vehicle"/>s found</returns>
		/// <remarks>Note: This function can return <see cref="Array.Empty{T}"/> if the internal call to worldGetAllVehicles() failed</remarks>
		[HandleProcessCorruptedStateExceptions]
		public static Vehicle[] GetAllVehicles()
		{
			int[] vehs = new int[1024];
			int count = 0;

			try
			{
				count = RDR2DN.NativeMemory.worldGetAllVehicles(vehs, 1024);
			}
			catch
			{
				return Array.Empty<Vehicle>();
			}

			List<Vehicle> Vehs = new List<Vehicle>();
			for (int i = 0; i < count; i++)
				Vehs.Add(new Vehicle(vehs[i]));

			return Vehs.ToArray();
		}

		/// <summary>
		/// Gets all of the <see cref="Prop"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Prop"/>s found</returns>
		/// <remarks>Note: This function can return <see cref="Array.Empty{T}"/> if the internal call to worldGetAllObjects() failed</remarks>
		[HandleProcessCorruptedStateExceptions]
		public static Prop[] GetAllObjects()
		{
			int[] props = new int[1024];
			int count = 0;

			try
			{
				count = RDR2DN.NativeMemory.worldGetAllObjects(props, 1024);
			}
			catch
			{
				return Array.Empty<Prop>();
			}

			List<Prop> Prop = new List<Prop>();
			for (int i = 0; i < count; i++)
				Prop.Add(new Prop(props[i]));

			return Prop.ToArray();
		}

#if CPP_SCRIPTHOOKRDR_V2

		/// <summary>
		/// Gets all of the <see cref="Blip"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Blips"/>s found</returns>
		public static Blip[] GetAllBlips()
		{
			int[] blips = new int[1024];

			// ScriptHookRDR2 V2 pools don't crash, so no need for a try catch
			int count = RDR2DN.NativeMemory.worldGetAllBlips(blips, 1024);

			List<Blip> blipsArr = new List<Blip>();
			for (int i = 0; i < count; i++)
				blipsArr.Add(new Blip(blips[i]));

			return blipsArr.ToArray();
		}

		/// <summary>
		/// Gets all of the <see cref="Camera"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Camera"/>s found</returns>
		public static Camera[] GetAllCams()
		{
			int[] cams = new int[1024];
			int count = RDR2DN.NativeMemory.worldGetAllCams(cams, 1024);

			List<Camera> cameras = new List<Camera>();
			for (int i = 0; i < count; i++)
				cameras.Add(new Camera(cams[i]));

			return cameras.ToArray();
		}

		/// <summary>
		/// Gets all of the <see cref="Volume"/>s currently spawned/loaded in the game world
		/// </summary>
		/// <returns>An <see cref="Array"/> of all <see cref="Volume"/>s found</returns>
		public static int[] GetAllVolumes()
		{
			int[] vols = new int[1024];
			int count = RDR2DN.NativeMemory.worldGetAllVolumes(vols, 1024);

			List<Volume> volumes = new List<Volume>();
			for (int i = 0; i < count; i++)
				volumes.Add(new Volume(vols[i]));

			return volumes.ToArray();
		}

#endif //CPP_SCRIPTHOOKRDR_V2

		public static T GetClosest<T>(Vector3 position, params T[] spatials)
			where T : ISpatial
		{
			ISpatial closest = null;
			float closestDistance = 3e38f;

			foreach (var spatial in spatials)
			{
				float distance = position.DistanceToSquared(spatial.Position);

				if (distance <= closestDistance)
				{
					closest = spatial;
					closestDistance = distance;
				}
			}
			return (T)closest;
		}

		public static Ped GetClosestPed(Vector3 position)
		{
			Ped[] peds = GetAllPeds();
			return GetClosest(position, peds);
		}

		public static Vehicle GetClosestVehicle(Vector3 position)
		{
			Vehicle[] vehicles = GetAllVehicles();
			return GetClosest(position, vehicles);
		}

		public static Prop GetClosestObject(Vector3 position)
		{
			Prop[] objects = GetAllObjects();
			return GetClosest(position, objects);
		}

		public static Ped CreatePed(PedHash hash, Vector3 position, float heading = 0f)
		{
			var model = new Model(hash);
			if (!model.Request(4000))
			{
				return null;
			}
			int ped = PED.CREATE_PED((uint)hash, position.X, position.Y, position.Z, heading, true, true, false, false);
			PED._SET_RANDOM_OUTFIT_VARIATION(ped, true);
			PED._UPDATE_PED_VARIATION(ped, false, true, true, true, false);
			return ped == 0 ? null : (Ped)Entity.FromHandle(ped);
		}

		public static Vehicle CreateVehicle(VehicleHash hash, Vector3 position, float heading = 0f)
		{
			var model = new Model((uint)hash);
			if (!model.Request(4000))
			{
				return null;
			}
			return new Vehicle(VEHICLE.CREATE_VEHICLE((uint)hash, position.X, position.Y, position.Z, heading, true, true, false, false));
		}

		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
		/// <param name="dynamic">if set to <c>true</c> the <see cref="Prop"/> will have physics; otherwise, it will be static.</param>
		/// <param name="placeOnGround">if set to <c>true</c> place the prop on the ground nearest to the <paramref name="position"/>.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Prop"/> could not be spawned</remarks>
		public static Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround)
		{
			if (!model.Request(1000))
			{
				return null;
			}

			if (placeOnGround)
			{
				position.Z = GetGroundZ(position);
			}

			return new Prop(OBJECT.CREATE_OBJECT((uint)model.Hash, position.X, position.Y, position.Z, true, true, dynamic, false, true));
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
		/// <param name="rotation">The rotation of the <see cref="Prop"/>.</param>
		/// <param name="dynamic">if set to <c>true</c> the <see cref="Prop"/> will have physics; otherwise, it will be static.</param>
		/// <param name="placeOnGround">if set to <c>true</c> place the prop on the ground nearest to the <paramref name="position"/>.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Prop"/> could not be spawned</remarks>
		public static Prop CreateProp(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround)
		{
			Prop prop = CreateProp(model, position, dynamic, placeOnGround);

			if (prop != null)
			{
				prop.Rotation = rotation;
			}

			return prop;
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the position specified without any offset.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
		/// <param name="dynamic">if set to <c>true</c> the <see cref="Prop"/> will have physics; otherwise, it will be static.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Prop"/> could not be spawned</remarks>
		public static Prop CreatePropNoOffset(Model model, Vector3 position, bool dynamic)
		{
			if (!model.Request(1000))
			{
				return null;
			}

			return new Prop(OBJECT.CREATE_OBJECT_NO_OFFSET((uint)model.Hash, position.X, position.Y, position.Z, true, true, dynamic, false));
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the position specified without any offset.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
		/// <param name="rotation">The rotation of the <see cref="Prop"/>.</param>
		/// <param name="dynamic">if set to <c>true</c> the <see cref="Prop"/> will have physics; otherwise, it will be static.</param>
		/// <remarks>returns <c>null</c> if the <see cref="Prop"/> could not be spawned</remarks>
		public static Prop CreatePropNoOffset(Model model, Vector3 position, Vector3 rotation, bool dynamic)
		{
			Prop prop = CreatePropNoOffset(model, position, dynamic);

			if (prop != null)
			{
				prop.Rotation = rotation;
			}

			return prop;
		}
		public static Pickup CreatePickup(PickupType type, Vector3 pos)
		{
			var handle = OBJECT.CREATE_PICKUP((uint)type, pos.X, pos.Y, pos.Z, 32770, 0, true, 0, 0, 0.0f, 0);
			return new Pickup(handle);
		}

		public static void SetAmbientRoadPopulationEnabled(bool enabled)
		{
			if (enabled)
			{
				POPULATION.ENABLE_AMBIENT_ROAD_POPULATION();
			}
			else
			{
				POPULATION.DISABLE_AMBIENT_ROAD_POPULATION(true);
			}
		}
		#endregion

		#region Cameras

		public static void DestroyAllCameras()
		{
			CAM.DESTROY_ALL_CAMS(false);
		}

		public static Camera CreateCamera(Vector3 position, Vector3 rotation, float fov)
		{
			return new Camera(CAM.CREATE_CAM_WITH_PARAMS("DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, true, 2));
		}

		public static Camera RenderingCamera
		{
			get => new Camera(CAM.GET_RENDERING_CAM());
			set
			{
				if (value == null)
				{
					CAM.RENDER_SCRIPT_CAMS(false, false, 3000, true, false, 0);
				}
				else
				{
					value.Active = true;
					CAM.RENDER_SCRIPT_CAMS(true, false, 3000, true, false, 0);
				}
			}
		}

		#endregion

		#region Others


		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage)
		{
			ShootBullet(sourcePosition, targetPosition, owner, model, damage, -1.0f);
		}
		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage, float speed)
		{
			MISC.SHOOT_SINGLE_BULLET_BETWEEN_COORDS(sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, true, (uint)model.Hash, owner.Handle, true, false, speed, false);
		}

		public static void AddExplosion(Vector3 position, int type, float radius, float cameraShake)
		{
			FIRE.ADD_EXPLOSION(position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		public static void AddExplosion(Vector3 position, int type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			FIRE.ADD_EXPLOSION(position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}
		public static void AddOwnedExplosion(Ped ped, Vector3 position, int type, float radius, float cameraShake)
		{
			FIRE.ADD_OWNED_EXPLOSION(ped.Handle, position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		public static void AddOwnedExplosion(Ped ped, Vector3 position, int type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			FIRE.ADD_OWNED_EXPLOSION(ped.Handle, position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}

		public static uint AddRelationshipGroup(string groupName)
		{
			uint handle = 0;
			unsafe
			{
				PED.ADD_RELATIONSHIP_GROUP(groupName, &handle);
			};
			return handle;
		}
		public static void RemoveRelationshipGroup(uint group)
		{
			PED.REMOVE_RELATIONSHIP_GROUP(group);
		}
		public static eRelationshipType GetRelationshipBetweenGroups(uint group1, uint group2)
		{
			return (eRelationshipType)PED.GET_RELATIONSHIP_BETWEEN_GROUPS(group1, group2);
		}
		public static void SetRelationshipBetweenGroups(eRelationshipType relationship, uint group1, uint group2)
		{
			PED.SET_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, group1, group2);
			PED.SET_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, group2, group1);
		}
		public static void ClearRelationshipBetweenGroups(eRelationshipType relationship, uint group1, uint group2)
		{
			PED.CLEAR_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, group1, group2);
			PED.CLEAR_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, group2, group1);
		}

		#endregion

		#region Drawing


		public static void DrawLight(Vector3 position, Color color, float range, float brightness)
		{
			GRAPHICS.DRAW_LIGHT_WITH_RANGE(position.X, position.Y, position.Z, color.R, color.G, color.B, range, brightness);
		}

		/// <summary>
		/// Draws a marker in the world, this needs to be done on a per frame basis
		/// </summary>
		/// <param name="type">The type of marker.</param>
		/// <param name="pos">The position of the marker.</param>
		/// <param name="dir">The direction the marker points in.</param>
		/// <param name="rot">The rotation of the marker.</param>
		/// <param name="scale">The amount to scale the marker by.</param>
		/// <param name="color">The color of the marker.</param>
		/// <param name="bobUpAndDown">if set to <c>true</c> the marker will bob up and down.</param>
		/// <param name="faceCamera">if set to <c>true</c> the marker will always face the camera, regardless of its rotation.</param>
		/// <param name="rotateY">if set to <c>true</c> rotates only on the y axis(heading).</param>
		/// <param name="textueDict">Name of texture dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="textureName">Name of texture inside the dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="drawOnEntity">if set to <c>true</c> draw on any <see cref="Entity"/> that intersects the marker.</param>
		public static void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color,
		 bool bobUpAndDown = false, bool faceCamera = false, bool rotateY = false, string textueDict = "", string textureName = "", bool drawOnEntity = false)
		{
			if (!string.IsNullOrEmpty(textueDict) && !string.IsNullOrEmpty(textureName))
			{
				GRAPHICS._DRAW_MARKER((uint)type, pos, dir, rot, scale, color.R, color.G, color.B, color.A,
					bobUpAndDown, faceCamera, 2, rotateY, textueDict, textureName, drawOnEntity);
			}
			else
			{
				GRAPHICS._DRAW_MARKER((uint)type, pos, dir, rot, scale, color.R, color.G, color.B, color.A,
					bobUpAndDown, faceCamera, 2, rotateY, "", "", drawOnEntity);
			}
		}
		#endregion

		#region Positioning

		public static float GetDistance(Vector3 origin, Vector3 destination)
		{
			return MISC.GET_DISTANCE_BETWEEN_COORDS(origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, true);
		}
		/*public static float CalculateTravelDistance(Vector3 origin, Vector3 destination)
		{
			return Function.Call<float>(Hash.CALCULATE_TRAVEL_DISTANCE_BETWEEN_POINTS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z);
		}*/
		public static float GetGroundZ(Vector2 position)
		{
			return GetGroundZ(new Vector3(position.X, position.Y, 1000f));
		}
		public static float GetGroundZ(Vector3 position)
		{
			float groundZ;

			unsafe
			{
				MISC.GET_GROUND_Z_FOR_3D_COORD(position.X, position.Y, position.Z, &groundZ, false);
			}

			return groundZ;
		}

		public static Vector3 GetSafeCoordForPed(Vector3 position)
		{
			return GetSafeCoordForPed(position, true, 0);
		}
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk)
		{
			return GetSafeCoordForPed(position, sidewalk, 0);
		}
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool onGround, int flags)
		{
			Vector3 outPos = Vector3.Zero;

			unsafe
			{
				if (PATHFIND.GET_SAFE_COORD_FOR_PED(position.X, position.Y, position.Z, onGround, &outPos, flags))
				{
					return outPos;
				}
			}

			return Vector3.Zero;
		}

		public static Vector3 GetNextPositionOnStreet(Vector3 position)
		{
			return GetNextPositionOnStreet(position, false);
		}
		public static Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied)
		{
			return GetNextPositionOnStreet(new Vector3(position.X, position.Y, 0), unoccupied);
		}
		public static Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied)
		{
			Vector3 outPos = Vector3.Zero;

			if (unoccupied)
			{
				for (int i = 1; i < 40; i++)
				{
					unsafe
					{
						if (PATHFIND.GET_NTH_CLOSEST_VEHICLE_NODE(position.X, position.Y, position.Z, i, &outPos, 1, 3.0f, 0))
						{
							return outPos;
						}
					}
				}
			}
			else
			{
				unsafe
				{
					if (PATHFIND.GET_NTH_CLOSEST_VEHICLE_NODE(position.X, position.Y, position.Z, 1, &outPos, 1, 3.0f, 0))
					{
						return outPos;
					}
				}
			}

			return Vector3.Zero;
		}

		public static Vector3 GetNextPositionOnSidewalk(Vector2 position)
		{
			return GetNextPositionOnSidewalk(new Vector3(position.X, position.Y, 0));
		}
		public static Vector3 GetNextPositionOnSidewalk(Vector3 position)
		{
			Vector3 outPos = Vector3.Zero;

			unsafe
			{
				if (PATHFIND.GET_SAFE_COORD_FOR_PED(position.X, position.Y, position.Z, true, &outPos, 0))
				{
					return outPos;
				}
				else if (PATHFIND.GET_SAFE_COORD_FOR_PED(position.X, position.Y, position.Z, false, &outPos, 0))
				{
					return outPos;
				}
			}

			return Vector3.Zero;
		}


		#endregion
	}

	public enum WeatherType : uint
	{
		Overcast = 0xBB898D2D,
		Rain = 0x54A69840,
		Fog = 0xD61BDE01,
		Snowlight = 0x23FB812B,
		Thunder = 0xB677829F,
		Blizzard = 0x27EA2814,
		Snow = 0xEFB6EFF6,
		Misty = 0x5974E8E5,
		Sunny = 0x614A1F91,
		HighPressure = 0xF5A87B65,
		Clearing = 0x6DB1A50D,
		Sleet = 0xCA71D7C,
		Drizzle = 0x995C7F44,
		Shower = 0xE72679D5,
		SnowClearing = 0x641DFC11,
		OvercastDark = 0x19D4F1D9,
		Thunderstorm = 0x7C1C4A13,
		Sandstorm = 0xB17F6111,
		Hurricane = 0x320D0951,
		Hail = 0x75A9E268,
		Whiteout = 0x2B402288,
		GroundBlizzard = 0x7F622122
	}

	public enum eSnowCoverageType
	{
		Primary,
		Secondary,
		Xmas,
		XmasSecondary,
	}
}
