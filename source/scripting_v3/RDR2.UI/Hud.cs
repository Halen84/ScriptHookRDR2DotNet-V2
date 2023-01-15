//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;

namespace RDR2.UI
{
	/// <summary>
	/// Methods to manipulate the HUD (heads-up-display) of the game.
	/// </summary>
	public static class Hud
	{
		/// <summary>
		/// Enables a <see cref="eHudContext"/>
		/// </summary>
		/// <param name="component">The <see cref="eHudContext"/> to enable</param>
		public static void EnableHudContext(eHudContext component)
		{
			HUD._ENABLE_HUD_CONTEXT((uint)component);
		}

		/// <summary>
		/// Disables a <see cref="eHudContext"/>
		/// </summary>
		/// <param name="component">The <see cref="eHudContext"/> to disable</param>
		public static void DisableHudContext(eHudContext component)
		{
			HUD._DISABLE_HUD_CONTEXT((uint)component);
		}

		/// <summary>
		/// Shows the mouse cursor this frame.
		/// </summary>
		public static void ShowCursorThisFrame()
		{
			_NAMESPACE30.SET_MOUSE_CURSOR_THIS_FRAME();
		}

		/// <summary>
		/// Gets or sets the sprite the cursor should used when drawn
		/// </summary>
		public static eCursor CursorSprite
		{
			set => _NAMESPACE30.SET_MOUSE_CURSOR_STYLE((int)value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether any HUD components should be rendered.
		/// </summary>
		public static bool IsVisible
		{
			get => !HUD.IS_HUD_HIDDEN();
			set => HUD.DISPLAY_HUD(value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the radar is visible.
		/// </summary>
		public static bool IsRadarVisible
		{
			get => !HUD.IS_RADAR_HIDDEN();
			set => MAP.DISPLAY_RADAR(value);
		}

		/// <summary>
		/// Sets how far the minimap should be zoomed in.
		/// </summary>
		/// <value>
		/// The radar zoom; accepts values from 0 to 200.
		/// </value>
		public static int RadarZoom
		{
			set => MAP.SET_RADAR_ZOOM(value);
		}
	}
}
