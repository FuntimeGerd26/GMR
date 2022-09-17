using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class SandwavePants : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandwave Pants");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 30);
			Item.vanity = true;
			Item.maxStack = 1;
		}
	}
}
