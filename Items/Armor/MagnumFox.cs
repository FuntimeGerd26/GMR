using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagnumFoxMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Mask");
			Tooltip.SetDefault("Increases all damage by 10%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 85);
			Item.rare = 2;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.10f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagnumFoxChestplate>() && legs.type == ModContent.ItemType<MagnumFoxBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases ranged projectile velocity\nIncreases base ranged damage by 8 and endurance by 2%\nIncreases max health by 5%";
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Ranged).Base += 8f;
			player.statLifeMax2 += player.statLifeMax / 20;
			player.GPlayer().MagnumSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe.AddIngredient(ItemID.Goggles);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.DemoniteBar, 25);
			recipe2.AddIngredient(ItemID.Goggles);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddIngredient(ItemID.ShadowScale, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MagnumFoxChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Chestplate");
			Tooltip.SetDefault("Increases all weapon attack speed by 5%\nIncreases all knockback slightly");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = 2;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.GetKnockback(DamageClass.Generic) += 0.5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 30);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddIngredient(ItemID.TissueSample, 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.DemoniteBar, 30);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddIngredient(ItemID.ShadowScale, 7);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MagnumFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Boots");
			Tooltip.SetDefault("Increases melee attack speed by 4%\nIncreases movement speed by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 2;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.DemoniteBar, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddIngredient(ItemID.ShadowScale, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
