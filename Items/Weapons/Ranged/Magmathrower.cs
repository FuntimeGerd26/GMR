using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Ranged
{
	public class Magmathrower : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts 'On Fire!' to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 88;
			Item.height = 34;
			Item.rare = 3;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 15;
			Item.crit = 1;
			Item.knockBack = 0.1f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.MagmaFire>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Gel;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, -4);
		}
	}
}