using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class HardmodeUpgradeCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crytal Claw");
			Tooltip.SetDefault("'A crystal powerful enough to make hardmode items'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 22;
			Item.rare = 4;
			Item.maxStack = 99999;
			Item.value = Item.sellPrice(silver: 60);
			Item.buyPrice(gold: 30);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "BossUpgradeCrystal", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "UpgradeCrystal", 400);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe(20);
			recipe3.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();
		}
	}
}