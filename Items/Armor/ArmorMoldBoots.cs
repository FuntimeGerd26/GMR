using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ArmorMoldBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legging Mold");
			Tooltip.SetDefault("Useful for creating Leggings");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 4;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 40);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
