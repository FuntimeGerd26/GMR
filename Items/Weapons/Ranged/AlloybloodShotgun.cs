using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AlloybloodShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Shotgun");
			Tooltip.SetDefault($" Shoots 6 bullets that can pass through tiles, and an extra one that pierces infinite times as long as the enemy hit isn't a boss");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 164;
			Item.height = 34;
			Item.rare = 6;
			Item.useTime = 20;
            Item.useAnimation = 20;
			Item.reuseDelay = 80;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 175);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 58;
			Item.crit = 4;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-52, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			int NumProjectiles = 6; // The humber of projectiles that this gun will shoot.

			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 14 degrees at max.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(14));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.75f);

				// Create a projectile.
				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, damage, knockback, player.whoAmI);
				Main.projectile[p].extraUpdates += 4;
				Main.projectile[p].penetrate += 1;
				Main.projectile[p].tileCollide = false;
			}
			Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ArkShotgunBullet>(), damage * 2, knockback, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item36, player.position);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MaskedPlagueShotgun");
			recipe.AddRecipeGroup("IronBar", 25);
			recipe.AddIngredient(ItemID.Wire, 38);
			recipe.AddIngredient(ItemID.HallowedBar, 28);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}