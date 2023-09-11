using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class AmalgamationLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amalgamation's Leggings");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdLegs");
			recipe.AddIngredient(null, "IceyLegs");
			recipe.AddIngredient(null, "ChaosAngelPants");
			recipe.AddIngredient(null, "InfraRedGreaves");
			recipe.AddTile(TileID.Loom);
			recipe.Register();
		}
	}
}
