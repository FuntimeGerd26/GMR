using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class DuneSearcherShotgun : ModItem
	{
		public int ShotSpeed;
		public int ShotLoading;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dune Searcher Shotgun");
			Tooltip.SetDefault("'That'll hurt'" +
				$"\nShoots 5 high velocity bullets that can pierce 3 extra times, and has a chance to shoot an extra one that explodes when hitting an enemy, and deals 50% more damage to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 144;
			Item.height = 34;
			Item.rare = 5;
			Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 275);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-48, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (ShotLoading > 0)
				damage *= 3;

			int NumProjectiles = 5; // The humber of projectiles that this gun will shoot.

				for (int i = 0; i < NumProjectiles; i++)
				{
					// Rotate the velocity randomly by 30 degrees at max.
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(ShotLoading > 0 ? 6 : 14));

					// Decrease velocity randomly for nicer visuals.
					newVelocity *= 1f - Main.rand.NextFloat(0.5f);

					// Create a projectile.
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, damage, knockback, player.whoAmI);
					Main.projectile[p].extraUpdates += 9;
					Main.projectile[p].penetrate += 3;
				}
				if (Main.rand.NextBool(2))
					Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.DuneSearcherBullet>(), damage * 2, knockback, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item36, player.position);
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MaskedPlagueShotgun");
			recipe.AddIngredient(null, "PressureBuilder");
			recipe.AddIngredient(ItemID.SandBlock, 80);
			recipe.AddIngredient(ItemID.HallowedBar, 28);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}