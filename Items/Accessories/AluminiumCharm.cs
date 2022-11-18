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
	public class AluminiumCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Charm");
			Tooltip.SetDefault("Ranged weapons will shoot a projectile that will home into enemies\nIncreases ranged crit chance by 3%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 38;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().AluminiumCharm = Item;
			player.GetCritChance(DamageClass.Ranged) += 3f;
		}
	}
}