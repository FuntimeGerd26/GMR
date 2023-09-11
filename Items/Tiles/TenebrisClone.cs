using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Tiles
{
	public class TenebrisClone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tenebris");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.width = 16;
			Item.height = 14;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Items.Tiles.TenebrisCloneTile>();
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(silver: 0);
		}
	}
}
