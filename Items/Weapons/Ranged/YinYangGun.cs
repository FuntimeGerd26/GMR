using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class YinYangGuns : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Black and White into a balance'\nUpon hitting an enemy projectiles will split into 3 and then bounce from enemies and tiles");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 28;
			Item.rare = 6;
			Item.useTime = 16;
			Item.useAnimation = 23;
			Item.reuseDelay = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 25;
			Item.crit = 4;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.GungeonBulletBalance>();
			Item.shootSpeed = 25f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(14);
				position += Vector2.Normalize(velocity);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)) * .5f);
					Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.GungeonBulletBalance>(), damage, knockback, player.whoAmI);
					type = 0;
					SoundEngine.PlaySound(SoundID.Item41, player.position);
				}
			}
		}
	}
}