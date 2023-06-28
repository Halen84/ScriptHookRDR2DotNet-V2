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

			// OnTick() will be called every frame/tick/update
			Tick += OnTick;

			// OnKeyDown() will be called everytime a key is pressed
			KeyDown += OnKeyDown;

			// OnKeyUp() will be called everytime a key is released
			KeyUp += OnKeyUp;

			// Set how often this script should run each frame (tick)
			Interval = 0;
		}

		private void OnTick(object sender, EventArgs e)
		{
			Ped playerPed = Game.Player.Character; // Get our player ped
			Player player = Game.Player; // Get our player
			Vehicle veh = playerPed.CurrentVehicle; // Get the vehicle we are currently in

			// Instead of keyboard only on KeyUp and KeyDown, you use the Game.IsControl... functions (PAD namespace)
			// which work on both controller and keyboard.
			if (Game.IsControlJustPressed(eInputType.Reload))
			{
				// Do Stuff (Keyboard AND Controller)

				RDR2.UI.Screen.PrintSubtitle("~COLOR_OBJECTIVE~Reload~s~ was pressed.");
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
