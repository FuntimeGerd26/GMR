using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class MagnumShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Ready, Fight'\nShoots in bursts of 3");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
		}

		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 36;
			Item.rare = 2;
			Item.useTime = 4;
			Item.useAnimation = 12;
			Item.reuseDelay = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 245);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 13;
			Item.crit = 6;
			Item.knockBack = 5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.MagnumBlast>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.MagnumBlast>();
				SoundEngine.PlaySound(SoundID.Item41, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TungstenBar, 25);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddIngredient(ItemID.Ruby, 3);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.SilverBar, 25);
			recipe2.AddIngredient(ItemID.IllegalGunParts);
			recipe2.AddIngredient(ItemID.Ruby, 3);
			recipe2.AddIngredient(ItemID.Silk, 8);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "MagnumRifle");
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();
		}
	}
}