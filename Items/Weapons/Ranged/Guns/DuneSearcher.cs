using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class DuneSearcher : ModItem
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
			Item.rare = 7;
			Item.useTime = 10;
			Item.useAnimation = 30;
			Item.reuseDelay = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 255);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 35;
			Item.crit = 0;
			Item.knockBack = 6f;
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
			int NumProjectiles = 5; // The humber of projectiles that this gun will shoot.

			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.5f, 0.75f);

				// Create a projectile.
				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, damage, knockback, player.whoAmI);
				Main.projectile[p].extraUpdates += 4;
			}

			SoundEngine.PlaySound(SoundID.Item36, player.position);
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sandstone, 100);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 8);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}