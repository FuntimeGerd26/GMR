using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AlloybloodSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hitting enemies increases size of the projectile");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 68;
			Item.rare = 5;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 50;
			Item.crit = 50;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AlloybloodSwing>();
			Item.shootSpeed = 1f;
		}
	}
}