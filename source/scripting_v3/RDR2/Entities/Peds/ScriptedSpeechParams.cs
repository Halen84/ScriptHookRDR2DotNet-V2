using System;
using System.Runtime.InteropServices;

namespace RDR2
{
	[StructLayout(LayoutKind.Explicit, Size = 0x40), Serializable]
	public struct ScriptedSpeechParams
	{
		// In C#, "string" is a managed type so we have to do some bullshittery to get this to work.
		// We set string values (const char*) as a Int64 to get around this.
		[FieldOffset(0x0)] public long speechName; // const char*
		[FieldOffset(0x8)] public long voiceName; // const char*

		[FieldOffset(0x10)] public int variation;
		[FieldOffset(0x18)] public uint speechParamHash;
		[FieldOffset(0x20)] public int listenerPed;

		// Since a C# boolean is not interpreted as 1 and 0, make the type an int
		[FieldOffset(0x28)] public int syncOverNetwork; // BOOL

		[FieldOffset(0x30)] public int v7; // Always 1
		[FieldOffset(0x38)] public int v8; // Unused in the scripts, exists in IDA
	}
}
