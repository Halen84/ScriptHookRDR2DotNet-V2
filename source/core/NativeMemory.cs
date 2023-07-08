//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/ScriptHookRDR2dotnet#license
//

#define CPP_SCRIPTHOOKRDR_V2
#undef CPP_SCRIPTHOOKRDR_V2 // Comment this out if you are using keps C++ ScriptHookRDR V2

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Security;
using static System.Runtime.InteropServices.Marshal;

namespace RDR2DN
{
	/// <summary>
	/// Class responsible for managing all access to game memory.
	/// </summary>
	public static unsafe class NativeMemory
	{
		#region ScriptHookRDR2 Imports
		/// <summary>
		/// Gets the game version enumeration value as specified by ScriptHookRDR2.
		/// </summary>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?getGameVersion@@YA?AW4eGameVersion@@XZ")]
		public static extern int GetGameVersion();

		/// <summary>
		/// Returns pointer to a global variable. IDs may differ between game versions.
		/// </summary>
		/// <param name="index">The variable ID to query.</param>
		/// <returns>Pointer to the variable, or <see cref="IntPtr.Zero"/> if it does not exist.</returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?getGlobalPtr@@YAPEA_KH@Z")]
		public static extern IntPtr GetGlobalPtr(int index);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?getScriptHandleBaseAddress@@YAPEAEH@Z")]
		static extern IntPtr GetScriptHandleBaseAddress(int handle);

		// Pools
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllObjects@@YAHPEAHH@Z")]
		public static extern int worldGetAllObjects(int[] arr, int arrSize);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllPeds@@YAHPEAHH@Z")]
		public static extern int worldGetAllPeds(int[] arr, int arrSize);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllPickups@@YAHPEAHH@Z")]
		public static extern int worldGetAllPickups(int[] arr, int arrSize);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllVehicles@@YAHPEAHH@Z")]
		public static extern int worldGetAllVehicles(int[] arr, int arrSize);

#if CPP_SCRIPTHOOKRDR_V2

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllBlips@@YAHPEAHH@Z")]
		public static extern int worldGetAllBlips(int[] arr, int arrSize);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllCams@@YAHPEAHH@Z")]
		public static extern int worldGetAllCams(int[] arr, int arrSize);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?worldGetAllVolumes@@YAHPEAHH@Z")]
		public static extern int worldGetAllVolumes(int[] arr, int arrSize);

		/// <summary>
		/// Switch text labels from the game with your own, this will allow you to provide your custom text in input boxes such as the onscreen keyboard
		/// <param name="oldLabel">The old label to replace</param>
		/// <param name="newLabel">Your new label</param>
		/// </summary>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?switchLabel@@YAXPEBD0@Z")]
		public static extern int SwitchLabel([MarshalAs(UnmanagedType.LPStr)] string oldLabel, [MarshalAs(UnmanagedType.LPStr)] string newLabel);

		/// <summary>
		/// Returns a pointer to local variables in game scripts 
		/// </summary>
		/// <param name="scriptName">The script name</param>
		/// <param name="staticIndex">The local variable index</param>
		/// <returns>Pointer to the variable, or <see cref="IntPtr.Zero"/> if it does not exist.</returns>
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookRDR2.dll", ExactSpelling = true, EntryPoint = "?getStaticPtr@@YAPEA_KPEBDH@Z")]
		public static extern IntPtr GetStaticPtr([MarshalAs(UnmanagedType.LPStr)] string scriptName, int staticIndex);

#endif //CPP_SCRIPTHOOKRDR_V2

		#endregion

