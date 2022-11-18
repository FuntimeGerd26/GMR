using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ArmorMoldChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chestplate Mold");
			Tooltip.SetDefault("Useful for making Chestplate");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 50);
			Item.maxStack = 1;
			Item.defense = 5;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(null, "UpgradeCrystal", 50);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 6);
			recipe2.AddIngredient(null, "UpgradeCrystal", 50);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
