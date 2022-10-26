using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class CrystalNeonChip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Currently useless shop, Sell anything you don't want");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.rare = -1;
			Item.value = Item.sellPrice(silver: 0);
		}
	}
}