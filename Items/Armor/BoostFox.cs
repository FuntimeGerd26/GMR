using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class BoostFoxMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Mask");
			Tooltip.SetDefault("Increases melee damage by 10%\nIncreases melee speed and magic damage by 6%\nIncreases mana regen and reduces mana cost by 5%\nIncreases movement speed by 15%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 95);
			Item.rare = 4;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.1f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.06f;
			player.GetDamage(DamageClass.Magic) += 0.06f;
			player.manaRegen += 5;
			player.manaCost -= 0.05f;
			player.moveSpeed += 0.15f;
			player.runAcceleration += 0.15f;
			player.maxRunSpeed += 0.15f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<BoostFoxChestplate>() && legs.type == ModContent.ItemType<BoostFoxBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 8%\nIncreases max mana by 10%\n Melee weapons that shoot projectiles now shoot an aditional fire ball";
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.statManaMax2 += player.statManaMax / 10;
			player.GPlayer().BoostSet = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrichalcumBar, 20);
			recipe.AddIngredient(ItemID.Goggles);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(ItemID.Amber, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.MythrilBar, 20);
			recipe2.AddIngredient(ItemID.Goggles);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddIngredient(ItemID.Amber, 2);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class BoostFoxChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Chestplate");
			Tooltip.SetDefault($"Increases melee, magic and summon damage and attack speed by 7%\nDecreases ranged damage by 10%\nIncreases melee knockback by 20%\nIncreases movement speed by 10%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
			player.GetAttackSpeed(DamageClass.Magic) += 0.07f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.07f;
			player.GetDamage(DamageClass.Melee) += 0.07f;
			player.GetDamage(DamageClass.Magic) += 0.07f;
			player.GetDamage(DamageClass.Summon) += 0.07f;
			player.GetDamage(DamageClass.Ranged) += -0.1f;
			player.GetKnockback(DamageClass.Melee) += 0.20f;
			player.moveSpeed += 0.1f;
			player.runAcceleration += 0.1f;
			player.maxRunSpeed += 0.1f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrichalcumBar, 28);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddIngredient(ItemID.Amber);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.MythrilBar, 28);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe2.AddIngredient(ItemID.Amber);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class BoostFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Boots");
			Tooltip.SetDefault($"Increases movement speed by 25%\nIncreases all weapon attack speed by 15%\nIncreases wing time by 10%\n Allows walking on water and lava");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 4;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
				player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
				player.moveSpeed += 0.25f;
			    player.wingTimeMax += player.wingTimeMax / 10;
				player.waterWalk = true;
				player.fireWalk = true;
				player.runAcceleration += 0.25f;
				player.maxRunSpeed += 0.25f;
		}

		public override void AddRecipes()
		{
				Recipe recipe = CreateRecipe();
				recipe.AddIngredient(ItemID.OrichalcumBar, 20);
				recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
				recipe.AddIngredient(ItemID.Amber, 1);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.Register();

				Recipe recipe2 = CreateRecipe();
				recipe2.AddIngredient(ItemID.MythrilBar, 20);
				recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
				recipe2.AddIngredient(ItemID.Amber, 1);
				recipe2.AddTile(TileID.MythrilAnvil);
				recipe2.Register();
		}
	}
}
