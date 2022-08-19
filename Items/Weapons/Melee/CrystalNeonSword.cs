using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class CrystalNeonSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Slashy slash'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 84;
			Item.rare = 1;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.CrystalNeonSlash>();
			Item.shootSpeed = 1f;
		}
	}
}