using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class AncientInfraRedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.rare = 3;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 2);
			Item.buyPrice(gold: 6);
		}
	}
}