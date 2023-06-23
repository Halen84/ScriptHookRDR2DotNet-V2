using System;
using System.Windows.Forms;

using RDR2;             // Generic RDR2 related stuff
using RDR2.UI;          // UI related stuff
using RDR2.Native;      // RDR2 native functions (commands)
using RDR2.Math;        // Vectors, Quaternions, Matrixes

namespace RagdollExample
{
	public class Main : Script
	{
		private bool bRagdoll = false;

		public Main()
		{
			KeyDown += OnKeyDown; // Hook KeyDown event
			Tick += OnTick; // Hook Tick event. This is called every frame
			Interval = 1; // Optional, default is 0
		}

		private void OnTick(object sender, EventArgs e)
		{
			// Our current player ped
			// Note: "Ped" is a class, not a type alias like it is in C++
			Ped myPlayerPed = Game.Player.Character;

			// As long as "bRagdoll" is true, we will continue
			// to ragdoll because were calling this every frame (tick)
			if (bRagdoll)
			{
				PED.SET_PED_TO_RAGDOLL(myPlayerPed.Handle, 5000, 5000, 0, false, false, string.Empty);
			}
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.C)
			{
				bRagdoll = !bRagdoll;
				RDR2.UI.Screen.PrintSubtitle("Ragdoll: " + bRagdoll.ToString());
			}
		}
	}
}
