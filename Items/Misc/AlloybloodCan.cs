using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Items.Misc
{
	public class AlloybloodCan : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 26;
			Item.rare = 0;
		}

		public override bool OnPickup(Player player)
		{
			player.Heal(player.statLife / 4);
			player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 300);
			Item.active = false;
			Item.TurnToAir();
			return false;
		}
	}
}