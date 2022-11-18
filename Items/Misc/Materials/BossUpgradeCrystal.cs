using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class BossUpgradeCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A crystal powerful enough to make more items'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 22;
			Item.rare = 2;
			Item.maxStack = 99999;
			Item.value = Item.sellPrice(silver: 40);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(20);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}