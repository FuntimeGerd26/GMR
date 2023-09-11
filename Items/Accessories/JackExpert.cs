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
	[AutoloadEquip(EquipType.HandsOn)]
	public class JackExpert : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Emblem");
			Tooltip.SetDefault($"Increases ranged damage by 6%\nIncreases movement speed by 5%\nWhen hit, the player will create an explotion\n'It contains a small amount of information'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 52;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.accessory = true;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Ranged) += 0.06f;
			player.moveSpeed += 0.05f;
			player.GPlayer().JackExpert = Item;
		}
	}
}