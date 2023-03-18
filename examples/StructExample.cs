using System;
using System.Runtime.InteropServices;
using System.Text;
using RDR2;             // Generic RDR2 related stuff
using RDR2.UI;          // UI related stuff
using RDR2.Native;      // RDR2 native functions (commands)
using RDR2.Math;        // Vectors, Quaternions, Matrixes

namespace RDR2
{
	public class StructExample : Script
	{
		public StructExample()
		{
			Tick += OnTick;
		}


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

		private void OnTick(object sender, EventArgs e)
		{
			if (Game.IsControlJustPressed(eInputType.Reload))
			{
				PlaySpeechOnPed(Game.Player.Character, "RE_PH_RHD_V3_AGGRO", "0405_U_M_M_RhdSheriff_01", MISC.GET_HASH_KEY("SPEECH_PARAMS_BEAT_SHOUTED_CLEAR"));
			}
		}

		private void PlaySpeechOnPed(Ped speaker, string speechName, string voiceName, uint speechParamHash)
		{
			ScriptedSpeechParams @params = new ScriptedSpeechParams();

			// Convert the strings to a unmanaged IntPtr
			IntPtr pSpeechName = MarshalManagedStrToNative(speechName);
			IntPtr pVoiceName = MarshalManagedStrToNative(voiceName);

			@params.speechName = pSpeechName.ToInt64(); // Convert the pointers to Int64 (they will act as strings)
			@params.voiceName = pVoiceName.ToInt64();
			@params.variation = 1;
			@params.speechParamHash = speechParamHash;
			@params.listenerPed = 0;
			@params.syncOverNetwork = 1; // TRUE
			@params.v7 = 1;
			@params.v8 = 0;

			// RAGE Script structs must be passed to natives as type ulong* aka Any*
			unsafe { AUDIO.PLAY_PED_AMBIENT_SPEECH_NATIVE(speaker.Handle, (ulong*)&@params); }

			Marshal.FreeCoTaskMem(pSpeechName);
			Marshal.FreeCoTaskMem(pVoiceName);
		}

		// Used for struct fields that are of type  const char*
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
