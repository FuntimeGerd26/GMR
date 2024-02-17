using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class SpazCape : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows the player to fly");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 60 ticks = 1 seconds
			// Fly speed: 4
			// Acceleration multiplier: 1.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(60, 4f, 1.5f);
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 220);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.45f; // Falling glide speed
			ascentWhenRising = 0.15f; // Rising speed
			maxCanAscendMultiplier = 0.5f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.10f;
		}
	}
}