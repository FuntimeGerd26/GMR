using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.UI
{
	public class ItemEffectIcon : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"[i:{ModContent.ItemType<UI.ItemEffectIcon>()}] An icon for Alt click effects, for no reason tbh");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0; //No reason to be here
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.rare = -1;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}