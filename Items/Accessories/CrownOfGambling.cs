using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class CrownOfGambling : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 380);
			Item.rare = 3;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().GambleCrown = Item;
			player.GetDamage(DamageClass.Generic) *= 1.5f;
			player.GetCritChance(DamageClass.Generic) *= 0f;
			player.GetKnockback(DamageClass.Generic) *= 0.5f;
			player.statDefense -= player.statDefense / 2;
		}
	}
}