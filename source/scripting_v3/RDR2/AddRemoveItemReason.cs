namespace RDR2
{
	public enum eAddItemReason : uint
	{
		Awards = 0xB784AD1E,
		CreateCharacter = 0xE2C4FF71,
		Debug = 0x5C05C64D,
		Default = 0x2CD419DC,
		GetInventory = 0xD8188685,
		Incentive = 0x8ADC2E95,
		Loadout = 0xCA3454E6,
		LoadSavegame = 0x56212906,
		Looted = 0xCA806A55,
		Melee = 0x7B9BDCE7,
		MPMission = 0xEC0E0194,
		Notification = 0xC56292D2,
		Pickup = 0x1A770E22,
		Purchased = 0x4A6726C9,
		SetAmount = 0x4504731E,
		Syncing = 0x8D4B4FF4,
		UseFailed = 0xD385B670
	};

	public enum eRemoveItemReason : uint
	{
		ClientPurged = 0x4A4E94DC,
		Coalesce = 0x2ABE393E,
		Debug = 0xA07362E6,
		Default = 0xF77DE93D,
		DeleteCharacter = 0x20AFBDE9,
		Dropped = 0xEC7FB5D5,
		Duplicate = 0x19047132,
		GiftedIncorrectly = 0x9C4E3829,
		Given = 0xAD5377D4,
		InsufficientInventory = 0x518D1AAE,
		ItemDoesNotExist = 0xEAD5D889,
		Loadout = 0x1B94E3BA,
		SetAmount = 0x19D5CFA5,
		Sold = 0x76C4B482,
		Used = 0x2188E0A3,
		UseFailed = 0x671F9EAD
	};
}
