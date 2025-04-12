using Terraria;
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
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 180);
			Item.maxStack = 1;
			Item.defense = 15;
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
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AlloybloodChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Torso"); //殺意, Bloodthirst
			Tooltip.SetDefault("'殺意'\nIncreases damage by 12% and movement speed by 5%\nIncreases weapon speed by 7%\nIncreases max minions by 2\nDecreases mana cost by 18%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 20;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 140);
			Item.maxStack = 1;
			Item.defense = 26;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.07f;
			player.maxMinions += 2;
			player.manaCost -= 0.18f;
			player.GetDamage(DamageClass.Generic) += 0.12f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 24);
			recipe.AddIngredient(ItemID.Ectoplasm, 16);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AlloybloodLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Greaves"); //破滅, Ruin
			Tooltip.SetDefault("'破滅'\nIncreases movement speed by 25%\nIncreases knockback of weapons\nIncreases armor penetration and attack speed of melee and summoner weapons by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 120);
			Item.maxStack = 1;
			Item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.25f;
			player.GetKnockback(DamageClass.Generic) += 1f;
			player.GetArmorPenetration(DamageClass.Melee) += 5f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetArmorPenetration(DamageClass.Summon) += 5f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.Ectoplasm, 12);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
