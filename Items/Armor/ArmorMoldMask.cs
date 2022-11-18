using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ArmorMoldMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mask Mold");
			Tooltip.SetDefault("Useful for making Masks");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 40);
			Item.maxStack = 1;
			Item.defense = 3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 5);
			recipe2.AddIngredient(null, "UpgradeCrystal", 30);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
