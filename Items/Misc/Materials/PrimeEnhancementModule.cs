using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class PrimeEnhancementModule : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.rare = 7;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 15);
			Item.buyPrice(gold: 30);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(ItemID.TitaniumBar, 12);
			recipe.AddIngredient(ItemID.Nanites, 30);
			recipe.AddRecipeGroup("IronBar", 30);
			recipe.AddIngredient(null, "PrimePlating", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(2);
			recipe2.AddIngredient(ItemID.AdamantiteBar, 12);
			recipe2.AddIngredient(ItemID.Nanites, 30);
			recipe2.AddRecipeGroup("IronBar", 30);
			recipe2.AddIngredient(null, "PrimePlating", 2);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}