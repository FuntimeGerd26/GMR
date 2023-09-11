using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class GerdBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amalgamate's Chestplate");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 20;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}
	}
}
