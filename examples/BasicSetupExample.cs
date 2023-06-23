using System;
using System.Windows.Forms;

using RDR2;             // Generic RDR2 related stuff
using RDR2.UI;          // UI related stuff
using RDR2.Native;      // RDR2 native functions (commands)
using RDR2.Math;        // Vectors, Quaternions, Matrixes

namespace BasicSetupExample
{
	public class Main : Script
	{
		public Main()
		{
			// Hook script events. These functions are called automatically

			// This function will be called every frame/tick/update
			Tick += OnTick;

			// This function will be called everytime a key is pressed
			KeyDown += OnKeyDown;

			// This function will be called everytime a key is released
			KeyUp += OnKeyUp;
		}

		private void OnTick(object sender, EventArgs e)
		{
			Ped playerPed = Game.Player.Character; // Get our player ped
			Player player = Game.Player; // Get our player

			// Instead of keyboard only on KeyUp and KeyDown, you use the Game.IsControl... functions (PAD namespace)
			// which work on both controller and keyboard.
			if (Game.IsControlJustPressed(eInputType.Reload))
			{
				// Do Stuff (Keyboard AND Controller)
			}
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			// (Keyboard Only)
			if (e.KeyCode == Keys.F24)
			{
				// Do Stuff
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			// (Keyboard Only)
			if (e.KeyCode == Keys.F24)
			{
				// Do Stuff
			}
		}
	}
}
