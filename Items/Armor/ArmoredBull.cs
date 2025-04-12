using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ArmoredBullHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 4%\nIncreases attack speed and knockback by 3%\nIncreases damage reduction by 1%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 65);
			Item.rare = 2;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.04f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.03f;
			player.GetKnockback(DamageClass.Generic) += 0.03f;
			player.endurance += 0.01f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ArmoredBullChestplate>() && legs.type == ModContent.ItemType<ArmoredBullGreaves>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases melee damage by 8%\nIncreases damage reduction by 2%\nIncreases max health by 30%";
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Melee) += 0.08f;
			player.statLifeMax2 += (int)(player.statLifeMax * 0.3f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 15);
			recipe.AddIngredient(ItemID.RottenChunk, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddIngredient(ItemID.Amber, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 15);
			recipe2.AddIngredient(ItemID.Vertebrae, 8);
			recipe2.AddIngredient(null, "BossUpgradeCrystal");
			recipe2.AddIngredient(ItemID.Amber, 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class ArmoredBullChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee attack speed by 6%\nIncreases damage reduction by 2%\nIncreases aggression slightly");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = 2;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.06f;
			player.endurance += 0.02f;
			player.aggro += 20;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 25);
			recipe.AddIngredient(ItemID.RottenChunk, 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 25);
			recipe2.AddIngredient(ItemID.Vertebrae, 15);
			recipe2.AddIngredient(null, "BossUpgradeCrystal");
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class ArmoredBullGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee attack speed and damage by 2%\nIncreases damage reduction by 1%\nIncreases movement speed by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = 2;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.02f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.02f;
			player.endurance += 0.01f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 20);
			recipe.AddIngredient(ItemID.RottenChunk, 10);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddRecipeGroup("IronBar", 20);
			recipe2.AddIngredient(ItemID.Vertebrae, 10);
			recipe2.AddIngredient(null, "BossUpgradeCrystal");
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}