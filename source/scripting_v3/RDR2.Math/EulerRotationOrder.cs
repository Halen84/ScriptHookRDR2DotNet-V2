//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace RDR2
{
	/// <summary>
	/// Enums for the order in which to apply rotations in local space, just like how Rockstar Games define <c>EULER_ROT_ORDER</c>.
	/// </summary>
	public enum eEulerRotationOrder
	{
		XYZ = 0,
		XZY = 1,
		YXZ = 2,
		YZX = 3,
		ZXY = 4,
		ZYX = 5,
	}
}
