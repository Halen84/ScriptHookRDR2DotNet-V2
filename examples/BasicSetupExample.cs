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

			// OnAbort() will be called when this script thread gets aborted
			Aborted += OnAbort;

			// Sets the interval between each tick or "frame". 0 means this script will run every frame
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

			{
				//
				// There are 2 ways to call natives.
				// Function.Call(hash, args...) (old)  and  [Namespace].[Native Name] (new)
				// Like so:
				//

				// Note: There is an implicit conversion when passing "INativeValue" or "PoolObject" classes to natives.
				// At compile time, this call is converted to "player.Handle" instead.
				PLAYER.SET_PLAYER_INVINCIBLE(player, true); // New way of calling natives (recommended)
				Function.Call(0xFEBEEBC9CBDF4B12, true); // Old way of calling natives. 0xFEBEEBC9CBDF4B12 is the hash of SET_PLAYER_INVINCIBLE

				bool isPlayerInvincible_1 = PLAYER.GET_PLAYER_INVINCIBLE(player);
				bool isPlayerInvincible_2 = Function.Call<bool>(0x0CBBCB2CCFA7DC4E); // 0x0CBBCB2CCFA7DC4E is the hash of GET_PLAYER_INVINCIBLE
			}

			{
				//
				// Calling natives that have pointers:
				//

				// New way
				unsafe
				{
					Vector3 min = Vector3.Zero;
					Vector3 max = Vector3.Zero;
					ENTITY._GET_ENTITY_WORLD_POSITION_OF_DIMENSIONS(player.Ped, &min, &max);
				}

				// Old way
				OutputArgument out1 = new OutputArgument();
				OutputArgument out2 = new OutputArgument();
				Function.Call(0xF3FDA9A617A15145, player.Ped, out1, out2); // 0xF3FDA9A617A15145 is the hash of _GET_ENTITY_WORLD_POSITION_OF_DIMENSIONS
				Vector3 _min = out1.GetResult<Vector3>();
				Vector3 _max = out2.GetResult<Vector3>();

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

		private void OnAbort(object sender, EventArgs e)
		{
			// Do work when this script thread has been aborted
		}
	}
}
