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
			Item.width = 20;
			Item.height = 28;
			Item.rare = 3;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(gold: 2);
		}
	}
}