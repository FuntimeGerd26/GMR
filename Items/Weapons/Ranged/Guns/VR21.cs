using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class VR21 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 68;
			Item.height = 20;
			Item.rare = 8;
			Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 410);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36.WithPitchOffset(0.8f);
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 51;
			Item.crit = -2;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;

			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.ShotgunBullet>();
				damage = (int)(damage * 1.2f);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(2f));

				newVelocity *= 1f - Main.rand.NextFloat(0f, 0.25f);

				int p = Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
				Main.projectile[p].penetrate += 1;
			}
			player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.2f);

			return true;
		}
	}
}