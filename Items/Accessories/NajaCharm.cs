using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class NajaCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volcano Charm");
			Tooltip.SetDefault($"Using any weapon that's not ranged will shoot a fireball that explodes dealing damage on a large area");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 145);
			Item.rare = 3;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().NajaCharm = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Obsidian, 30);
			recipe.AddIngredient(ItemID.LavaBucket);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}