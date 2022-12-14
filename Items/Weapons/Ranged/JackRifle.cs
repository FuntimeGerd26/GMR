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
	public class JackRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Rifle");
			Tooltip.SetDefault("'Your enemies wish they don't get hit by this'\nInflicts 'Partially Crystalized' on enemies, deals massive damage over time");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 32;
			Item.rare = 7;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/JackBullet")
			{
				Volume = 0.75f,
				PitchVariance = 0.2f,
				MaxInstances = 2,
			};
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 100;
			Item.knockBack = 5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
			Item.expert = true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -14);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
				velocity = velocity.RotatedByRandom(MathHelper.ToRadians(7));
			}
		}
	}
}