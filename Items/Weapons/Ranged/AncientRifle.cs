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
	public class AncientRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Replaces normal bullets with Infra-Red Bullets that inflict 'Partially Crystalized' on enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 108;
			Item.height = 40;
			Item.rare = 3;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 95);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot2");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.crit = 6;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-38, -8);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 7;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
			}
		}
	}
}