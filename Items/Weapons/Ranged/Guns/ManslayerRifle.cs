using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class ManslayerRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 138;
			Item.height = 34;
			Item.rare = 5;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 265);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.crit = 21;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-30, -6);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = position.Y - 8;
			for (int i = 0; i < 2; i++)
			{
				Vector2 newVelocity = velocity * (0.5f * (i + 1));

				int p = Projectile.NewProjectile(source, position, newVelocity, type, (int)(damage / (i + 2)), knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item11.WithPitchOffset(0.25f), player.position);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ManslayerShotgun");
			recipe.AddIngredient(ItemID.MythrilBar, 12);
			recipe.AddIngredient(ItemID.PixieDust, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ManslayerShotgun");
			recipe2.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe2.AddIngredient(ItemID.PixieDust, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}