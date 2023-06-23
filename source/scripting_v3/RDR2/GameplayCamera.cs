//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;

namespace RDR2
{
	public static class GameplayCamera
	{
		/// <summary>
		/// Gets the field of view of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float FieldOfView => CAM.GET_GAMEPLAY_CAM_FOV();

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is rendering.
		/// </summary>
		public static bool IsRendering => CAM.IS_GAMEPLAY_CAM_RENDERING();

		/// <summary>
		/// Gets a value indicating whether the aiming camera is rendering.
		/// </summary>
		public static bool IsAimCamActive => CAM.IS_AIM_CAM_ACTIVE();

		/// <summary>
		/// Gets a value indicating whether the first person aiming camera is rendering.
		/// </summary>
		public static bool IsFirstPersonAimCamActive => CAM.IS_FIRST_PERSON_AIM_CAM_ACTIVE();

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is looking behind.
		/// </summary>
		public static bool IsLookingBehind => CAM.IS_GAMEPLAY_CAM_LOOKING_BEHIND();

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is shaking.
		/// </summary>
		public static bool IsShaking => CAM.IS_GAMEPLAY_CAM_SHAKING();

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is in first person.
		/// </summary>
		public static bool IsInFirstPerson => CAM._IS_IN_FULL_FIRST_PERSON_MODE();

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is in third person.
		/// </summary>
		public static bool IsInThirdPerson => !IsInFirstPerson;

		/// <summary>
		/// Set the zoom of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float Zoom
		{
			set => CAM.SET_THIRD_PERSON_CAM_ORBIT_DISTANCE_LIMITS_THIS_UPDATE(1.0f, value);
		}

		/// <summary>
		/// Gets the position of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Vector3 Position => CAM.GET_GAMEPLAY_CAM_COORD();

		/// <summary>
		/// Gets the rotation of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Vector3 Rotation => CAM.GET_GAMEPLAY_CAM_ROT(2);

		/// <summary>
		/// Gets the direction the <see cref="GameplayCamera"/> is pointing in.
		/// </summary>
		public static Vector3 Direction
		{
			get
			{
				Vector3 rot = Rotation;
				double rotX = rot.X / 57.295779513082320876798154814105;
				double rotZ = rot.Z / 57.295779513082320876798154814105;
				double multXY = System.Math.Abs(System.Math.Cos(rotX));
				
				return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)System.Math.Sin(rotX));
			}
		}

		/// <summary>
		/// Gets the forward vector of the <see cref="GameplayCamera"/>, see also <seealso cref="Direction"/>.
		/// </summary>
		public static Vector3 ForwardVector => Direction;

		public static Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			return Position + (Right * offset.X) + (Forward * offset.Y) + (Up * offset.Z);
		}

		public static Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			Vector3 Delta = worldCoords - Position;
			return new Vector3(Vector3.Dot(Right, Delta), Vector3.Dot(Forward, Delta), Vector3.Dot(Up, Delta));
		}

		/// <summary>
		/// Clamps the yaw of the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="min">The minimum yaw value.</param>
		/// <param name="max">The maximum yaw value.</param>
		public static void ClampYaw(float min, float max)
		{
			CAM.SET_THIRD_PERSON_CAM_RELATIVE_HEADING_LIMITS_THIS_UPDATE(min, max);
		}

		/// <summary>
		/// Clamps the pitch of the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="min">The minimum pitch value.</param>
		/// <param name="max">The maximum pitch value.</param>
		public static void ClampPitch(float min, float max)
		{
			CAM.SET_THIRD_PERSON_CAM_RELATIVE_PITCH_LIMITS_THIS_UPDATE(min, max);
		}

		/// <summary>
		/// Gets or sets the relative pitch of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float RelativePitch
		{
			get => CAM.GET_GAMEPLAY_CAM_RELATIVE_PITCH();
			set => CAM.SET_GAMEPLAY_CAM_RELATIVE_PITCH(value, 1.0f);
		}

		/// <summary>
		/// Gets or sets the relative heading of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float RelativeHeading
		{
			get => CAM.GET_GAMEPLAY_CAM_RELATIVE_HEADING();
			set => CAM.SET_GAMEPLAY_CAM_RELATIVE_HEADING(value, 1.0f);
		}
	}
}
