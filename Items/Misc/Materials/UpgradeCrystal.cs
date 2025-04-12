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
			DisplayName.SetDefault("Crystal Screw");
			Tooltip.SetDefault("'A crystal powerful enough to make common items'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 22;
			Item.rare = 0;
			Item.maxStack = 99999;
			Item.value = Item.sellPrice(silver: 15);
			Item.buyPrice(silver: 150);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(20);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe(350);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}