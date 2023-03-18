using RDR2.Native;

namespace RDR2
{
	public abstract class PedCoreAttribute
	{
		public abstract eAttributeCore CoreType { get; }

		protected Ped Ped = null;

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
		internal HealthCore(Ped ped) { base.Ped = ped; }
		public override eAttributeCore CoreType { get => eAttributeCore.Health; }
	}

	public class StaminaCore : PedCoreAttribute
	{
		internal StaminaCore(Ped ped) { base.Ped = ped; }
		public override eAttributeCore CoreType { get => eAttributeCore.Stamina; }
	}

	public class DeadEyeCore : PedCoreAttribute
	{
		internal DeadEyeCore(Ped ped) { base.Ped = ped; }
		public override eAttributeCore CoreType { get => eAttributeCore.DeadEye; }
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
