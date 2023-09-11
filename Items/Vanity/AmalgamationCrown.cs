using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class AmalgamationCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amalgamation's Crown");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdHead");
			recipe.AddIngredient(null, "IceyMask");
			recipe.AddIngredient(null, "ChaosAngelHalo");
			recipe.AddIngredient(null, "JackyMask");
			recipe.AddTile(TileID.Loom);
			recipe.Register();
		}
	}
}
