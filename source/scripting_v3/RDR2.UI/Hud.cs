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
		/*/// <summary>
		/// Determines whether a given <see cref="HudComponent"/> is active.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to check</param>
		/// <returns><c>true</c> if the <see cref="HudComponent"/> is active; otherwise, <c>false</c></returns>
		public static bool IsComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, component);
		}

		/// <summary>
		/// Draws the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/></param>
		///<remarks>This will only draw the <see cref="HudComponent"/> if the <see cref="HudComponent"/> can be drawn</remarks>
		public static void ShowComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, component);
		}
		/// <summary>
		/// Hides the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to hide.</param>
		public static void HideComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, component);
		}
		*/
		/// <summary>
		/// Shows the mouse cursor this frame.
		/// </summary>
		public static void ShowCursorThisFrame()
		{
			_NAMESPACE30._SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME();
		}

		/// <summary>
		/// Gets or sets the sprite the cursor should used when drawn
		/// </summary>
		public static CursorSprite CursorSprite
		{
			set => _NAMESPACE30._SET_MOUSE_CURSOR_SPRITE((int)value);
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
