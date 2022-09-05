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
	public class NeonGunblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hitting enemies with the projectile will create 2 more going to the sides of the projectile's direction");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 116;
			Item.height = 44;
			Item.rare = 6;
			Item.useTime = 7;
			Item.useAnimation = 14;
			Item.reuseDelay = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item15;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 60;
			Item.crit = 14;
			Item.knockBack = 3f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.NeonGunSlash>();
			Item.shootSpeed = 40f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-26, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.NeonGunSlash>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 14);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.SilverBar, 14);
			recipe2.AddIngredient(ItemID.SoulofLight, 18);
			recipe2.AddIngredient(ItemID.PlatinumBar, 8);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.SilverBar, 14);
			recipe3.AddIngredient(ItemID.SoulofLight, 18);
			recipe3.AddIngredient(ItemID.GoldBar, 8);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.CobaltBar, 14);
			recipe4.AddIngredient(ItemID.SoulofLight, 18);
			recipe4.AddIngredient(ItemID.PlatinumBar, 8);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}