using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Ranged
{
	public class DuneSearcherPrime : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 172;
			Item.height = 38;
			Item.rare = 9;
			Item.useTime = 5;
			Item.useAnimation = 20;
			Item.reuseDelay = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 450);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 50;
			Item.crit = 6;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-54, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;
			position += Vector2.Normalize(velocity);
			int NumProjectiles = 7; // The humber of projectiles that this gun will shoot.

			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.2f, 0.5f);

				// Create a projectile.
				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, damage, knockback, player.whoAmI);
				Main.projectile[p].extraUpdates += 4;
			}
			Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.PrimeBullet>(), damage, knockback, player.whoAmI);

			SoundEngine.PlaySound(SoundID.Item36, player.position);
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "VacuumBuilder");
			recipe.AddIngredient(null, "PrimePlating", 4); // 80 Iron
			recipe.AddIngredient(null, "PrimeEnhancementModule"); // 70 Iron
			recipe.AddIngredient(ItemID.HallowedBar, 20);
			recipe.AddIngredient(ItemID.ShroomiteBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 24);
			recipe.AddIngredient(ItemID.FragmentVortex, 12);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}