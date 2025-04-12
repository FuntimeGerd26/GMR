using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class InfraRedCrystalShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.rare = 4;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 3);
			Item.buyPrice(gold: 9);
		}
	}
}