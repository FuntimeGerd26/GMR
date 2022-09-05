using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class ThunderbladeCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunderblade Charm");
			Tooltip.SetDefault("Increases attack speed by 5%\nMakes lightnings fall from the sky when taking damage hit you");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 62;
			Item.value = Item.sellPrice(silver: 125);
			Item.rare = 3;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.5f;
			player.GPlayer().Thunderblade = Item;
		}
	}
}