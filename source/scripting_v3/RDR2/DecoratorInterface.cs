//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using RDR2.Native;
using RDR2;

namespace RDR2
{
	/// <summary>
	/// Represents the global decorator interface (internally <c>CDecoratorInterface</c>) where variables can be accessed across scripts.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Decorator names are hashed in the same way as <see cref="Game.GenerateHash(string)"/> before being used.
	/// </para>
	/// <para>
	/// Despite get/set methods only accepts <see cref="Entity"/>'s at the moment in this class, the relevant native functions 
	/// take any values associated to guids (e.g. entity handles).
	/// </para>
	/// </remarks>
	public static class DecoratorInterface
	{
		/// <summary>
		/// Adds or updates a bool decorator on the <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the <see cref="Entity"/> exists; otherwise, <see langword="false"/>.
		/// Does not check if <paramref name="decoratorName"/> is registered.
		/// </returns>
		public static bool SetBool(Entity entity, string decoratorName, bool value)
			=> DECORATOR.DECOR_SET_BOOL(entity, decoratorName, value);

		/// <summary>
		/// Adds or updates a float decorator on the <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the <see cref="Entity"/> exists; otherwise, <see langword="false"/>.
		/// Does not check if <paramref name="decoratorName"/> is registered.
		/// </returns>
		public static bool SetFloat(Entity entity, string decoratorName, float value)
			=> DECORATOR.DECOR_SET_FLOAT(entity, decoratorName, value);

		/// <summary>
		/// Adds or updates an int decorator on the <see cref="Entity"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the <see cref="Entity"/> exists; otherwise, <see langword="false"/>.
		/// Does not check if <paramref name="decoratorName"/> is registered.
		/// </returns>
		public static bool SetInt(Entity entity, string decoratorName, int value)
			=> DECORATOR.DECOR_SET_INT(entity, decoratorName, value);

		/// <summary>
		/// Gets the value of a bool decorator.
		/// </summary>
		/// <returns>The bool value of the decorator.</returns>
		public static bool GetBool(Entity entity, string decoratorName)
			=> DECORATOR.DECOR_GET_BOOL(entity, decoratorName);

		/// <summary>
		/// Gets the value of a float decorator.
		/// </summary>
		/// <returns>The float value of the decorator.</returns>
		public static float GetFloat(Entity entity, string decoratorName)
			=> DECORATOR.DECOR_GET_FLOAT(entity, decoratorName);

		/// <summary>
		/// Gets the value of a int decorator.
		/// </summary>
		/// <returns>The int value of the decorator.</returns>
		public static int GetInt(Entity entity, string decoratorName)
			=> DECORATOR.DECOR_GET_INT(entity, decoratorName);

		/// <summary>
		/// Queries to see if an <see cref="Entity"/> has the decorator applied to it.
		/// </summary>
		/// <returns><see langword="true"/> if the <see cref="Entity"/> has the decorator; otherwise, <see langword="false"/>.</returns>
		public static bool ExistsOn(Entity entity, string decoratorName)
			=> DECORATOR.DECOR_EXIST_ON(entity, decoratorName);

		/// <summary>
		/// Removes a decorator from an <see cref="Entity"/>.
		/// </summary>
		/// <returns><see langword="true"/> if the decorator exists; otherwise, <see langword="false"/>.</returns>
		public static bool Remove(Entity entity, string decoratorName)
			=> DECORATOR.DECOR_REMOVE(entity, decoratorName);

		/// <summary>
		/// Registers a decorator to be used as a specific type.
		/// </summary>
		public static void Register(string decoratorName, eDecoratorType type)
			=> DECORATOR.DECOR_REGISTER(decoratorName, (int)type);

		/// <summary>
		/// Queries to see if a registered decorator is of an expected type.
		/// </summary>
		/// <returns><see langword="true"/> if the decorator is registered as the specified type; otherwise, <see langword="false"/>.</returns>
		public static bool IsRegisteredAsType(string decoratorName, eDecoratorType type)
			=> DECORATOR.DECOR_IS_REGISTERED_AS_TYPE(decoratorName, (int)type);
	}
}
