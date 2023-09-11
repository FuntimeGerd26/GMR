using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AluminiumShuriken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Shuriken");
			Tooltip.SetDefault("The damage the projectile deals increases by 3 every 0.25 seconds it's on the air");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 46;
			Item.rare = 1;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 35);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.AluminiumShuriken>();
			Item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 18);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ItemID.FallenStar, 6);
			recipe.AddIngredient(ItemID.Shuriken, 35);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 18);
			recipe2.AddIngredient(ItemID.Glass, 20);
			recipe2.AddIngredient(ItemID.FallenStar, 6);
			recipe2.AddIngredient(ItemID.Shuriken, 35);
			recipe2.AddIngredient(null, "UpgradeCrystal", 30);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}