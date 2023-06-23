using RDR2.Native;

namespace RDR2.UI
{
	public static class Drawing
	{
		public static void DrawRect(float screenX, float screenY, float width, float height, int red, int green, int blue, int alpha, bool centered)
		{
			screenX /= Game.ScreenResolution.Width;
			screenY /= Game.ScreenResolution.Height;

			width /= Game.ScreenResolution.Width;
			height /= Game.ScreenResolution.Height;

			if (!centered)
			{
				screenX += width * 0.5f;
				screenY += height * 0.5f;
			}

			GRAPHICS.DRAW_RECT(screenX, screenY, width, height, red, green, blue, alpha, false, false);
		}

		public static void DrawSprite(string textureDict, string textureName, float screenX, float screenY, float width, float height, float rotation, int red, int green, int blue, int alpha, bool centered)
		{
			screenX /= Game.ScreenResolution.Width;
			screenY /= Game.ScreenResolution.Height;

			width /= Game.ScreenResolution.Width;
			height /= Game.ScreenResolution.Height;

			if (!centered)
			{
				screenX += width * 0.5f;
				screenY += height * 0.5f;
			}

			GRAPHICS.DRAW_SPRITE(textureDict, textureName, screenX, screenY, width, height, rotation, red, green, blue, alpha, false);
		}

		public static void DrawText(string text, float screenX, float screenY, float scale, int red, int green, int blue, int alpha)
		{
			string varString = MISC.VAR_STRING(10, "LITERAL_STRING", text);
			UIDEBUG._BG_SET_TEXT_SCALE(scale, scale);
			UIDEBUG._BG_SET_TEXT_COLOR(red, green, blue, alpha);
			UIDEBUG._BG_DISPLAY_TEXT(varString, screenX, screenY);
		}
	}
}
