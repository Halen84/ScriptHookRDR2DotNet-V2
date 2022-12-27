using System;
using System.Runtime.InteropServices;
using System.Text;
using RDR2;             // Generic RDR2 related stuff
using RDR2.UI;          // UI related stuff
using RDR2.Native;      // RDR2 native functions (commands)
using RDR2.Math;        // Vectors, Quaternions, Matrixes


// C# type aliasing is so shit...

// In C#, a string is a managed type so we have to do some bullshittery to get it to work.
// We set string values as a int64 to get around this.
using StructString = System.Int64;

// Since a C# boolean is not interpreted as 1 and 0, make it an int
using StructBool = System.Int32;


namespace RDR2
{
	public class StructExample : Script
	{
		public StructExample()
		{
			Tick += OnTick;
		}


		[StructLayout(LayoutKind.Explicit, Size = 0x38/*56*/), Serializable]
		public struct ScriptedSpeechParams
		{
			[FieldOffset(0)] public StructString speechName;
			[FieldOffset(8)] public StructString voiceName;
			[FieldOffset(16)] public int variation;
			[FieldOffset(24)] public uint speechParamHash;
			[FieldOffset(32)] public int listenerPed;
			[FieldOffset(40)] public StructBool syncOverNetwork;
			[FieldOffset(48)] public int v7; // Always 1 (Only 0 in one game script)
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (Game.IsControlJustPressed(eInputType.Reload))
			{
				PlaySpeechOnPed(Game.Player.Character, "RE_PH_RHD_V3_AGGRO", "0405_U_M_M_RhdSheriff_01", MISC.GET_HASH_KEY("SPEECH_PARAMS_BEAT_SHOUTED_CLEAR"));
			}
		}

		private unsafe void PlaySpeechOnPed(Ped speaker, string speechName, string voiceName, uint speechParamHash)
		{
			ScriptedSpeechParams @params = new ScriptedSpeechParams();

			// Convert the strings to a unmanaged IntPtr
			IntPtr pSpeechName = MarshalManagedStrToNative(speechName);
			IntPtr pVoiceName = MarshalManagedStrToNative(voiceName);

			@params.speechName = pSpeechName.ToInt64(); // Convert the pointers to int64 (StructString)
			@params.voiceName = pVoiceName.ToInt64();
			@params.variation = 1;
			@params.speechParamHash = speechParamHash;
			@params.listenerPed = 0;
			@params.syncOverNetwork = 1; // TRUE
			@params.v7 = 1;

			// RAGE Script structs must be passed as type "ulong*".
			// So cast it to a pointer and get its address.
			AUDIO.PLAY_PED_AMBIENT_SPEECH_NATIVE(speaker.Handle, (ulong*)&@params);

			Marshal.FreeCoTaskMem(pSpeechName);
			Marshal.FreeCoTaskMem(pVoiceName);
		}

		// Used for struct fields that are strings (StructString)
		private IntPtr MarshalManagedStrToNative(string str)
		{
			var bytes = Encoding.UTF8.GetBytes(str);
			var ptr = Marshal.AllocCoTaskMem(bytes.Length + 1);
			Marshal.Copy(bytes, 0, ptr, bytes.Length);
			Marshal.WriteByte(ptr, bytes.Length, 0);
			return ptr;
		}
	}
}
