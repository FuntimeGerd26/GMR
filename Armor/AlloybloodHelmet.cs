﻿using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AlloybloodHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Visor"); //絶望, Despair
			Tooltip.SetDefault("'絶望'\nIncreases damage by 7% and crit chance by 2%\nIncreases weapon speed by 18%\nIncreases max minions by 1\nIncreases max mana by 40");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.rare = 8;
			Item.value = Item.sellPrice(silver: 180);
			Item.maxStack = 1;
			Item.defense = 18;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.18f;
			player.maxMinions += 1;
			player.statManaMax2 += 40;
			player.GetDamage(DamageClass.Generic) += 0.07f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AlloybloodChestplate>() && legs.type == ModContent.ItemType<AlloybloodLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases all weapon speed and damage by 6%\nWhen under 50% of health: increased armor penetration, crit chance and damage by 7%";
			player.GetAttackSpeed(DamageClass.Generic) += 0.06f;
			player.GetDamage(DamageClass.Generic) += 0.06f;
			player.maxMinions++;

			if (player.statLife < player.statLifeMax / 2)
			{
				player.GetDamage(DamageClass.Generic) += 0.07f;
				player.GetCritChance(DamageClass.Generic) += 7f;
				player.GetArmorPenetration(DamageClass.Generic) += 7f;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.Ectoplasm, 8);
			recipe.AddIngredient(ItemID.TitaniumHelmet);
			recipe.AddIngredient(ItemID.TitaniumMask);
			recipe.AddIngredient(ItemID.TitaniumHeadgear);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.HallowedBar, 12);
			recipe2.AddIngredient(ItemID.Ectoplasm, 8);
			recipe2.AddIngredient(ItemID.AdamantiteHelmet);
			recipe2.AddIngredient(ItemID.AdamantiteMask);
			recipe2.AddIngredient(ItemID.AdamantiteHeadgear);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
