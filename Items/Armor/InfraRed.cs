using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class InfraRedVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 110);
			Item.rare = 4;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedPlating>() && legs.type == ModContent.ItemType<InfraRedGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 4%\n A style of swords can now shoot an extra projectile that goes through walls and deals 50% more damage\nHello World.";
			player.GetDamage(DamageClass.Generic) += 0.04f;
			player.GPlayer().InfraRedSet = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(null, "InfraRedBar", 10);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 25);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 4);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class InfraRedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 215);
			Item.rare = 4;
			Item.defense = 17;
		}

		public override void UpdateEquip(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
			{
				player.GetDamage(DamageClass.Generic) += 0.06f;
				player.GetCritChance(DamageClass.Generic) += 4f;
				player.maxMinions += 2;
				player.maxTurrets += 2;
			}
			else
			{
				Item.defense = 0;
				Item.vanity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddIngredient(null, "InfraRedBar", 18);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 7);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 65);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class InfraRedGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 175);
			Item.defense = 14;
		}

		public override void UpdateEquip(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
			{
				player.moveSpeed += 0.5f;
				player.jumpSpeedBoost += 0.07f;
				player.statManaMax2 += 60;
				player.GetAttackSpeed(DamageClass.Generic) += 0.03f;
			}
			else
			{
				Item.defense = 0;
				Item.vanity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(null, "InfraRedBar", 15);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 25);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 5);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 60);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}


	[AutoloadEquip(EquipType.Body)]
	public class InfraRedGuardChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 215);
			Item.rare = 4;
			Item.defense = 19;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.11f;
			player.moveSpeed -= 0.6f;
			player.maxMinions += 2;
			player.maxTurrets += 3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddIngredient(null, "InfraRedBar", 18);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 7);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 65);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}


	[AutoloadEquip(EquipType.Head)]
	public class InfraRedGuardVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 110);
			Item.rare = 4;
			Item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.03f;
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedGuardChestplate>() && legs.type == ModContent.ItemType<InfraRedGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases damage reduction by 4%\nDecreases movement speed by 4%\n A style of swords can now shoot an extra projectile that goes through walls and deals 50% more damage\n'Hello World'.";
			player.endurance += 0.04f;
			player.moveSpeed -= 0.04f;
			player.GPlayer().InfraRedSet = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(null, "InfraRedBar", 10);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 25);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 4);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}


	[AutoloadEquip(EquipType.Head)]
	public class InfraRedHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.value = Item.sellPrice(silver: 110);
			Item.rare = 4;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.maxTurrets += 1;
			player.manaCost -= 0.10f;
			player.endurance += 0.05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<InfraRedChestplate>() && legs.type == ModContent.ItemType<InfraRedLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 5%\nIncreases melee weapon size by 15%\nWhen taking damage, heal 0.5% of max hp (A minimum of 5 hp is healed by this effect)";
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GPlayer().InfraRedArmor = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 16);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe.AddIngredient(null, "UpgradeCrystal", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class InfraRedChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 215);
			Item.rare = 4;
			Item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
			{
				player.GetDamage(DamageClass.Generic) += 0.08f;
				player.GetCritChance(DamageClass.Generic) += 6f;
			}
			else
			{
				Item.defense = 0;
				Item.vanity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 28);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(null, "UpgradeCrystal", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class InfraRedLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 175);
			Item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.JackyMaskOn>()))
			{
				player.moveSpeed += 0.25f;
				player.jumpSpeedBoost += 0.05f;
				player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			}
			else
			{
				Item.defense = 0;
				Item.vanity = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 25);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 10);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(null, "UpgradeCrystal", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
