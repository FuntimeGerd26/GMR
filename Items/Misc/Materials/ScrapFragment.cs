using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class ScrapFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A bunch of crystals and metal plates, somehow perfectly cut'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 26;
			Item.rare = 3;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 2);
			Item.buyPrice(gold: 6);
		}
	}
}