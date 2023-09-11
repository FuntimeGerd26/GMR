using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class NeonBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflits 'Hellfire' to enemies\n'1000 degrees knife'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NeonBlade>(8);
			Item.useTime /= 2;
			Item.SetWeaponValues(25, 3f, 4);
			Item.width = 68;
			Item.height = 68;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 2;
			Item.autoReuse = true;
			Item.reuseDelay = 3;
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
			recipe.AddIngredient(ItemID.GoldBar, 18);
			recipe.AddIngredient(ItemID.CrimstoneBlock, 28);
			recipe.AddIngredient(ItemID.Ruby, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GoldBar, 18);
			recipe2.AddIngredient(ItemID.EbonstoneBlock, 28);
			recipe2.AddIngredient(ItemID.Ruby, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 40);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PlatinumBar, 18);
			recipe3.AddIngredient(ItemID.CrimstoneBlock, 28);
			recipe3.AddIngredient(ItemID.Ruby, 4);
			recipe3.AddIngredient(null, "UpgradeCrystal", 40);
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.PlatinumBar, 18);
			recipe4.AddIngredient(ItemID.EbonstoneBlock, 28);
			recipe4.AddIngredient(ItemID.Ruby, 4);
			recipe4.AddIngredient(null, "UpgradeCrystal", 40);
			recipe4.AddTile(TileID.Anvils);
			recipe4.Register();
		}
	}
}