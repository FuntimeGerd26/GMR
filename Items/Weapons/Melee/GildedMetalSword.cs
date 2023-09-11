using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class GildedMetalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Watch it'\nShoots a piercing cut that causes explosions on hit\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.GildedMetalSword>(20);
			Item.useTime /= 2;
			Item.SetWeaponValues(48, 2f, 0);
			Item.crit = 4;
			Item.width = 54;
			Item.height = 68;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 225);
			Item.rare = 4;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}
	}
}