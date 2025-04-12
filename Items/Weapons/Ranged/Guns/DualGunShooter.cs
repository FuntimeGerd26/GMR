using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class DualGunShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Shoot!'\nBullets home into enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 30;
			Item.rare = 4;
			Item.useTime = 12;
			Item.useAnimation = 20;
			Item.reuseDelay = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 225);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 64;
			Item.crit = 2;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.DualShooterBullet>();
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.DualShooterBullet>();
				SoundEngine.PlaySound(SoundID.Item41, player.position);
			}
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
			recipe2.AddIngredient(null, "DualSlashCutter");
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}