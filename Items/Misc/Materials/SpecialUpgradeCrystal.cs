using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class SpecialUpgradeCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Emblem");
			Tooltip.SetDefault("'A crystal gotten from dificult extraction, extracting it is more of patience than effort'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.rare = 3;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 25);
			Item.buyPrice(gold: 150);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}