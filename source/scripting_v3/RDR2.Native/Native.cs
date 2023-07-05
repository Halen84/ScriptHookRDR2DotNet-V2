//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace RDR2.Native
{
	public interface INativeValue
	{
		ulong NativeValue
		{
			get; set;
		}
	}

	internal static class NativeHelper<T>
	{
		static class CastCache<TFrom>
		{
			internal static readonly Func<TFrom, T> Cast;

			static CastCache()
			{
				ParameterExpression paramExp = Expression.Parameter(typeof(TFrom));
				UnaryExpression convertExp = Expression.Convert(paramExp, typeof(T));
				Cast = Expression.Lambda<Func<TFrom, T>>(convertExp, paramExp).Compile();
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
			return CastCache<TFrom>.Cast(from);
		}

		internal static T PtrToStructure(IntPtr ptr)
		{
			return _ptrToStrFunc(ptr);
		}
	}
	internal static class InstanceCreator<T1, TInstance>
	{
		internal static Func<T1, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructorInfo = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1) }, null);
			ParameterExpression arg1Exp = Expression.Parameter(typeof(T1));

			NewExpression newExp = Expression.New(constructorInfo, arg1Exp);
			var lambdaExp = Expression.Lambda<Func<T1, TInstance>>(newExp, arg1Exp);
			Create = lambdaExp.Compile();
		}
	}
	internal static class InstanceCreator<T1, T2, TInstance>
	{
		internal static Func<T1, T2, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructorInfo = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1), typeof(T2) }, null);
			ParameterExpression arg1Exp = Expression.Parameter(typeof(T1));
			ParameterExpression arg2Exp = Expression.Parameter(typeof(T2));

			NewExpression newExp = Expression.New(constructorInfo, arg1Exp, arg2Exp);
			var lambdaExp = Expression.Lambda<Func<T1, T2, TInstance>>(newExp, arg1Exp, arg2Exp);
			Create = lambdaExp.Compile();
		}
	}
	internal static class InstanceCreator<T1, T2, T3, TInstance>
	{
		internal static Func<T1, T2, T3, TInstance> Create;

		static InstanceCreator()
		{
			ConstructorInfo constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
				new[] { typeof(T1), typeof(T2), typeof(T3) }, null);
			ParameterExpression arg1 = Expression.Parameter(typeof(T1));
			ParameterExpression arg2 = Expression.Parameter(typeof(T2));
			ParameterExpression arg3 = Expression.Parameter(typeof(T3));

			NewExpression newExp = Expression.New(constructor, arg1, arg2, arg3);
			var lambdaExp = Expression.Lambda<Func<T1, T2, T3, TInstance>>(newExp, arg1, arg2, arg3);
			Create = lambdaExp.Compile();
		}
	}

	public class InputArgument
	{
		internal ulong data;
		internal List<ulong> variadicData = new List<ulong>();

		public InputArgument(ulong value) { data = value; }
		public InputArgument(object value) { data = Function.ObjectToNative(value); }
		public InputArgument(InputArgument[] value)
		{
			// Copy the data over
			for (int i = 0; i < value.Length; i++)
			{
				variadicData.Add(value[i].data);
			}
		}

		// Types - ctor
		public InputArgument([MarshalAs(UnmanagedType.U1)] bool value) : this(value ? 1UL : 0UL) { }
		public InputArgument(int value)     : this((uint)value) { }
		public InputArgument(uint value)    : this((ulong)value) { }
		public InputArgument(double value)  : this((float)value) { }
		public InputArgument(string value)  : this((object)value) { }
		public InputArgument(float value)   { unsafe { data = *(uint*)&value; } }
		public InputArgument(Vector3 value) : this((object)value) { }

		// Types - Operator
		public static implicit operator InputArgument([MarshalAs(UnmanagedType.U1)] bool value) { return value ? new InputArgument(1UL) : new InputArgument(0UL); }
		public static implicit operator InputArgument(InputArgument[] value) { return new InputArgument(value); } // Template Natives (e.g. VAR_STRING)
		public static implicit operator InputArgument(byte value)      { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(sbyte value)     { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(short value)     { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(ushort value)    { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(int value)       { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(uint value)      { return new InputArgument((ulong)value); }
		public static implicit operator InputArgument(float value)     { return new InputArgument(value); }
		public static implicit operator InputArgument(string value)    { return new InputArgument(value); }
		public static implicit operator InputArgument(double value)    { return new InputArgument((float)value); }
		public static implicit operator InputArgument(Vector3 value)   { return new InputArgument(value); }
		public static implicit operator InputArgument(Model value)     { return new InputArgument(value); }
		public static implicit operator InputArgument(AnimScene value) { return new InputArgument(value); }
		public static implicit operator InputArgument(Blip value)      { return new InputArgument(value); }
		public static implicit operator InputArgument(Camera value)    { return new InputArgument(value); }
		public static implicit operator InputArgument(Entity value)    { return new InputArgument(value); }
		public static implicit operator InputArgument(Ped value)       { return new InputArgument(value); }
		public static implicit operator InputArgument(Player value)    { return new InputArgument(value); }
		public static implicit operator InputArgument(Pickup value)    { return new InputArgument(value); }
		public static implicit operator InputArgument(Prop value)      { return new InputArgument(value); }
		public static implicit operator InputArgument(Rope value)      { return new InputArgument(value); }
		public static implicit operator InputArgument(Vehicle value)   { return new InputArgument(value); }
		public static implicit operator InputArgument(Volume value)    { return new InputArgument(value); }
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
		public static unsafe implicit operator InputArgument(bool* value)    { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(int* value)     { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(uint* value)    { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(ulong* value)   { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(float* value)   { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(Vector3* value) { return new InputArgument((ulong)new IntPtr(value).ToInt64()); }
		public static unsafe implicit operator InputArgument(sbyte* value)   { return new InputArgument(new string(value)); }

		public override string ToString() { return data.ToString(); }
	}


	/// <summary>
	/// An output argument passed to a script function.
	/// </summary>
	public class OutputArgument : InputArgument, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OutputArgument"/> class for script functions that output data into pointers.
		/// </summary>
		public OutputArgument() : base(Marshal.AllocCoTaskMem(24))
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OutputArgument"/> class with an initial value for script functions that require the pointer to data instead of the actual data.
		/// </summary>
		/// <param name="value">The value to set the data of this <see cref="OutputArgument"/> to.</param>
		public OutputArgument(object value) : this()
		{
			unsafe
			{
				*(ulong*)(data) = Function.ObjectToNative(value);
			}
		}

		~OutputArgument()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool disposing)
		{
			if (data != 0)
			{
				Marshal.FreeCoTaskMem((IntPtr)(long)data);
				data = 0;
			}
		}

		public T GetResult<T>()
		{
			unsafe
			{
				if (typeof(T).IsEnum || typeof(T).IsPrimitive || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector2))
				{
					return Function.ObjectFromNative<T>((ulong*)data);
				}
				else
				{
					return (T)Function.ObjectFromNative(typeof(T), (ulong*)data);
				}
			}
		}
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
				variadicArgsCount = arguments[arguments.Length - 1].variadicData.Count;
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

			// Add InputArgument.variadicData
			if (variadicArgsCount > 0)
			{
				for (int i = 0; i < variadicArgsCount; i++)
				{
					args[i + (args.Length - variadicArgsCount)] = arguments[arguments.Length - 1].variadicData[i];
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
				return NativeHelper<T>.Convert(*(NativeVector3*)value);
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

			// Scripting types
			if (type == typeof(AnimScene))
			{
				return new AnimScene(*(int*)value);
			}
			if (type == typeof(Blip))
			{
				return new Blip(*(int*)value);
			}
			if (type == typeof(Camera))
			{
				return new Camera(*(int*)value);
			}
			if (type == typeof(Entity))
			{
				return Entity.FromHandle(*(int*)value);
			}
			if (type == typeof(Ped))
			{
				return new Ped(*(int*)value);
			}
			if (type == typeof(Player))
			{
				return new Player(*(int*)value);
			}
			if (type == typeof(Prop))
			{
				return new Prop(*(int*)value);
			}
			if (type == typeof(Rope))
			{
				return new Rope(*(int*)value);
			}
			if (type == typeof(Vehicle))
			{
				return new Vehicle(*(int*)value);
			}
			if (type == typeof(Volume))
			{
				return new Volume(*(int*)value);
			}

			throw new InvalidCastException(string.Concat("Unable to cast native value to object of type '", type, "'"));
		}
	}
}
