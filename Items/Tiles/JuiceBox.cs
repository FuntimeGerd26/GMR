using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Tiles
{
	public class JuiceBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oil Box");
			Tooltip.SetDefault("'Smells so nice'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.width = 18;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Items.Tiles.JuiceBoxTile>();
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 5);
		}
	}
}
