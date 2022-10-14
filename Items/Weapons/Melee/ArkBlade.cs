using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class ArkBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Spins around the player");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 68;
			Item.rare = 7;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 95;
			Item.crit = 14;
			Item.knockBack = 7f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.ArkBladeSpin>();
			Item.shootSpeed = 0f;
		}
	}
}