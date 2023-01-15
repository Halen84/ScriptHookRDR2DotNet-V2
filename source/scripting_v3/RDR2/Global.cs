//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Text;
using System.Runtime.InteropServices;
using RDR2.Math;

namespace RDR2
{
	public unsafe struct Global
	{
		readonly IntPtr address;

		internal Global(int globalId)
		{
			address = RDR2DN.NativeMemory.GetGlobalPtr(globalId);
		}

		public unsafe ulong* MemoryAddress => (ulong*)address.ToPointer();

		/*public unsafe void Set<T>(T value)
		{
			if (typeof(T) == typeof(int)) {
				RDR2DN.NativeMemory.WriteInt32(address, Convert.ToInt32(value));
			}
			else if (typeof(T) == typeof(float)) {
				RDR2DN.NativeMemory.WriteFloat(address, (float)Convert.ToDouble(value));
			}
			else if (typeof(T) == typeof(string)) {
				string _value = Convert.ToString(value);
				int size = Encoding.UTF8.GetByteCount(_value);
				Marshal.Copy(Encoding.UTF8.GetBytes(_value), 0, address, size);
				*((byte*)MemoryAddress + size) = 0;
			}
			else if (typeof(T) == typeof(Vector3)) {
				Vector3 vec = (Vector3)Convert.ChangeType(value, typeof(Vector3));
				RDR2DN.NativeMemory.WriteVector3(address, vec.ToArray());
			}

			throw new InvalidCastException("Cannot set type '" + value.GetType() + "' to global");
		}*/

		public unsafe void SetInt(int value)
		{
			RDR2DN.NativeMemory.WriteInt32(address, value);
		}
		public unsafe void SetFloat(float value)
		{
			RDR2DN.NativeMemory.WriteFloat(address, value);
		}
		public unsafe void SetString(string value)
		{
			int size = Encoding.UTF8.GetByteCount(value);
			Marshal.Copy(Encoding.UTF8.GetBytes(value), 0, address, size);
			*((byte*)MemoryAddress + size) = 0;
		}
		public unsafe void SetVector3(Vector3 value)
		{
			RDR2DN.NativeMemory.WriteVector3(address, value.ToArray());
		}

		public unsafe int GetInt()
		{
			return RDR2DN.NativeMemory.ReadInt32(address);
		}
		public unsafe float GetFloat()
		{
			return RDR2DN.NativeMemory.ReadFloat(address);
		}
		public unsafe string GetString()
		{
			return RDR2DN.NativeMemory.PtrToStringUTF8(address);
		}
		public unsafe Vector3 GetVector3()
		{
			var data = RDR2DN.NativeMemory.ReadVector3(address);
			return new Vector3(data[0], data[1], data[2]);
		}
	}
}
