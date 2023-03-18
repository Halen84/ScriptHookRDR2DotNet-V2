//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System;

namespace RDR2
{
	public struct RelationshipGroup : INativeValue, IEquatable<RelationshipGroup>
	{
		RelationshipGroup(string name) : this()
		{
			uint hashArg;
			unsafe
			{
				PED.ADD_RELATIONSHIP_GROUP(name, &hashArg);
			}

			Hash = (int)hashArg;
		}
		public RelationshipGroup(int hash) : this()
		{
			Hash = hash;
		}
		public RelationshipGroup(uint hash) : this((int)hash)
		{
		}

		/// <summary>
		/// Gets the hash for this <see cref="RelationshipGroup"/>.
		/// </summary>
		public int Hash
		{
			get; private set;
		}

		/// <summary>
		/// Gets the native representation of this <see cref="RelationshipGroup"/>.
		/// </summary>
		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = unchecked((int)value);
		}

		public eRelationshipType GetRelationshipBetweenGroups(RelationshipGroup targetGroup)
		{
			return (eRelationshipType)PED.GET_RELATIONSHIP_BETWEEN_GROUPS((uint)Hash, (uint)targetGroup.NativeValue);
		}

		public void SetRelationshipBetweenGroups(RelationshipGroup targetGroup, eRelationshipType relationship, bool bidirectionally = false)
		{
			PED.SET_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, (uint)Hash, (uint)targetGroup.NativeValue);

			if (bidirectionally)
			{
				PED.SET_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, (uint)targetGroup.NativeValue, (uint)Hash);
			}
		}

		public void ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, eRelationshipType relationship, bool bidirectionally = false)
		{
			PED.CLEAR_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, (uint)Hash, (uint)targetGroup.NativeValue);

			if (bidirectionally)
			{
				PED.CLEAR_RELATIONSHIP_BETWEEN_GROUPS((int)relationship, (uint)targetGroup.NativeValue, (uint)Hash);
			}
		}

		public void Remove()
		{
			PED.REMOVE_RELATIONSHIP_GROUP((uint)Hash);
		}

		public bool Equals(RelationshipGroup group)
		{
			return Hash == group.Hash;
		}
		public override bool Equals(object obj)
		{
			if (obj is RelationshipGroup group)
			{
				return Equals(group);
			}

			return false;
		}

		public static bool operator ==(RelationshipGroup left, RelationshipGroup right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(RelationshipGroup left, RelationshipGroup right)
		{
			return !(left == right);
		}

		public static implicit operator RelationshipGroup(int source)
		{
			return new RelationshipGroup(source);
		}
		public static implicit operator RelationshipGroup(uint source)
		{
			return new RelationshipGroup(source);
		}
		public static implicit operator RelationshipGroup(string source)
		{
			return new RelationshipGroup(source);
		}

		public static implicit operator InputArgument(RelationshipGroup value)
		{
			return new InputArgument((ulong)value.Hash);
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return "0x" + Hash.ToString("X");
		}
	}
}
