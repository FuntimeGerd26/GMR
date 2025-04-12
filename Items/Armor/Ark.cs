using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ArkHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Headgear"); //絶滅, Extinction
			Tooltip.SetDefault("'絶滅'\nIncreases all weapon damage, crit chance, attack speed and armor penetration by 15% if the player is below 50% health\nIncreases damage and attack speed by 5%\nIncreases max minion slots by 2, increases max mana by 60");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.rare = 9;
			Item.value = Item.sellPrice(silver: 220);
			Item.maxStack = 1;
			Item.defense = 22;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.maxMinions += 2;
			player.statManaMax2 += 60;
			if (player.statLife < player.statLifeMax)
			{
				player.GetDamage(DamageClass.Generic) += 0.15f;
				player.GetCritChance(DamageClass.Generic) += 15f;
				player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
			}
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ArkChestplate>() && legs.type == ModContent.ItemType<ArkBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 2\nIncreases Melee, Ranged and Magic attack speed by 25% and decreases Summon attack speed by 25%\nIncreases damage reduction by 2% and increases life regeneration slightly" +
				"\nWhen under 25% of health: Increase all buffs gained previously except additional minion slots by 20%, Health regeneration is massively boosted based on Max HP";
			player.maxMinions += 2;

			if (player.statLife < (int)(player.statLifeMax * 0.25f))
			{
				player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuffBoost>(), 2);
			}
			else
				player.AddBuff(ModContent.BuffType<Buffs.Buff.ArkBuff>(), 2);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodHelmet");
			recipe.AddIngredient(null, "MagnumFoxMask");
			recipe.AddIngredient(3467, 12); // Luminite Bar
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body, EquipType.Back)]
	public class ArkChestplate : ModItem
	{
		public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Back}", EquipType.Back, this);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Body"); //憎悪, Wrath
			Tooltip.SetDefault("'憎悪'\nIncreases all weapon damage and armor penetration by 10%, increases attack speed by 15%\nIncreases knockback of weapons\nIncreases max minion slots by 2, increases damage reduction by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


			int capeSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
			ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = capeSlot;
			ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = capeSlot;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = 9;
			Item.value = Item.sellPrice(silver: 260);
			Item.maxStack = 1;
			Item.defense = 22;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetKnockback(DamageClass.Generic) += 1f;
			player.GetArmorPenetration(DamageClass.Generic) += 10f;
			player.endurance += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
			player.GetDamage(DamageClass.Generic) += 0.10f;
			player.maxMinions += 2;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodChestplate");
			recipe.AddIngredient(null, "MagnumFoxChestplate");
			recipe.AddIngredient(3467, 16); // Luminite Bar
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class ArkBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Greaves"); //闘争, Strife
			Tooltip.SetDefault("'闘争'\nIncreases all weapon damage and armor penetration by 10%\nIncreases knockback of weapons\nIncreases damage reduction by 5%\nIncreases movement speed by 30%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 9;
			Item.value = Item.sellPrice(silver: 190);
			Item.maxStack = 1;
			Item.defense = 22;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.30f;
			player.GetKnockback(DamageClass.Generic) += 1f;
			player.GetArmorPenetration(DamageClass.Generic) += 10f;
			player.GetDamage(DamageClass.Generic) += 0.10f;
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodLeggings");
			recipe.AddIngredient(null, "MagnumFoxBoots");
			recipe.AddIngredient(3467, 12); // Luminite Bar
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
