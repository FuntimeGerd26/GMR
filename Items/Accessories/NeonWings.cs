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
	public class NeonWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows the player to fly");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 60 ticks = 1 seconds
			// Fly speed: 2
			// Acceleration multiplier: 1
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(60, 2f, 1f);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 180);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.55f; // Falling glide speed
			ascentWhenRising = 0.25f; // Rising speed
			maxCanAscendMultiplier = 0.25f;
			maxAscentMultiplier = 2f;
			constantAscend = 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.SoulofFlight, 12);
			recipe.AddIngredient(ItemID.PearlstoneBlock, 28);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}