//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using RDR2.Math;

namespace RDR2.Native
{
	public interface INativeValue
	{
		ulong NativeValue
		{
			get; set;
		}
	}
	[StructLayout(LayoutKind.Explicit, Size = 0x18)]
	internal struct scrVector3
	{
		[FieldOffset(0x00)] internal float X;
		[FieldOffset(0x08)] internal float Y;
		[FieldOffset(0x10)] internal float Z;

		internal scrVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static implicit operator Vector3(scrVector3 value) => new Vector3(value.X, value.Y, value.Z);
		public static implicit operator scrVector3(Vector3 value) => new scrVector3(value.X, value.Y, value.Z);
	}

	internal unsafe static class NativeHelper<T>
	{
		static class CastCache<TFrom>
		{
			internal static readonly Func<TFrom, T> Convert;

			static CastCache()
			{
				var paramExp = Expression.Parameter(typeof(TFrom));
				var convertExp = Expression.Convert(paramExp, typeof(T));
				Convert = Expression.Lambda<Func<TFrom, T>>(convertExp, paramExp).Compile();
			}
		}

		static readonly Func<IntPtr, T> _ptrToStrFunc;

		static NativeHelper()
		{
			var ptrToStrMethod = new DynamicMethod("PtrToStructure<" + typeof(T) + ">", typeof(T),
				new Type[] { typeof(IntPtr) }, typeof(NativeHelper<T>), true);

			ILGenerator generator = ptrToStrMethod.GetILGenerator();
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldobj, typeof(T));
			generator.Emit(OpCodes.Ret);

			_ptrToStrFunc = (Func<IntPtr, T>)ptrToStrMethod.CreateDelegate(typeof(Func<IntPtr, T>));
		}

		internal static T Convert<TFrom>(TFrom from)
		{
			return CastCache<TFrom>.Convert(from);
		}

