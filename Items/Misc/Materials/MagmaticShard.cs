using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class MagmaticShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 42;
			Item.rare = 2;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 1);
			Item.buyPrice(gold: 4);
		}
	}
}