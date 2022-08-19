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
			Tooltip.SetDefault("'Hurts and dosen't at the same time'\nProjectiles can damage players too after hitting an enemy");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 62;
			Item.height = 32;
			Item.scale = 1.1f;
			Item.rare = 5;
			Item.useTime = 12;
			Item.useAnimation = 18;
			Item.reuseDelay = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 100);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/JackBullet")
			{
				Volume = 1.25f,
				PitchVariance = 0.1f,
				MaxInstances = 1,
			};
			Item.DamageType = ModContent.GetInstance<Classless>();
			Item.damage = 200;
			Item.crit = 100;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
			Item.shootSpeed = 40f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-26, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
			}
		}
	}
}