		internal static T PtrToStructure(IntPtr ptr)
		{
			return _ptrToStrFunc(ptr);
		}
	}

	public class InputArgument
	{
		internal ulong data;
		internal List<ulong> arData = new List<ulong>();

		public InputArgument(ulong value) { data = value; }
		public InputArgument(object value) { data = Function.ObjectToNative(value); }
		public InputArgument(InputArgument[] value)
		{
			// Copy the data over
			for (int i = 0; i < value.Length; i++)
			{
				arData.Add(value[i].data);
			}
		}

		// Types - ctor
		public InputArgument([MarshalAs(UnmanagedType.U1)] bool value) : this(value ? 1UL : 0UL) { }
		public InputArgument(int value) : this((uint)value) { }
		public InputArgument(uint value) : this((ulong)value) { }
		public InputArgument(double value) : this((float)value) { }
		public InputArgument(string value) : this((object)value) { }
		public InputArgument(float value) { unsafe { data = *(uint*)&value; } }
		public InputArgument(Vector3 value) : this((object)value) { }

		// Types - Operator
		public static implicit operator InputArgument([MarshalAs(UnmanagedType.U1)] bool value) { return value ? new InputArgument(1UL) : new InputArgument(0UL); }
		public static implicit operator InputArgument(InputArgument[] value) { return new InputArgument(value); } // Template Natives (e.g. VAR_STRING)
		public static implicit operator InputArgument(byte value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(sbyte value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(short value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(ushort value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(int value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(uint value) { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(float value) { return new InputArgument(value); }
		public static implicit operator InputArgument(string value) { return new InputArgument(value); }
		public static implicit operator InputArgument(double value) { return new InputArgument((float)value); }
		public static implicit operator InputArgument(Vector3 value) { return new InputArgument(value); }
		public static implicit operator InputArgument(Enum value)
		{
			// Note: The value will be boxed if the original value is a concrete enum
			Type enumDataType = Enum.GetUnderlyingType(value.GetType());
			ulong ulongValue = 0;

			if (enumDataType == typeof(int))
			{
				ulongValue = (ulong)Convert.ToInt32(value);
			}
			else if (enumDataType == typeof(uint))
			{
				ulongValue = Convert.ToUInt32(value);
			}
			else if (enumDataType == typeof(long))
			{
				ulongValue = (ulong)Convert.ToInt64(value);
			}
			else if (enumDataType == typeof(ulong))
			{
				ulongValue = Convert.ToUInt64(value);
			}
			else if (enumDataType == typeof(short))
			{
				ulongValue = (ulong)Convert.ToInt16(value);
			}
			else if (enumDataType == typeof(ushort))
			{
				ulongValue = Convert.ToUInt16(value);
			}
			else if (enumDataType == typeof(byte))
			{
				ulongValue = Convert.ToByte(value);
			}
			else if (enumDataType == typeof(sbyte))
			{
				ulongValue = (ulong)Convert.ToSByte(value);
			}

			return new InputArgument(ulongValue);
		}

		// Pointer Types - Operator
		public static unsafe implicit operator InputArgument(bool* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(int* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(uint* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(ulong* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(float* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(Vector3* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(sbyte* value) { return new InputArgument(new string(value)); }

		public override string ToString() { return data.ToString(); }
	}


	public static class Function
	{
		internal static ulong[] FillArgsArray(InputArgument[] arguments)
		{
			// The total number of args passed to the native
			int argCount = arguments.Length;
			// The number of var args passed to the native
			int variadicArgsCount = 0;

			if (argCount > 0)
			{
				// Templated natives have their var args param as the last one
				variadicArgsCount = arguments[arguments.Length - 1].arData.Count;
				// Check if this is a templated native, and get number of var args passed to it
				if (variadicArgsCount > 0)
				{
					// -1 because the variadic param array counts as +1 param length
					// which would end up making our array size 1 more than it should be
					argCount += variadicArgsCount - 1;
				}
			}

			ulong[] args = new ulong[argCount];

			// Add InputArgument.Data
			int len = (variadicArgsCount > 0 ? (arguments.Length - 1) : arguments.Length);
			for (int i = 0; i < len; i++)
			{
				args[i] = arguments[i].data;
			}

			// Add InputArgument.arData
			if (variadicArgsCount > 0)
			{
				for (int i = 0; i < variadicArgsCount; i++)
				{
					args[i + (args.Length - variadicArgsCount)] = arguments[arguments.Length - 1].arData[i];
				}
			}

			return args;
		}

		public static unsafe T Call<T>(ulong hash, params InputArgument[] arguments)
		{
			ulong[] args = FillArgsArray(arguments);

			var result = RDR2DN.NativeFunc.Invoke(hash, args);

			// The result will be null when this method is called from a thread other than the main thread
			if (result == null)
			{
				throw new InvalidOperationException("Native.Function.Call can only be called from the main thread.");
			}

			// Get native return value
			if (typeof(T).IsEnum || typeof(T).IsPrimitive || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector2))
			{
				return ObjectFromNative<T>(result);
			}
			else
			{
				return (T)ObjectFromNative(typeof(T), result);
			}
		}

		public static unsafe void Call(ulong hash, params InputArgument[] arguments)
		{
			ulong[] args = FillArgsArray(arguments);
			RDR2DN.NativeFunc.Invoke(hash, args);
		}

		internal static unsafe ulong ObjectToNative(object value)
		{
			if (value is null)
			{
				return 0;
			}

			if (value is bool valueBool)
			{
				return valueBool ? 1UL : 0UL;
			}
			if (value is int valueInt32)
			{
				// Prevent value from changing memory expression, in case the type is incorrect
				return (uint)valueInt32;
			}
			if (value is uint valueUInt32)
			{
				return valueUInt32;
			}
			if (value is float valueFloat)
			{
				return *(uint*)&valueFloat;
			}
			if (value is double valueDouble)
			{
				valueFloat = (float)valueDouble;
				return *(uint*)&valueFloat;
			}
			if (value is IntPtr valueIntPtr)
			{
				return (ulong)valueIntPtr.ToInt64();
			}
			if (value is string valueString)
			{
				// A C# null/empty string is different from a C++ null/empty string, which is 0.
				if (string.IsNullOrEmpty(valueString))
				{
					return 0;
				}

				return (ulong)RDR2DN.ScriptDomain.CurrentDomain.PinString(valueString).ToInt64();
			}

			// Scripting types
			if (value is Model valueModel)
			{
				return (ulong)valueModel.Hash;
			}
			if (typeof(INativeValue).IsAssignableFrom(value.GetType()))
			{
				return ((INativeValue)value).NativeValue;
			}

			throw new InvalidCastException(string.Concat("Unable to cast object of type '", value.GetType(), "' to native value"));
		}

		internal static unsafe T ObjectFromNative<T>(ulong* value)
		{
			if (typeof(T).IsEnum)
			{
				return NativeHelper<T>.Convert(*value);
			}

			if (typeof(T) == typeof(bool))
			{
				// Return proper boolean values (true if non-zero and false if zero)
				bool valueBool = *value != 0;
				return NativeHelper<T>.PtrToStructure(new IntPtr(&valueBool));
			}

			if (typeof(T) == typeof(int) || typeof(T) == typeof(uint) || typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(float))
			{
				return NativeHelper<T>.PtrToStructure(new IntPtr(value));
			}

			if (typeof(T) == typeof(double))
			{
				return NativeHelper<T>.Convert(NativeHelper<T>.PtrToStructure(new IntPtr(value)));
			}

			if (typeof(T) == typeof(Vector2) || typeof(T) == typeof(Vector3))
			{
				return NativeHelper<T>.Convert(*(scrVector3*)value);
			}

			if (typeof(T) == typeof(IntPtr))
			{
				return NativeHelper<T>.PtrToStructure(new IntPtr(value));
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", typeof(T), "'"));
		}

		internal static unsafe object ObjectFromNative(Type type, ulong* value)
		{
			if (type == typeof(string))
			{
				return RDR2DN.NativeMemory.PtrToStringUTF8(new IntPtr((byte*)*value));
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", type, "'"));
		}
	}
}
