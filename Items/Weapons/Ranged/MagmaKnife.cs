using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class MagmaKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Causes an explosion when hitting enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 46;
			Item.rare = 3;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 135);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 32;
			Item.crit = 10;
			Item.knockBack = 1.5f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.MagmaKnife>();
			Item.shootSpeed = 30f;
		}
	}
}