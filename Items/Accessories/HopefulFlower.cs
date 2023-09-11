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
	public class HopefulFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases magic and ranged damage by 15%\nMultiplies attack speed by x1.05\nDecreases crit chance by 15%\nHealth regen is slower by 20%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 58;
			Item.value = Item.sellPrice(silver: 280);
			Item.rare = 5;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.11f;
			player.GetDamage(DamageClass.Ranged) += 0.11f;
			player.GetAttackSpeed(DamageClass.Generic) *= 1.05f;
			player.GetCritChance(DamageClass.Generic) += -11f;
			if (player.lifeRegen >= 1)
				player.lifeRegen = player.lifeRegen - (int)(player.lifeRegen * 0.2f);
		}
	}
}