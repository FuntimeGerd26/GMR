using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class PressureBuilder : ModItem
	{
		public int ShotSpeed;
		public int ShotLoading;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots 6 high velocity bullets that can pierce 3 extra times and an extra one that moves x3 as fast, and deals double damage" +
				"\nDeals [c/66FF66:300%] more damage when under [c/FF4444:50%] HP");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 110;
			Item.height = 38;
			Item.rare = 4;
			Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 145);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 58;
			Item.crit = 10;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.BulletHighVelocity;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.statLife >= player.statLifeMax / 2)
				damage = damage * 3;

				int NumProjectiles = ShotLoading > 0 ? 8 : 5; // The humber of projectiles that this gun will shoot.

				for (int i = 0; i < NumProjectiles; i++)
				{
					// Rotate the velocity randomly by 8 degrees at max.
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(ShotLoading > 0 ? 2 : 6));

					// Decrease velocity randomly for nicer visuals.
					newVelocity *= 2f - Main.rand.NextFloat(0.75f);

					// Create a projectile.
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
					Main.projectile[p].penetrate += 3;
				}
				velocity *= 3f;
				Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>(), damage * 2, knockback, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item36, player.position);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AsphaltBlock, 40);
			recipe.AddIngredient(ItemID.CobaltBar, 28);
			recipe.AddIngredient(null, "UpgradeCrystal", 60);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.AsphaltBlock, 40);
			recipe2.AddIngredient(ItemID.PalladiumBar, 28);
			recipe2.AddIngredient(null, "UpgradeCrystal", 60);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "VacuumBuilder");
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();
		}
	}
}