using RDR2.Native;

namespace RDR2
{
	public abstract class PedAttribute
	{
		public abstract ePedAttribute AttribType { get; }

		protected Ped Ped = null;

		public int Rank
		{
			get => ATTRIBUTE.GET_ATTRIBUTE_RANK(Ped, (int)AttribType);
		}

		public int BaseRank
		{
			get => ATTRIBUTE.GET_ATTRIBUTE_BASE_RANK(Ped, (int)AttribType);
			set => ATTRIBUTE.SET_ATTRIBUTE_BASE_RANK(Ped, (int)AttribType, value);
		}

		public int BonusRank
		{
			get => ATTRIBUTE.GET_ATTRIBUTE_BONUS_RANK(Ped, (int)AttribType);
			set => ATTRIBUTE.SET_ATTRIBUTE_BONUS_RANK(Ped, (int)AttribType, value);
		}

		public int MaxRank
		{
			get => ATTRIBUTE.GET_MAX_ATTRIBUTE_RANK(Ped, (int)AttribType);
		}

		public int Points
		{
			get => ATTRIBUTE.GET_ATTRIBUTE_POINTS(Ped, (int)AttribType);
			set => ATTRIBUTE.SET_ATTRIBUTE_POINTS(Ped, (int)AttribType, value);
		}

		public int MaxPoints
		{
			get => ATTRIBUTE.GET_MAX_ATTRIBUTE_POINTS(Ped, (int)AttribType);
		}

		public bool IsOverpowered
		{
			get => ATTRIBUTE._IS_ATTRIBUTE_OVERPOWERED(Ped, (int)AttribType);
		}

		public float OverpowerSecondsLeft
		{
			get => ATTRIBUTE._GET_ATTRIBUTE_OVERPOWER_SECONDS_LEFT(Ped, (int)AttribType);
		}

		public void SetOverpowered(bool overpower, float value, bool makeOverpoweredSound = true)
		{
			if (overpower)
			{
				ATTRIBUTE.ENABLE_ATTRIBUTE_OVERPOWER(Ped, (int)AttribType, value, makeOverpoweredSound);
			}
			else
			{
				ATTRIBUTE.DISABLE_ATTRIBUTE_OVERPOWER(Ped, (int)AttribType);
			}
		}
	}


	public class HealthAttribute : PedAttribute
	{
		internal HealthAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Health;
	}

	public class StaminaAttribute : PedAttribute
	{
		internal StaminaAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Stamina;
	}

	public class DeadEyeAttribute : PedAttribute
	{
		internal DeadEyeAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.DeadEye;
	}

	public class CourageAttribute : PedAttribute
	{
		internal CourageAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Courage;
	}

	public class AgilityAttribute : PedAttribute
	{
		internal AgilityAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Agility;
	}

	public class SpeedAttribute : PedAttribute
	{
		internal SpeedAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Speed;
	}

	public class AccelerationAttribute : PedAttribute
	{
		internal AccelerationAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Acceleration;
	}

	public class BondingAttribute : PedAttribute
	{
		internal BondingAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Bonding;
	}

	public class HungerAttribute : PedAttribute
	{
		internal HungerAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Hunger;
	}

	public class FatiguedAttribute : PedAttribute
	{
		internal FatiguedAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Fatigued;
	}

	public class InebriatedAttribute : PedAttribute
	{
		internal InebriatedAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Inebriated;
	}

	public class PoisonedAttribute : PedAttribute
	{
		internal PoisonedAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Poisoned;
	}

	public class BodyHeatAttribute : PedAttribute
	{
		internal BodyHeatAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.BodyHeat;
	}

	public class BodyWeightAttribute : PedAttribute
	{
		internal BodyWeightAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.BodyWeight;
	}

	public class OverfedAttribute : PedAttribute
	{
		internal OverfedAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Overfed;
	}

	public class SicknessAttribute : PedAttribute
	{
		internal SicknessAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Sickness;
	}

	public class DirtinessAttribute : PedAttribute
	{
		internal DirtinessAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Dirtiness;
	}

	public class DirtinessHatAttribute : PedAttribute
	{
		internal DirtinessHatAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.DirtinessHat;
	}

	public class StrengthAttribute : PedAttribute
	{
		internal StrengthAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Strength;
	}

	public class GritAttribute : PedAttribute
	{
		internal GritAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Grit;
	}

	public class InstinctAttribute : PedAttribute
	{
		internal InstinctAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Instinct;
	}

	public class UnrulinessAttribute : PedAttribute
	{
		internal UnrulinessAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.Unruliness;
	}

	public class DirtinessSkinAttribute : PedAttribute
	{
		internal DirtinessSkinAttribute(Ped ped) { base.Ped = ped; }
		public override ePedAttribute AttribType => ePedAttribute.DirtinessSkin;
	}

	public class PedAttribs
	{
		private Ped _ped;

		internal PedAttribs(Ped ped)
		{
			_ped = ped;

			Health = new HealthAttribute(_ped);
			Stamina = new StaminaAttribute(_ped);
			DeadEye = new DeadEyeAttribute(_ped);
			Courage = new CourageAttribute(_ped);
			Agility = new AgilityAttribute(_ped);
			Speed = new SpeedAttribute(_ped);
			Acceleration = new AccelerationAttribute(_ped);
			Bonding = new BondingAttribute(_ped);
			Hunger = new HungerAttribute(_ped);
			Fatigued = new FatiguedAttribute(_ped);
			Inebriated = new InebriatedAttribute(_ped);
			Poisoned = new PoisonedAttribute(_ped);
			BodyHeat = new BodyHeatAttribute(_ped);
			BodyWeight = new BodyWeightAttribute(_ped);
			Overfed = new OverfedAttribute(_ped);
			Sickness = new SicknessAttribute(_ped);
			Dirtiness = new DirtinessAttribute(_ped);
			DirtinessHat = new DirtinessHatAttribute(_ped);
			Strength = new StrengthAttribute(_ped);
			Grit = new GritAttribute(_ped);
			Instinct = new InstinctAttribute(_ped);
			Unruliness = new UnrulinessAttribute(_ped);
			DirtinessSkin = new DirtinessSkinAttribute(_ped);
		}

		public HealthAttribute Health;
		public StaminaAttribute Stamina;
		public DeadEyeAttribute DeadEye;
		public CourageAttribute Courage;
		public AgilityAttribute Agility;
		public SpeedAttribute Speed;
		public AccelerationAttribute Acceleration;
		public BondingAttribute Bonding;
		public HungerAttribute Hunger;
		public FatiguedAttribute Fatigued;
		public InebriatedAttribute Inebriated;
		public PoisonedAttribute Poisoned;
		public BodyHeatAttribute BodyHeat;
		public BodyWeightAttribute BodyWeight;
		public OverfedAttribute Overfed;
		public SicknessAttribute Sickness;
		public DirtinessAttribute Dirtiness;
		public DirtinessHatAttribute DirtinessHat;
		public StrengthAttribute Strength;
		public GritAttribute Grit;
		public InstinctAttribute Instinct;
		public UnrulinessAttribute Unruliness;
		public DirtinessSkinAttribute DirtinessSkin;
	}
}
