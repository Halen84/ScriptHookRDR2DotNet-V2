using System;
using System.Windows.Forms;
using System.Drawing;

using RDR2;             // Generic RDR2 related stuff
using RDR2.UI;          // UI related stuff
using RDR2.Native;      // RDR2 native functions (commands)
using RDR2.Math;        // Vectors, Quaternions, Matrixes

namespace PoolsExample
{
	public class Main : Script
	{
		public Main()
		{
			Tick += OnTick; // Hook Tick event. This is called every frame
			Interval = 1; // Optional, default is 0
		}

		private void OnTick(object sender, EventArgs e)
		{
			Ped[] pedsArray = World.GetAllPeds();
			Vehicle[] vehiclesArray = World.GetAllVehicles();
			Prop[] objectsArray = World.GetAllObjects();

			string caption = $"Number of peds in the game world:     {pedsArray.Length}\n" +
				             $"Number of vehicles in the game world: {vehiclesArray.Length}\n" +
				             $"Number of objects in the game world:  {objectsArray.Length}";

			TextElement textElement = new TextElement(caption, new PointF(200.0f, 200.0f), 0.35f);
			textElement.Draw();
		}
	}
}
