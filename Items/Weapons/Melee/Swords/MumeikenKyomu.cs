using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class MumeikenKyomu : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflits 'Hellfire' to enemies\nShoots homing Phoenixes\n'Kyomu, the pitch black blade returns all to nothing'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.MumeikenKyomu>(35);
			Item.useTime /= 4;
			Item.SetWeaponValues(25, 5f, 1);
			Item.width = 74;
			Item.height = 78;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 180);
			Item.rare = 3;
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

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "PhoenixSword");
			recipe.AddIngredient(null, "MagmaticShard", 20);
			recipe.AddIngredient(ItemID.AshBlock, 45);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}