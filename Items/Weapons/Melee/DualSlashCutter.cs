using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 70;
			Item.rare = 4;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.crit = 0;
			Item.knockBack = 4.5f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DualSlashCutterSwing>();
			Item.shootSpeed = 6f;
		}

		float flipSwingDir;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (flipSwingDir == 0f)
				flipSwingDir = 1f;
			flipSwingDir *= -1f;

			if (flipSwingDir >= 1f)
				type = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DualSlashCutterSwing>();
			if (flipSwingDir <= -1f)
				type = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.DualSlashCutterSwingFlip>();
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 18);
			recipe.AddIngredient(ItemID.SoulofNight, 22);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipeAlt = CreateRecipe();
			recipeAlt.AddIngredient(ItemID.PalladiumBar, 18);
			recipeAlt.AddIngredient(ItemID.SoulofNight, 22);
			recipeAlt.AddIngredient(ItemID.SoulofLight, 18);
			recipeAlt.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipeAlt.AddTile(TileID.Anvils);
			recipeAlt.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualGunShooter");
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}