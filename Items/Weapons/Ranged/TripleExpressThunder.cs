using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class TripleExpressThunder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hitting an enemy will make the projectile split into 4 and deal double the damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 28;
			Item.rare = 4;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 40;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.GungeonBullet2>();
			Item.shootSpeed = 45f;
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
				float numberProjectiles = 2;
				float rotation = MathHelper.ToRadians(20);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .5f;
					Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.GungeonBullet2>(), damage, knockback, player.whoAmI);
					type = ModContent.ProjectileType<Projectiles.Ranged.GungeonBullet2>();
				}
			}
		}
	}
}