using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class UpgradeCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A crystal powerful enough to make some items'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 22;
			Item.rare = 1;
			Item.maxStack = 99999;
			Item.value = Item.sellPrice(silver: 20);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(20);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(40);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}