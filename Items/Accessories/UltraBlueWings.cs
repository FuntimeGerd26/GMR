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
	public class UltraBlueWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 60 ticks = 1 seconds
			// Fly speed: 2
			// Acceleration multiplier: 1
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(30, 3f, 1.25f);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.value = Item.sellPrice(silver: 280);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.75f; // Falling glide speed
			ascentWhenRising = 0.35f; // Rising speed
			maxCanAscendMultiplier = 0.5f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PalladiumBar, 24);
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.CobaltBar, 24);
			recipe2.AddIngredient(ItemID.SoulofFlight, 20);
			recipe2.AddIngredient(ItemID.SoulofLight, 18);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}