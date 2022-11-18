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
			Tooltip.SetDefault("'A crystal powerful enough to make even more items'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 22;
			Item.rare = 5;
			Item.maxStack = 99999;
			Item.value = Item.sellPrice(silver: 60);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "BossUpgradeCrystal", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal", 40);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}