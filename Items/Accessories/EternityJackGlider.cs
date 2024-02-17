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
	public class EternityJackGlider : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 60 ticks = 1 seconds
			// Fly speed: 2
			// Acceleration multiplier: 1
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(1, 0.01f, 1f);
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 200);
			Item.rare = 12;
			Item.expert = true;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.6f; // Falling glide speed
			ascentWhenRising = 0f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1f;
			constantAscend = 0f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.2f;
			player.maxFallSpeed += 0.25f;
			player.jumpSpeedBoost += 2f;
		}
	}
}