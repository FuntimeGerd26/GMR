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
	public class JackExpert : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Emblem");
			Tooltip.SetDefault("Increases ranged damage by 5%\nConverts wooden arrows turn into evenly spread 3 Jack Shards, Fire arrows decrease the spread");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 52;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Ranged) += 0.5f;
			player.GPlayer().JackExpert = Item;
		}
	}
}