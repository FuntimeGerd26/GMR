using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR.Items.Accessories
{
	public class BloodyMedkit : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Medkit");
			Tooltip.SetDefault("Heals you by 10% of HP when you are below 20% HP\nYou take 25% less damage while in cooldown");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 140);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife < player.statLifeMax2 / 5 && !player.HasBuff(ModContent.BuffType<Buffs.Debuffs.PainfullyHealed>()) && !Main.dedServ)
			{
				player.statLife += player.statLifeMax2 / 10;
				player.AddBuff(ModContent.BuffType<Buffs.Debuffs.PainfullyHealed>(), 2400);
			}
		}
	}
}