//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace RDR2
{
	public class GlobalCollection
	{
		internal GlobalCollection()
		{
		}

		public Global this[int globalId]
		{
			get => new Global(globalId);
			set
			{
				unsafe
				{
					*(ulong*)RDR2DN.NativeMemory.GetGlobalPtr(globalId).ToPointer() = *value.MemoryAddress;
				}
			}
		}

		/// <summary>
		/// Returns an instance of <see cref="Global"/> to a script global.
		/// <remarks>Make sure that you check the game version before accessing globals. ID's may differ between patches.</remarks>
		/// </summary>
		/// <param name="globalId">The script global index</param>
		/// <returns></returns>
		public Global Get(int globalId)
		{
			return new Global(globalId);
		}
	}
}
