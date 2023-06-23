using RDR2.Native;

namespace RDR2
{
	public sealed class Weapon
	{
		private Ped Owner { get; }
		public eWeapon Hash { get; }

		public Weapon()
		{
		}

		public Weapon(Ped owner, eWeapon weaponHash)
		{
			Owner = owner;
			Hash = weaponHash;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is valid.
		/// </summary>
		public bool IsValid => WEAPON.IS_WEAPON_VALID((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> has flag CWeaponInfoFlags::Flags::Gun set.
		/// </summary>
		public bool IsGun => WEAPON.IS_WEAPON_A_GUN((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is a bow.
		/// </summary>
		public bool IsBow => WEAPON.IS_WEAPON_BOW((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is a melee weapon.
		/// </summary>
		public bool IsMelee => WEAPON.IS_WEAPON_MELEE_WEAPON((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is a pistol.
		/// </summary>
		public bool IsPistol => WEAPON.IS_WEAPON_PISTOL((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is repeater.
		/// </summary>
		public bool IsRepeater => WEAPON.IS_WEAPON_REPEATER((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is rifle.
		/// </summary>
		public bool IsRifle => WEAPON.IS_WEAPON_RIFLE((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is a sniper.
		/// </summary>
		public bool IsSniper => WEAPON._IS_WEAPON_SNIPER((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is shotgun.
		/// </summary>
		public bool IsShotgun => WEAPON.IS_WEAPON_SHOTGUN((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is a kit weapon.
		/// </summary>
		public bool IsKit => WEAPON._IS_WEAPON_KIT((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is one handed.
		/// </summary>
		public bool IsOneHanded => WEAPON._IS_WEAPON_ONE_HANDED((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is two handed.
		/// </summary>
		public bool IsTwoHanded => WEAPON._IS_WEAPON_TWO_HANDED((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> is throwable.
		/// </summary>
		public bool IsThrowable => WEAPON._IS_WEAPON_THROWABLE((uint)Hash);

		/// <summary>
		/// Gets the amount of ammo this <see cref="Weapon"/> can hold in its clip.
		/// </summary>
		public int ClipSize => WEAPON.GET_WEAPON_CLIP_SIZE((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Weapon"/> can be twirled.
		/// </summary>
		public bool CanBeTwirled => WEAPON._GET_CAN_TWIRL_WEAPON((uint)Hash);

		/// <summary>
		/// Get the <see cref="RDR2.Model"/> of this <see cref="Weapon"/>
		/// </summary>
		public Model Model => (Model)WEAPON._GET_WEAPONTYPE_MODEL((uint)Hash);

		/// <summary>
		/// Get the <see cref="eWeaponGroup"/> of this <see cref="Weapon"/>
		/// </summary>
		public eWeaponGroup Group => (eWeaponGroup)WEAPON.GET_WEAPONTYPE_GROUP((uint)Hash);

		/// <summary>
		/// Get the slot hash of this <see cref="Weapon"/>
		/// </summary>
		public uint SlotHash => WEAPON._GET_WEAPONTYPE_SLOT((uint)Hash);

		/// <summary>
		/// Get the name of this <see cref="Weapon"/>
		/// </summary>
		public string Name => WEAPON._GET_WEAPON_NAME((uint)Hash);

		/// <summary>
		/// Gets a value indicating whether the <see cref="Owner"/> has this <see cref="Weapon"/>.
		/// </summary>
		public bool PedHasThisWeapon => WEAPON.HAS_PED_GOT_WEAPON(Owner.Handle, (uint)Hash, 0, false);

		/// <summary>
		/// Gets a value indicating whether the <see cref="Owner"/> is carrying this <see cref="Weapon"/>.
		/// </summary>
		public bool IsBeingCarried => WEAPON.IS_PED_CARRYING_WEAPON(Owner.Handle, (uint)Hash);

		/// <summary>
		/// Gets the <see cref="eAmmoType"/> that this weapon uses by default.
		/// </summary>
		public eAmmoType DefaultAmmoType => (eAmmoType)WEAPON._GET_AMMO_TYPE_FOR_WEAPON((uint)Hash);

		/// <summary>
		/// Gets the <see cref="eAmmoType"/> that this weapon currently using.
		/// </summary>
		public eAmmoType CurrentAmmoType => (eAmmoType)WEAPON.GET_PED_AMMO_TYPE_FROM_WEAPON(Owner.Handle, (uint)Hash);

		/// <summary>
		/// Sets this <see cref="Weapon"/> to have infinite ammo.
		/// </summary>
		public bool InfiniteAmmo { set => WEAPON.SET_PED_INFINITE_AMMO(Owner.Handle, value, (uint)Hash); }

		/// <summary>
		/// Get or set the total amount of ammo that this <see cref="Weapon"/> has.
		/// </summary>
		public int Ammo
		{
			get => WEAPON.GET_PED_AMMO_BY_TYPE(Owner.Handle, (uint)CurrentAmmoType);
			set => WEAPON.SET_PED_AMMO_BY_TYPE(Owner.Handle, (uint)CurrentAmmoType, value);
		}

		/// <summary>
		/// Get or set the amount of ammo that this <see cref="Weapon"/> has in its clip.
		/// </summary>
		public unsafe int AmmoInClip
		{
			get
			{
				int ammo;

				if (WEAPON.GET_AMMO_IN_CLIP(Owner.Handle, &ammo, (uint)Hash))
				{
					return ammo;
				}

				return 0;
			}

			set
			{
				if (PedHasThisWeapon)
				{
					WEAPON.SET_AMMO_IN_CLIP(Owner.Handle, (uint)Hash, value);
				}
			}
		}

		/// <summary>
		/// Get the max amount of ammo that this <see cref="Weapon"/> has.
		/// </summary>
		public unsafe int MaxAmmo
		{
			get
			{
				int ammo;

				if (WEAPON.GET_MAX_AMMO(Owner.Handle, &ammo, (uint)Hash))
				{
					return ammo;
				}

				return 0;
			}
		}

		public void RemoveAmmo(int amount)
		{
			WEAPON._REMOVE_AMMO_FROM_PED(Owner.Handle, (uint)Hash, amount, (uint)eRemoveItemReason.Default);
		}

		public void AddAmmo(int amount)
		{
			WEAPON._ADD_AMMO_TO_PED(Owner.Handle, (uint)Hash, amount, (uint)eAddItemReason.Default);
		}

		public void SetAmmoType(eAmmoType ammoType)
		{
			WEAPON._SET_AMMO_TYPE_FOR_PED_WEAPON(Owner.Handle, (uint)Hash, (uint)ammoType);
		}

		public void Equip()
		{
			WEAPON.SET_CURRENT_PED_WEAPON(Owner.Handle, (uint)Hash, true, 0, false, false);
		}

		public void Remove()
		{
			WEAPON.REMOVE_WEAPON_FROM_PED(Owner.Handle, (uint)Hash, true, (uint)eRemoveItemReason.Default);
		}

		public static implicit operator eWeapon(Weapon weapon)
		{
			return weapon.Hash;
		}
	}
}
