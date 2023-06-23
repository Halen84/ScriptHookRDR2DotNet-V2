//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System.Collections.Generic;

namespace RDR2
{
	public sealed class WeaponCollection
	{
		#region Fields
		readonly Ped owner;
		readonly Dictionary<uint, Weapon> weapons = new Dictionary<uint, Weapon>();
		#endregion

		internal WeaponCollection(Ped owner)
		{
			this.owner = owner;
		}

		public Weapon this[uint hash]
		{
			get
			{
				if (!weapons.TryGetValue(hash, out Weapon weapon))
				{
					if (!WEAPON.HAS_PED_GOT_WEAPON(owner.Handle, hash, 0, false))
					{
						return null;
					}

					weapon = new Weapon(owner, (eWeapon)hash);
					weapons.Add(hash, weapon);
				}

				return weapon;
			}
		}

		public Weapon Current
		{
			get
			{
				uint currentWeapon = WEAPON._GET_PED_CURRENT_HELD_WEAPON(owner.Handle);

				if (weapons.ContainsKey(currentWeapon))
				{
					return weapons[currentWeapon];
				}
				else
				{
					var weapon = new Weapon(owner, (eWeapon)currentWeapon);
					weapons.Add(currentWeapon, weapon);

					return weapon;
				}
			}
		}

		public Weapon BestWeapon
		{
			get
			{
				uint hash = WEAPON.GET_BEST_PED_WEAPON(owner.Handle, false, false);

				if (weapons.ContainsKey(hash))
				{
					return weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, (eWeapon)hash);
					weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public bool HasWeapon(uint wHash)
		{
			return WEAPON.HAS_PED_GOT_WEAPON(owner.Handle, (uint)wHash, 0, false);
		}

		public bool IsWeaponValid(uint hash)
		{
			return WEAPON.IS_WEAPON_VALID(hash);
		}

		public Entity CurrentWeaponEntity
		{
			get
			{
				if (Current.Hash == eWeapon.Unarmed)
				{
					return null;
				}

				return Entity.FromHandle(WEAPON.GET_CURRENT_PED_WEAPON_ENTITY_INDEX(owner.Handle, 0));
			}
		}

		public bool Select(Weapon weapon)
		{
			if (!weapon.PedHasThisWeapon)
			{
				return false;
			}

			WEAPON.SET_CURRENT_PED_WEAPON(owner.Handle, (uint)weapon.Hash, true, 0, false, false);

			return true;
		}
		public bool Select(uint wHash)
		{
			return Select(wHash, true);
		}
		public bool Select(uint wHash, bool equipNow)
		{
			if (!WEAPON.HAS_PED_GOT_WEAPON(owner.Handle, wHash, 0, false))
			{
				return false;
			}

			WEAPON.SET_CURRENT_PED_WEAPON(owner.Handle, wHash, equipNow, 0, false, false);

			return true;
		}

		public Weapon Give(uint hash, int ammoCount)
		{
			if (!weapons.TryGetValue(hash, out Weapon weapon))
			{
				weapon = new Weapon(owner, (eWeapon)hash);
				weapons.Add(hash, weapon);
			}

			if (weapon.PedHasThisWeapon)
			{
				Select(weapon);
			}
			else
			{
				WEAPON.GIVE_DELAYED_WEAPON_TO_PED(owner.Handle, (uint)weapon.Hash, ammoCount, false, (uint)eAddItemReason.Default);
			}

			return weapon;
		}


		public void Remove(Weapon weapon)
		{
			var hash = weapon.Hash;

			if (weapons.ContainsKey((uint)hash))
			{
				weapons.Remove((uint)hash);
			}

			Remove((uint)weapon.Hash);
		}
		public void Remove(uint wHash)
		{
			WEAPON.REMOVE_WEAPON_FROM_PED(owner.Handle, (uint)wHash, false, (uint)eRemoveItemReason.Default);
		}

		public void RemoveAll()
		{
			WEAPON.REMOVE_ALL_PED_WEAPONS(owner.Handle, true, true);

			weapons.Clear();
		}
	}
}
