using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class BiomeSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.rare = 1;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 5);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 10;
			Item.crit = -3;
			Item.knockBack = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.BiomeSwordBeam>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("Wood", 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}