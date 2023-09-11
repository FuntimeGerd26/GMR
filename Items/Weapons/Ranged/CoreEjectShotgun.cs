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
			Tooltip.SetDefault($" Shoots a spread of 10 bullets, along a special one\n Right click to shoot an explosive core that explodes dealing triple damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 62;
			Item.height = 26;
			Item.rare = 5;
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

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.reuseDelay = 80;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>();
				Item.shootSpeed = 12f;
				Item.UseSound = SoundID.Item61;
			}
			else
			{
				Item.reuseDelay = 20;
				Item.shoot = ProjectileID.Bullet;
				Item.shootSpeed = 16f;
				Item.UseSound = SoundID.Item36;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunCore>(), damage * 3, knockback, player.whoAmI);
				return false;
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)), type, damage, knockback, player.whoAmI);
				}
				Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>(), damage * 2, knockback, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item36, player.position);
				return false; // Return false because we don't want tModLoader to shoot projectile
			}

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AluminiumGun");
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(ItemID.HallowedBar, 14);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}