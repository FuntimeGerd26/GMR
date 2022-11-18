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
	public class IceyWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows the player to fly");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 120 ticks = 2 seconds
			// Fly speed: 6
			// Acceleration multiplier: 1.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 6f, 1.5f);
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 32;
			Item.value = Item.sellPrice(silver: 120);
			Item.rare = 5;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.75f; // Falling glide speed
			ascentWhenRising = 0.10f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2f;
			constantAscend = 0.11f;
		}
	}
}