using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class VacuumBuilder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots 6 high velocity bullets and an extra one that pierces normal enemies, deals [c/66FF66:200%] damage when at full HP");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 106;
			Item.height = 38;
			Item.rare = 4;
			Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 145);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 46;
			Item.crit = 10;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.BulletHighVelocity;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-14, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.statLife == player.statLifeMax)
			{ 
				damage = damage * 2;
			}
			
			int NumProjectiles = 5; // The humber of projectiles that this gun will shoot.

			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 6 degrees at max.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 2f - Main.rand.NextFloat(0.75f);

				// Create a projectile.
				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
			}
			velocity *= 3f;
			Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>(), damage * 3, knockback, player.whoAmI);
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
			recipe3.AddIngredient(null, "PressureBuilder");
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();
		}
	}
}