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
			Tooltip.SetDefault("'A strange box filled with unseen technology'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 100);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 20);
			recipe.AddIngredient(ItemID.TungstenBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 20);
			recipe2.AddIngredient(ItemID.TungstenBar, 20);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.IronBar, 20);
			recipe3.AddIngredient(ItemID.SilverBar, 20);
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.LeadBar, 20);
			recipe4.AddIngredient(ItemID.SilverBar, 20);
			recipe4.AddTile(TileID.Anvils);
			recipe4.Register();
		}
	}
}