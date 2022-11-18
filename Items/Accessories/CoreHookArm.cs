using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;
namespace GMR.Items.Accessories
{
	public class CoreHookArm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Hook Arm");
			Tooltip.SetDefault("Press the corresponding hotkey to shoot a quick moving hook");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 54;
			Item.width = 32;
			Item.height = 86;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.value = Item.sellPrice(silver: 255);
			Item.rare = 6;
			Item.shoot = ModContent.ProjectileType<Projectiles.Tools.CoreHook>();
			Item.shootSpeed = 12f;
			Item.accessory = true;
		}

		public override bool? UseItem(Player player)
		{
			Item.shoot = ModContent.ProjectileType<Projectiles.Tools.CoreHook>();
			return true;
		}
	}
}