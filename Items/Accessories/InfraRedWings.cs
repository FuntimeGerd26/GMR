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
	public class InfraRedWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows the player to fly\nIncreases damage and attack speed by 5%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Fly time: 120 ticks = 2 seconds
			// Fly speed: 3
			// Acceleration multiplier: 1.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 3f, 1.5f);
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 36;
			Item.value = Item.sellPrice(silver: 180);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.90f; // Falling glide speed
			ascentWhenRising = 0.5f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.45f;
			constantAscend = 0.35f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 15);
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}