//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System.Drawing;

namespace RDR2.UI
{
	/// <summary>
	/// Methods to handle UI actions that affect the whole screen.
	/// </summary>
	public static class Screen
	{
		#region Fields
		private static readonly string[] _effects = new string[] {
			
		};
		#endregion

		// Dimensions

		/// <summary>
		/// The base width of the screen used for all UI Calculations, unless ScaledDraw is used
		/// </summary>
		public const float Width = 1920f;
		/// <summary>
		/// The base height of the screen used for all UI Calculations
		/// </summary>
		public const float Height = 1080f;

		public static bool IsCinematicModeEnabled
		{
			set => CAM.SET_CINEMATIC_BUTTON_ACTIVE(value);
		}

		public static bool IsCinematicModeActive
		{
			set => CAM.SET_CINEMATIC_MODE_ACTIVE(value);
		}

		/// <summary>
		/// Gets the actual screen resolution the game is being rendered at
		/// </summary>
		public static Size Resolution
		{
			get
			{
				return Game.ScreenResolution;
			}
		}
		/// <summary>
		/// Gets the current screen aspect ratio
		/// </summary>
		public static float AspectRatio => 1.0f;
		/// <summary>
		/// Gets the screen width scaled against a 720pixel height base.
		/// </summary>
		public static float ScaledWidth => Height * 1.0f;

		// Fading

		/// <summary>
		/// Gets a value indicating whether the screen is faded in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedIn => CAM.IS_SCREEN_FADED_IN();
		/// <summary>
		/// Gets a value indicating whether the screen is faded out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedOut => CAM.IS_SCREEN_FADED_OUT();
		/// <summary>
		/// Gets a value indicating whether the screen is fading in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingIn => CAM.IS_SCREEN_FADING_IN();
		/// <summary>
		/// Gets a value indicating whether the screen is fading out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingOut => CAM.IS_SCREEN_FADING_OUT();

		/// <summary>
		/// Fades the screen in over a specific time, useful for transitioning
		/// </summary>
		/// <param name="duration">The time for the fade in to take</param>
		public static void FadeIn(int duration)
		{
			CAM.DO_SCREEN_FADE_IN(duration);
		}
		/// <summary>
		/// Fades the screen out over a specific time, useful for transitioning
		/// </summary>
		/// <param name="duration">The time for the fade out to take</param>
		public static void FadeOut(int duration)
		{
			CAM.DO_SCREEN_FADE_OUT(duration);
		}

		// Letter Box

		public static float LetterBoxRatio => CAM.GET_LETTER_BOX_RATIO();
		public static bool HasLetterBox
		{
			get => CAM.HAS_LETTER_BOX();
			set => CAM._REQUEST_LETTER_BOX_NOW(value, value);
		}

		public static void RequestLetterBox()
		{
			CAM._REQUEST_LETTER_BOX_OVERTIME(-1, -1, false, 17, true, false);
		}

		public static void RequestLetterBoxImmediately(bool bEnabled)
		{
			HasLetterBox = bEnabled;
		}

		// Post Processing Effects

		/// <summary>
		/// Gets a value indicating whether the specific screen effect is running.
		/// </summary>
		/// <param name="effectName">The effect to check.</param>
		/// <returns><c>true</c> if the screen effect is active; otherwise, <c>false</c>.</returns>
		public static bool IsEffectActive(string effectName)
		{
			return GRAPHICS.ANIMPOSTFX_IS_RUNNING(effectName);
		}

		/// <summary>
		/// Plays a post processing effect on screen
		/// </summary>
		/// <param name="effectName">The effect to play</param>
		public static void PlayEffect(string effectName)
		{
			if (!GRAPHICS._ANIMPOSTFX_HAS_LOADED(effectName)) {
				GRAPHICS._ANIMPOSTFX_PRELOAD_POSTFX(effectName);
			}

			if (GRAPHICS._ANIMPOSTFX_HAS_LOADED(effectName)) {
				GRAPHICS.ANIMPOSTFX_PLAY(effectName);
			}

			GRAPHICS._ANIMPOSTFX_SET_TO_UNLOAD(effectName);
		}

		/// <summary>
		/// Stops a running post processing effect on screen
		/// </summary>
		/// <param name="effectName">The effect to stop</param>
		public static void StopEffect(string effectName)
		{
			if (GRAPHICS.ANIMPOSTFX_IS_RUNNING(effectName)) {
				GRAPHICS.ANIMPOSTFX_STOP(effectName);
			}
		}

		/// <summary>
		/// Stops a running post processing effect on screen
		/// </summary>
		/// <param name="effectName">The effect to stop</param>
		public static void ClearEffect(string effectName)
		{
			if (GRAPHICS.ANIMPOSTFX_IS_RUNNING(effectName)) {
				GRAPHICS._ANIMPOSTFX_CLEAR_EFFECT(effectName);
			}
		}

		/// <summary>
		/// Stops all currently running effects.
		/// </summary>
		public static void StopAllEffects()
		{
			GRAPHICS.ANIMPOSTFX_STOP_ALL();
		}

		#region Subtitle Text Functions

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time
		/// </summary>
		/// <param name="message">The message to display.</param>
		public static void PrintSubtitle(string message)
		{
			UILOG._UILOG_SET_CACHED_OBJECTIVE(message);
			UILOG._UILOG_PRINT_CACHED_OBJECTIVE();
			UILOG._UILOG_CLEAR_HAS_DISPLAYED_CACHED_OBJECTIVE();
			UILOG._UILOG_CLEAR_CACHED_OBJECTIVE();
		}

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time
		/// </summary>
		/// <param name="message">The message to display.</param>
		public static void PrintSubtitle(params string[] message)
		{
			string actualMessage = string.Empty;
			foreach (string str in message) {
				actualMessage = string.Concat(actualMessage, str);
			}

			PrintSubtitle(actualMessage);
		}

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time.
		/// <remarks>This is an alias for <see cref="PrintSubtitle(string)"/></remarks>
		/// </summary>
		/// <param name="message">The message to display.</param>
		public static void DisplaySubtitle(string message)
		{
			PrintSubtitle(message);
		}

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time.
		/// <remarks>This is an alias for <see cref="PrintSubtitle(string[])"/></remarks>
		/// </summary>
		/// <param name="message">The message to display.</param>
		public static void DisplaySubtitle(params string[] message)
		{
			PrintSubtitle(message);
		}

		#endregion

		// Space Conversion

		/// <summary>
		/// Translates a point in WorldSpace to its given Coordinates on the <see cref="Screen"/>
		/// </summary>
		/// <param name="position">The position in the World.</param>
		/// <param name="scaleWidth">if set to <c>true</c> Returns the screen position scaled by <see cref="ScaledWidth"/>; otherwise, returns the screen position scaled by <see cref="Width"/>.</param>
		/// <returns></returns>
		public static PointF WorldToScreen(Vector3 position, bool scaleWidth = false)
		{
			float pointX, pointY;

			unsafe
			{
				if (!GRAPHICS.GET_SCREEN_COORD_FROM_WORLD_COORD(position.X, position.Y, position.Z, &pointX, &pointY)) {
					return PointF.Empty;
				}
			}

			pointX *= scaleWidth ? ScaledWidth : Width;
			pointY *= Height;

			return new PointF(pointX, pointY);
		}
	}
}
