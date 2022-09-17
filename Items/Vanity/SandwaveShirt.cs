using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Body)]
	public class SandwaveShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandwave Shirt");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 30);
			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
