using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.UI
{
	public class ItemEffectBoosted : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"[i:{ModContent.ItemType<UI.ItemEffectBoosted>()}] This is basically used for special effects that i don't think [i:{ModContent.ItemType<UI.ItemEffectIcon>()}] fits for");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0; //No reason to be here
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.rare = -1;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			Player player = Main.player[0];
			GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
			if (modPlayer.MagnumSet)
			{
				return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			else if (modPlayer.BoostSet != null)
            {
				return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			else
            {
				return Color.White;
			}
		}
	}
}