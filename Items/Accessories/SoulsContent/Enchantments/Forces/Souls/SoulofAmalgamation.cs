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

namespace GMR.Items.Accessories.SoulsContent.Enchantments.Forces.Souls
{
	public class SoulofAmalgamation : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 96;
			Item.height = 74;
			Item.value = Item.sellPrice(silver: 285);
			Item.rare = 9;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.15f;
			player.GetDamage(DamageClass.Magic) += 0.15f;
			player.GetAttackSpeed(DamageClass.Ranged) -= 0.12f;
			player.GetCritChance(DamageClass.Melee) += 5f;
			player.GetCritChance(DamageClass.Magic) += 5f;
			player.statDefense /= 2;
			if (player.statMana < player.statManaMax2 / 2)
				player.manaRegen += 16;
			else
				player.manaRegen += 4;
		}
	}
}