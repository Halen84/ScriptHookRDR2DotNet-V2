//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RDR2
{
	public static class Game
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern System.IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(System.IntPtr hWnd, Rectangle rect);

		static private Player _cachedPlayer;


		/// <summary>
		/// Get the interval in seconds from the last frame to the current one
		/// </summary>
		public static float FrameTime => MISC.GET_FRAME_TIME();

		/// <summary>
		/// Get the interval in seconds from the last frame to the current one
		/// <remarks>This is an alias for <see cref="FrameTime"/></remarks>
		/// </summary>
		public static float DeltaTime => FrameTime;

		/// <summary>
		/// Gets the current frame rate in frames per second.
		/// </summary>
		public static float FPS => 1.0f / FrameTime;

		/// <summary>
		/// Gets how many milliseconds the game has been open in this session
		/// </summary>
		public static int GameTime => MISC.GET_GAME_TIMER();

		/// <summary>
		/// Gets the total number of frames that have been rendered in this session.
		/// </summary>
		public static int FrameCount => MISC.GET_FRAME_COUNT();

		/// <summary>
		/// Gets the game window screen resolution
		/// </summary>
		public static Size ScreenResolution
		{
			get
			{
				Rectangle rect = new Rectangle();
				GetWindowRect(GetForegroundWindow(), rect);
				return new Size(rect.Width, rect.Height);
			}
		}

		/// <summary>
		/// Gets the main <see cref="RDR2.Player"/> that you are controlling.
		/// </summary>
		public static Player Player
		{
			get {
				int handle = PLAYER.PLAYER_ID();

				if (_cachedPlayer == null || handle != _cachedPlayer.Handle)
				{
					_cachedPlayer = new Player(handle);
				}

				return _cachedPlayer;
			}
		}

		/// <summary>
		/// Gets the blip of the <see cref="RDR2.Player"/> that you are controlling.
		/// </summary>
		public static Blip PlayerBlip => new Blip(MAP.GET_MAIN_PLAYER_BLIP_ID());

		/// <summary>
		/// Gets the version of the game.
		/// </summary>
		public static GameVersion Version => (GameVersion)RDR2DN.NativeMemory.GetGameVersion();

		/// <summary>
		/// Get the games script globals as a collection
		/// </summary>
		public static GlobalCollection Globals { get; private set; } = new GlobalCollection();

		/// <summary>
		/// Gets an instance of a <see cref="Global"/> that holds data to a script global.
		/// <remarks>Make sure that you check the game version before accessing globals. ID's may differ between patches.</remarks>
		/// </summary>
		/// <param name="globalId">The script global index</param>
		/// <returns>An instance to a script <see cref="Global"/></returns>
		public static Global GetGlobalPtr(int globalId)
		{
			return Globals.Get(globalId);
		}

		/// <summary>
		/// Gets a value indicating whether a cutscene is currently playing.
		/// </summary>
		public static bool IsCutsceneActive => ANIMSCENE.DOES_ANIM_SCENE_EXIST((int)RDR2DN.NativeMemory.GetGlobalPtr(43800)) && ANIMSCENE.IS_ANIM_SCENE_RUNNING((int)RDR2DN.NativeMemory.GetGlobalPtr(43800), false);

		/// <summary>
		/// Gets a value indicating whether a cutscene is currently playing.
		/// <remarks>This is an alias for <see cref="IsCutsceneActive"/></remarks>
		/// </summary>
		public static bool InCutscene => IsCutsceneActive;

		/// <summary>
		/// Gets or sets a value indicating whether the pause menu is active.
		/// </summary>
		public static bool IsPaused
		{
			get => HUD.IS_PAUSE_MENU_ACTIVE();
			set => MISC.SET_GAME_PAUSED(value);
		}

		/// <summary>
		/// Pause/resume the game.
		/// </summary>
		public static void Pause(bool toggle)
		{
			MISC.SET_GAME_PAUSED(toggle);
		}

		/// <summary>
		/// Pause/resume the in-game clock
		/// </summary>
		public static void PauseClock(bool toggle)
		{
			CLOCK.PAUSE_CLOCK(toggle, 0);
		}

		/// <summary>
		/// Gets a value indicating whether there is a loading screen being displayed.
		/// </summary>
		public static bool IsLoading => DLC.GET_IS_LOADING_SCREEN_ACTIVE();

		/// <summary>
		/// Gets a value indicating whether the screen is currently faded in
		/// </summary>
		public static bool IsScreenFadedIn => CAM.IS_SCREEN_FADED_IN();

		/// <summary>
		/// Gets a value indicating whether the screen is currently faded out
		/// </summary>
		public static bool IsScreenFadedOut => CAM.IS_SCREEN_FADED_OUT();

		/// <summary>
		/// Gets a value indicating whether the screen is currently fading in
		/// </summary>
		public static bool IsScreenFadingIn => CAM.IS_SCREEN_FADING_IN();

		/// <summary>
		/// Gets a value indicating whether the screen is currently fading out
		/// </summary>
		public static bool IsScreenFadingOut => CAM.IS_SCREEN_FADING_OUT();

		/// <summary>
		/// Fade the screen in
		/// </summary>
		public static void FadeScreenIn(int duration)
		{
			CAM.DO_SCREEN_FADE_IN(duration);
		}

		/// <summary>
		/// Fade the screen out
		/// </summary>
		public static void FadeScreenOut(int duration)
		{
			CAM.DO_SCREEN_FADE_OUT(duration);
		}

		/// <summary>
		/// Gets a value indicating whether there is a waypoint set on the map.
		/// </summary>
		public static bool IsWaypointActive => MAP.IS_WAYPOINT_ACTIVE();

		/// <summary>
		/// Gets the time scale of the game.
		/// </summary>
		public static float TimeScale
		{
			set => MISC.SET_TIME_SCALE(value);
		}

		/// <summary>
		/// Set the zoom on the minimap/radar
		/// </summary>
		public static int RadarZoom
		{
			set => MAP.SET_RADAR_ZOOM(value);
		}

		/// <summary>
		/// Set the zoom on the minimap/radar
		/// <remarks>This is an alias for <see cref="RadarZoom"/></remarks>
		/// </summary>
		public static int MinimapZoom
		{
			set => RadarZoom = value;
		}

		/// <summary>
		/// Gets or sets a value informing the engine if a mission is in progress.
		/// </summary>
		public static bool IsMissionActive
		{
			get => MISC.GET_MISSION_FLAG();
			set => MISC.SET_MISSION_FLAG(value);
		}

		/// <summary>
		/// Gets or sets a value informing the engine if a mission is in progress.
		/// <remarks>This is an alias for <see cref="IsMissionActive"/></remarks>
		/// </summary>
		public static bool InMission
		{
			get => IsMissionActive;
			set => IsMissionActive = value;
		}

		/// <summary>
		/// Set whether police/lawmen blips should appear on the radar/minimap.
		/// </summary>
		public static bool ShowsPoliceBlipsOnRadar
		{
			set => PLAYER.SET_POLICE_RADAR_BLIPS(value);
		}

		#region User Input

		/// <summary>
		/// Creates an onscreen keyboard for the user to input text.
		/// </summary>
		public static string GetUserInput(int maxLength)
		{
			return GetUserInput("", "", maxLength);
		}

		/// <summary>
		/// Creates an onscreen keyboard for the user to input text.
		/// </summary>
		public static string GetUserInput(string windowTitle, int maxLength)
		{
			return GetUserInput(windowTitle, "", maxLength);
		}

		/// <summary>
		/// Creates an onscreen keyboard for the user to input text.
		/// </summary>
		public static string GetUserInput(string windowTitle, string defaultText, int maxLength)
		{
			RDR2DN.ScriptDomain.CurrentDomain.PauseKeyEvents(true);

			MISC.DISPLAY_ONSCREEN_KEYBOARD(0, windowTitle, "", defaultText, "", "", "", maxLength + 1);

			while (MISC.UPDATE_ONSCREEN_KEYBOARD() == 0)
			{
				Script.Yield();
			}

			RDR2DN.ScriptDomain.CurrentDomain.PauseKeyEvents(false);

			return MISC.GET_ONSCREEN_KEYBOARD_RESULT();
		}

		#endregion

		#region PAD Namespace Wrappers

		/// <summary>
		/// Gets whether the last input was made with a GamePad or keyboard and mouse.
		/// </summary>
		public static InputMethod LastInputMethod => PAD.IS_USING_KEYBOARD_AND_MOUSE(0) ? InputMethod.Keyboard : InputMethod.GamePad;

		/// <summary>
		/// Gets an analog value of a <see cref="eInputType"/> input.
		/// </summary>
		public static int GetControlValue(eInputType control)
		{
			return PAD.GET_CONTROL_VALUE(0, (uint)control);
		}

		/// <summary>
		/// Gets an analog value of a <see cref="eInputType"/> input between -1.0f and 1.0f.
		/// </summary>
		public static float GetControlNormal(eInputType control)
		{
			return PAD.GET_CONTROL_NORMAL(0, (uint)control);
		}

		/// <summary>
		/// Gets an analog value of a disabled <see cref="eInputType"/> input between -1.0f and 1.0f.
		/// </summary>
		public static float GetDisabledControlNormal(eInputType control)
		{
			return PAD.GET_DISABLED_CONTROL_NORMAL(0, (uint)control);
		}

		/// <summary>
		/// Override a <see cref="eInputType"/> by giving it a user-defined value for this frame.
		/// </summary>
		public static void SetControlNormal(eInputType control, float value)
		{
			PAD.SET_CONTROL_VALUE_NEXT_FRAME(0, (uint)control, value);
		}

		/// <summary>
		/// Gets whether a <see cref="eInputType"/> control is currently enabled.
		/// </summary>
		public static bool IsControlEnabled(eInputType control)
		{
			return PAD.IS_CONTROL_ENABLED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a enabled <see cref="eInputType"/> control is currently pressed.
		/// </summary>
		public static bool IsControlPressed(eInputType control)
		{
			return PAD.IS_CONTROL_PRESSED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a enabled <see cref="eInputType"/> control was just pressed.
		/// </summary>
		public static bool IsControlJustPressed(eInputType control)
		{
			return PAD.IS_CONTROL_JUST_PRESSED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a enabled <see cref="eInputType"/> control was just released.
		/// </summary>
		public static bool IsControlJustReleased(eInputType control)
		{
			return PAD.IS_CONTROL_JUST_RELEASED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a enabled <see cref="eInputType"/> control is not pressed.
		/// </summary>
		public static bool IsControlReleased(eInputType control)
		{
			return PAD.IS_CONTROL_RELEASED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a disabled <see cref="eInputType"/> control is currently pressed.
		/// </summary>
		public static bool IsDisabledControlPressed(eInputType control)
		{
			return PAD.IS_DISABLED_CONTROL_PRESSED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a disabled <see cref="eInputType"/> control was just pressed.
		/// </summary>
		public static bool IsDisabledControlJustPressed(eInputType control)
		{
			return PAD.IS_DISABLED_CONTROL_JUST_PRESSED(0, (uint)control);
		}

		/// <summary>
		/// Gets whether a disabled <see cref="eInputType"/> control was just released.
		/// </summary>
		public static bool IsDisabledControlJustReleased(eInputType control)
		{
			return PAD.IS_DISABLED_CONTROL_JUST_RELEASED(0, (uint)control);
		}

		/// <summary>
		/// Enable a given <see cref="eInputType"/> control for one frame
		/// </summary>
		public static void EnableControlThisFrame(eInputType control)
		{
			PAD.ENABLE_CONTROL_ACTION(0, (uint)control, true);
		}

		/// <summary>
		/// Disable a given <see cref="eInputType"/> control for one frame
		/// </summary>
		public static void DisableControlThisFrame(eInputType control)
		{
			PAD.DISABLE_CONTROL_ACTION(0, (uint)control, true);
		}

		/// <summary>
		/// Disables all <see cref="eInputType"/> controls this frame.
		/// </summary>
		public static void DisableAllControlsThisFrame()
		{
			PAD.DISABLE_ALL_CONTROL_ACTIONS(0);
		}

		/// <summary>
		/// Get how long ago since any input was given in milliseconds
		/// </summary>
		public static int LastInputTime => PAD.GET_CONTROL_HOW_LONG_AGO(0);

		/// <summary>
		/// Gets a value indicating whether the game is using keyboard and mouse input
		/// </summary>
		public static bool IsUsingKeyboardAndMouse => PAD.IS_USING_KEYBOARD_AND_MOUSE(0);

		#endregion

		/// <summary>
		/// Create a (case-insensitive) Jenkins-One-at-a-Time hash key
		/// </summary>
		public static uint Joaat(string key)
		{
			return MISC.GET_HASH_KEY(key);
		}

		/// <summary>
		/// Get a GXT label entry
		/// </summary>
		public static string GetGXTEntry(string entry)
		{
			return HUD._GET_LABEL_TEXT_2(entry);
		}

		/// <summary>
		/// Check whether the given GXT label entry exists
		/// </summary>
		public static bool DoesGXTEntryExist(string entry)
		{
			return HUD.DOES_TEXT_LABEL_EXIST(entry);
		}
	}
}
