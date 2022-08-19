using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class JackCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Better keep this just in case'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 34;
			Item.scale = 1f;
			Item.rare = 5;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.reuseDelay = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 150);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item61;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 300;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.JackBlastSmall>();
			Item.shootSpeed = 30f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.JackBlastSmall>();
			}
		}
	}
}