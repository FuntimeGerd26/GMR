using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class PrimePlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 30;
			Item.rare = 4;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 2);
			Item.buyPrice(gold: 20);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(null, "MagmaticShard", 5);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddRecipeGroup("IronBar", 40);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(2);
			recipe2.AddIngredient(null, "MagmaticShard", 5);
			recipe2.AddIngredient(ItemID.PlatinumBar, 10);
			recipe2.AddRecipeGroup("IronBar", 40);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 10);
			recipe2.AddTile(TileID.Hellforge);
			recipe2.Register();
		}
	}
}