using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class AlloyBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A strange box filled with technology that you don't even remember making'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = 1;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver: 100);
			Item.buyPrice(gold: 4);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 15);
			recipe.AddIngredient(ItemID.TungstenBar, 10);
			recipe.AddIngredient(null, "UpgradeCrystal", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 15);
			recipe2.AddIngredient(ItemID.SilverBar, 10);
			recipe2.AddIngredient(null, "UpgradeCrystal", 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}