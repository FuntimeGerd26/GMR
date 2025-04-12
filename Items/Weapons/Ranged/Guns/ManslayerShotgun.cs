using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class ManslayerShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 74;
			Item.height = 22;
			Item.rare = 3;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 165);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.crit = 21;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -4);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = position.Y - 4;
			for (int i = 0; i < 4; i++)
			{
				// Rotate the velocity randomly between 5 degrees.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(3f));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.5f);

				int p = Projectile.NewProjectile(source, position, newVelocity, type, (int)(damage / (i + 1)), knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item36.WithPitchOffset(0.25f), player.position);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBar, 20);
			recipe.AddIngredient(ItemID.Bone, 60);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PlatinumBar, 20);
			recipe2.AddIngredient(ItemID.Bone, 60);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}