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
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.reuseDelay = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 140);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item15;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 60;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.NeonGunSlash>();
			Item.shootSpeed = 40f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-26, -6);
		}
	}
}