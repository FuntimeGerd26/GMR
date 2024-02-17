using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Materials
{
	public class InfraRedBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.rare = 4;
			Item.maxStack = 999;
			Item.value = Item.buyPrice(silver: 450);
			Item.buyPrice(gold: 15);
		}
	}
}