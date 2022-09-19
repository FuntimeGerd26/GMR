using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR
{
	public class Classless : DamageClass
	{
		public override void SetStaticDefaults()
		{
			ClassName.SetDefault("base damage");
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Generic)
				return StatInheritanceData.Full;
			return new StatInheritanceData
			(
				damageInheritance: 0.1f, //Damage ups give only 10% of their increase
				critChanceInheritance: 0f,
				attackSpeedInheritance: 0.5f, //half speed increase from attack speed increases
				armorPenInheritance: 0f,
				knockbackInheritance: 0f
			);
		}

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			if (damageClass == DamageClass.Melee)
				return false;
			if (damageClass == DamageClass.Magic)
				return false;
			if (damageClass == DamageClass.Summon)
				return false;
			if (damageClass == DamageClass.Ranged)
				return false;

			return false;
		}
	}
}