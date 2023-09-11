using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Tiles
{
	public class MagmaAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magmatic Altar");
			Tooltip.SetDefault("Allows for summoning Magma Eye freely");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.width = 28;
			Item.height = 30;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Items.Tiles.MagmaticAltar>();
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 95);
		}
	}
}
