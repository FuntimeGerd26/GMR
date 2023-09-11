using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class KizunaScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Will chase after enemies if they're close enough");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 82;
			Item.rare = 5;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 130);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 45;
			Item.crit = 1;
			Item.knockBack = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.KizunaScythe>();
			Item.shootSpeed = 12f;
		}
	}
}