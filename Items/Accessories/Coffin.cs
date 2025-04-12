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
	[AutoloadEquip(EquipType.Back)]
	public class Coffin : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 16;
			Item.value = Item.sellPrice(silver: 200);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.mount.Active)
				player.AddBuff(ModContent.BuffType<Buffs.Buff.WildHunt>(), 2);
			else
				player.AddBuff(ModContent.BuffType<Buffs.Buff.Coffin>(), 2);
		}
    }
}