		/// <inheritdoc cref="FindPatternNaive(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternNaive(string pattern, string mask)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;
			return FindPatternNaive(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternNaive(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
			{
				return null;
			}

			ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

			return FindPatternNaive(pattern, mask, startAddress, size);
		}

		/// <summary>
		/// Searches the specific address space of the current process for a memory pattern using the naive algorithm.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress, ulong size)
		{
			ulong address = (ulong)startAddress.ToInt64();
			ulong endAddress = address + size;

			for (; address < endAddress; address++)
			{
				for (int i = 0; i < pattern.Length; i++)
				{
					if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
					{
						break;
					}

					if (i + 1 == pattern.Length)
					{
						return (byte*)address;
					}
				}
			}

			return null;
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;
			return FindPatternBmh(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
			{
				return null;
			}

			ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

			return FindPatternBmh(pattern, mask, startAddress, size);
		}

		/// <summary>
		/// Searches the address space of the current process for a memory pattern using the Boyer–Moore–Horspool algorithm.
		/// Will perform faster than the naive algorithm when the pattern is long enough to expect the bad character skip is consistently high.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress, ulong size)
		{
			// Use short array intentionally to spare heap
			// Warning: throws an exception if length of pattern and mask strings does not match
			short[] patternArray = new short[pattern.Length];
			for (int i = 0; i < patternArray.Length; i++)
			{
				patternArray[i] = (mask[i] != '?') ? (short)pattern[i] : (short)-1;
			}

			int lastPatternIndex = patternArray.Length - 1;
			short[] skipTable = CreateShiftTableForBmh(patternArray);

			byte* endAddressToScan = (byte*)startAddress + size - patternArray.Length;

			// Pin arrays to avoid boundary check and search will be long enough to amortize the pin cost in time wise
			fixed (short* skipTablePtr = skipTable)
			fixed (short* patternArrayPtr = patternArray)
			{
				for (byte* curHeadAddress = (byte*)startAddress; curHeadAddress <= endAddressToScan; curHeadAddress += Math.Max((int)skipTablePtr[(curHeadAddress)[lastPatternIndex] & 0xFF], 1))
				{
					for (int i = lastPatternIndex; patternArrayPtr[i] < 0 || ((byte*)curHeadAddress)[i] == patternArrayPtr[i]; --i)
					{
						if (i == 0)
						{
							return curHeadAddress;
						}
					}
				}
			}

			return null;
		}

		private static short[] CreateShiftTableForBmh(short[] pattern)
		{
			short[] skipTable = new short[256];
			int lastIndex = pattern.Length - 1;

			int diff = lastIndex - Math.Max(Array.LastIndexOf<short>(pattern, -1), 0);
			if (diff == 0)
			{
				diff = 1;
			}

			for (int i = 0; i < skipTable.Length; i++)
			{
				skipTable[i] = (short)diff;
			}

			for (int i = lastIndex - diff; i < lastIndex; i++)
			{
				short patternVal = pattern[i];
				if (patternVal >= 0)
				{
					skipTable[patternVal] = (short)(lastIndex - i);
				}
			}

			return skipTable;
		}

		/// <summary>
		/// Disposes unmanaged resources.
		/// </summary>
		internal static void DisposeUnmanagedResources()
		{
			Marshal.FreeCoTaskMem(String);
			Marshal.FreeCoTaskMem(NullString);

			String = IntPtr.Zero;
			NullString = IntPtr.Zero;
		}

		/// <summary>
		/// Initializes all known functions and offsets based on pattern searching.
		/// </summary>
		static NativeMemory()
		{
			String = StringToCoTaskMemUTF8("LITERAL_STRING"); // "~a~"
			NullString = StringToCoTaskMemUTF8(string.Empty); // ""

			/*
			byte* address;

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x20\x33\xDB\x38\x1D\x00\x00\x00\x00\x74\x1C", "xxxxxxxxxx????xx");
			GetPlayerAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new new IntPtr(*(int*)(address + 3) + address + 7));

			address = FindPatternBmh("\x44\x8B\xC9\x83\xF9\xFF", "xxxxxx");
			GetEntityAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new IntPtr(*(int*)address));

			address = FindPatternBmh("\x48\x83\xEC\x28\x45\x33\xC0\x44\x8B\xC9", "xxxxxxxxxx");
			GetCameraAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new IntPtr(*(int*)address));
			*/
		}

		/// <summary>
		/// Reads a single 8-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static byte ReadByte(IntPtr address)
		{
			return *(byte*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single 16-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static Int16 ReadInt16(IntPtr address)
		{
			return *(short*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single 32-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static Int32 ReadInt32(IntPtr address)
		{
			return *(int*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single floating-point value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static float ReadFloat(IntPtr address)
		{
			return *(float*)address.ToPointer();
		}
		/// <summary>
		/// Reads a null-terminated UTF-8 string from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The string at the address.</returns>
		public static String ReadString(IntPtr address)
		{
			return PtrToStringUTF8(address);
		}
		/// <summary>
		/// Reads a single 64-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static IntPtr ReadAddress(IntPtr address)
		{
			return new IntPtr(*(void**)(address.ToPointer()));
		}
		/// <summary>
		/// Reads a 4x4 floating-point matrix from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>All elements of the matrix in row major arrangement.</returns>
		public static float[] ReadMatrix(IntPtr address)
		{
			var data = (float*)address.ToPointer();
			return new float[16] { data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15] };
		}
		/// <summary>
		/// Reads a 3-component floating-point vector from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>All elements of the vector.</returns>
		public static float[] ReadVector3(IntPtr address)
		{
			var data = (float*)address.ToPointer();
			return new float[3] { data[0], data[1], data[2] };
		}

		/// <summary>
		/// Writes a single 8-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteByte(IntPtr address, byte value)
		{
			var data = (byte*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 16-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt16(IntPtr address, Int16 value)
		{
			var data = (short*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 32-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt32(IntPtr address, Int32 value)
		{
			var data = (int*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single floating-point value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteFloat(IntPtr address, float value)
		{
			var data = (float*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a 4x4 floating-point matrix to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The elements of the matrix in row major arrangement to write.</param>
		public static void WriteMatrix(IntPtr address, float[] value)
		{
			var data = (float*)(address.ToPointer());
			for (int i = 0; i < value.Length; i++)
				data[i] = value[i];
		}
		/// <summary>
		/// Writes a 3-component floating-point to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The vector components to write.</param>
		public static void WriteVector3(IntPtr address, float[] value)
		{
			var data = (float*)address.ToPointer();
			data[0] = value[0];
			data[1] = value[1];
			data[2] = value[2];
		}

		/// <summary>
		/// Sets a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to change.</param>
		public static void SetBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			*data |= (1 << bit);
		}
		/// <summary>
		/// Clears a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to change.</param>
		public static void ClearBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			*data &= ~(1 << bit);
		}
		/// <summary>
		/// Checks a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to check.</param>
		/// <returns><c>true</c> if the bit is set, <c>false</c> if it is unset.</returns>
		public static bool IsBitSet(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			return (*data & (1 << bit)) != 0;
		}

		private static byte[] s_strBufferForStringToCoTaskMemUtf8 = new byte[100];
		public static IntPtr String { get; private set; } // "~a~"
		public static IntPtr NullString { get; private set; } // ""

		public static string PtrToStringUTF8(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return string.Empty;

			var data = (byte*)ptr.ToPointer();

			// Calculate length of null-terminated string
			int len = 0;
			while (data[len] != 0)
				++len;

			return PtrToStringUTF8(ptr, len);
		}
		public static string PtrToStringUTF8(IntPtr nativeUtf8, int len)
		{
			while (Marshal.ReadByte(nativeUtf8, len) != 0) ++len;
			byte[] buffer = new byte[len];
			Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
			return Encoding.UTF8.GetString(buffer);
		}


		public static IntPtr StringToCoTaskMemUTF8(string managedString)
		{
			int len = Encoding.UTF8.GetByteCount(managedString);
			byte[] buffer = new byte[len + 1];
			Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
			IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
			return nativeUtf8;
		}


		#region -- Pool Addresses --

		delegate ulong GetHandleAddressFuncDelegate(int handle);
		static GetHandleAddressFuncDelegate GetEntityAddressFunc;
		static GetHandleAddressFuncDelegate GetPlayerAddressFunc;
		static GetHandleAddressFuncDelegate GetCameraAddressFunc;

		/*public static IntPtr GetEntityAddress(int handle)
		{
			return new IntPtr((long)GetEntityAddressFunc(handle));
		}
		public static IntPtr GetPlayerAddress(int handle)
		{
			return new IntPtr((long)GetPlayerAddressFunc(handle));
		}
		public static IntPtr GetCameraAddress(int handle)
		{
			return new IntPtr((long)*GetCameraAddressFunc(handle));
		}
		*/

#endregion

			#region -- Game Data --

			delegate uint GetHashKeyDelegate(IntPtr stringPtr, uint initialHash);
		static GetHashKeyDelegate GetHashKeyFunc;

		public static uint GetHashKey(string key)
		{
			IntPtr keyPtr = ScriptDomain.CurrentDomain.PinString(key);
			return GetHashKeyFunc(keyPtr, 0);
		}

		//static ulong GetLabelTextByHashAddress;
		delegate ulong GetLabelTextByHashFuncDelegate(ulong address, int labelHash);
		//static GetLabelTextByHashFuncDelegate GetLabelTextByHashFunc;



		#endregion

		enum ModelInfoClassType
		{
			Invalid = 0,
			Object = 1,
			Mlo = 2,
			Time = 3,
			Weapon = 4,
			Vehicle = 5,
			Ped = 6
		}

	}
}
