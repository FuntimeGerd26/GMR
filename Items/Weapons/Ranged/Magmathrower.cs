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
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 88;
			Item.height = 34;
			Item.rare = 3;
			Item.useTime = 8;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 12;
			Item.crit = -3;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Flames;
			Item.shootSpeed = 5f;
			Item.useAmmo = AmmoID.Gel;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, -4);
		}
	}
}