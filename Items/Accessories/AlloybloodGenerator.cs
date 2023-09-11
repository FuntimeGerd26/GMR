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
	public class AlloybloodGenerator : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Grants the 'Blood Overflow' buff\nIncreases movement speed and knockback by 18%\nIncreases all damage and attack speed 16%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 120);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 2);
			player.GetDamage(DamageClass.Generic) += 0.31f; // It looks higher, but the buff takes away damage, so i have fixed it
			player.GetAttackSpeed(DamageClass.Generic) += 0.16f;
			player.GetKnockback(DamageClass.Generic) += 0.18f;
			player.moveSpeed += 0.18f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 30);
			recipe.AddIngredient(ItemID.Wire, 45);
			recipe.AddIngredient(ItemID.TitaniumBar, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 30);
			recipe2.AddIngredient(ItemID.Wire, 45);
			recipe2.AddIngredient(ItemID.AdamantiteBar, 18);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}