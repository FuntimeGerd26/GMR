using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class FallenBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 20;
			Item.rare = 3;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 38;
			Item.crit = 7;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.FallenBulletPortal>();
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 2);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Ranged.FallenBulletPortal>()] <= 0)
			{
				Item.useAmmo = AmmoID.Bullet;
			}
			else
				Item.useAmmo = AmmoID.None;

			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Ranged.FallenBulletPortal>()] <= 0)
			{
				Projectile.NewProjectileDirect(source, position, velocity * 0f, ModContent.ProjectileType<Projectiles.Ranged.FallenBulletPortal>(), 0, knockback, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item36, player.position);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 18);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 18);
			recipe2.AddIngredient(ItemID.FallenStar, 5);
			recipe2.AddIngredient(ItemID.TissueSample, 15);
			recipe2.AddIngredient(ItemID.Bone, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}