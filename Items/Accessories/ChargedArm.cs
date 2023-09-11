using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;
namespace GMR.Items.Accessories
{
	public class ChargedArm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Rocket Arm");
			Tooltip.SetDefault($"Ranged weapons have a chance to shoot a homing rocket\nIncreases summoner damage by 12%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 86;
			Item.value = Item.sellPrice(silver: 240);
			Item.rare = 3;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.12f;
			player.GPlayer().ChargedArm = Item;
		}
	}
}