using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace GMR.Items.Weapons.Melee
{
	public class AlloybloodSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws a sword which chases your cursor");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 68;
			Item.rare = 4;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 70;
			Item.crit = 4;
			Item.knockBack = 0f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloybloodSpin>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AluminiumSword");
			recipe.AddIngredient(null, "AlloySword");
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddIngredient(null, "UpgradeCrystal", 50);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AluminiumSword");
			recipe2.AddIngredient(null, "AlloySword");
			recipe2.AddIngredient(ItemID.MythrilBar, 12);
			recipe2.AddIngredient(null, "UpgradeCrystal", 50);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}