using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class SpazThighs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatanium's Thighs");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(silver: 30);
			Item.vanity = true;
		}
	}
}
