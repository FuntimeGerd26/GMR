using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class BiomeSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refined Wooden Sword");
			Tooltip.SetDefault("'Slightly better than before'\nShoots a beam that has just enough range to hit enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 52;
			Item.rare = 1;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 5);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 10;
			Item.crit = 2;
			Item.knockBack = 0.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.BiomeSwordBeam>();
			Item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("Wood", 7);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}