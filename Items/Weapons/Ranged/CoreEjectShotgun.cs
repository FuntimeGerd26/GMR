using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class CoreEjectShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Eject Shotgun");
			Tooltip.SetDefault("Shoots a spread of 10 bullets\nRight click to shoot an explosive core that explodes and deals double damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 62;
			Item.height = 26;
			Item.rare = 4;
			Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 165);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 40;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.reuseDelay = 120;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>();
				Item.shootSpeed = 10f;
				Item.UseSound = SoundID.Item61;
			}
			else
			{
				Item.reuseDelay = 20;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
				Item.shootSpeed = 18f;
				Item.UseSound = SoundID.Item36;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>(), damage, knockback, player.whoAmI);
				return false;
			}
			else
			{
				const int NumProjectiles = 10; // The humber of projectiles that this gun will shoot.

				for (int i = 0; i < NumProjectiles; i++)
				{
					// Rotate the velocity randomly by 30 degrees at max.
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));

					// Decrease velocity randomly for nicer visuals.
					newVelocity *= 1f - Main.rand.NextFloat(0.2f);

					// Create a projectile.
					Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>(), damage, knockback, player.whoAmI);
				}
				SoundEngine.PlaySound(SoundID.Item36, player.position);
				return false; // Return false because we don't want tModLoader to shoot projectile
			}

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ChargeShotgun");
			recipe.AddIngredient(null, "AluminiumRifle");
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 14);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}