using RDR2.Native;

namespace RDR2
{
	public abstract class PedCoreAttribute
	{
		/// <summary>
		/// The <see cref="eAttributeCore"/> of this <see cref="PedCoreAttribute"/>.
		/// </summary>
		public abstract eAttributeCore CoreType { get; }
		/// <summary>
		/// The <see cref="RDR2.Ped"/> to modify the <see cref="PedCoreAttribute"/> of.
		/// </summary>
		protected Ped Ped = null;

		public PedCoreAttribute(Ped ped)
		{
			this.Ped = ped;
		}

		public int Value
		{
			get => ATTRIBUTE._GET_ATTRIBUTE_CORE_VALUE(Ped, (int)CoreType);
			set => ATTRIBUTE._SET_ATTRIBUTE_CORE_VALUE(Ped, (int)CoreType, value);
		}

		public bool IsOverpowered
		{
			get => ATTRIBUTE._IS_ATTRIBUTE_CORE_OVERPOWERED(Ped, (int)CoreType);
		}

		public float OverpowerSecondsLeft
		{
			get => ATTRIBUTE._GET_ATTRIBUTE_CORE_OVERPOWER_SECONDS_LEFT(Ped, (int)CoreType);
		}

		public void SetOverpowered(float value, bool makeOverpoweredSound = true)
		{
			ATTRIBUTE._ENABLE_ATTRIBUTE_CORE_OVERPOWER(Ped, (int)CoreType, value, makeOverpoweredSound);
		}
	}

	public class HealthCore : PedCoreAttribute
	{
		internal HealthCore(Ped ped) : base(ped) { }
		public override eAttributeCore CoreType => eAttributeCore.Health;
	}

	public class StaminaCore : PedCoreAttribute
	{
		internal StaminaCore(Ped ped) : base(ped) { }
		public override eAttributeCore CoreType => eAttributeCore.Stamina;
	}

	public class DeadEyeCore : PedCoreAttribute
	{
		internal DeadEyeCore(Ped ped) : base(ped) { }
		public override eAttributeCore CoreType => eAttributeCore.DeadEye;
	}

	public class PedCoreAttribs
	{
		Ped _ped;

		internal PedCoreAttribs(Ped ped)
		{
			_ped = ped;

			Health = new HealthCore(_ped);
			Stamina = new StaminaCore(_ped);
			DeadEye = new DeadEyeCore(_ped);
		}

		public HealthCore Health;
		public StaminaCore Stamina;
		public DeadEyeCore DeadEye;
	}
}
