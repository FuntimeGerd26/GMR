using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class GutterVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 150);
			Item.rare = 4;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
			player.GetCritChance(DamageClass.Generic) -= 2f;
			player.ClearBuff(BuffID.Darkness);
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<GutterChestplate>() && legs.type == ModContent.ItemType<GutterLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases damage by 20%\nIncreases crit chance by 10%\nIncreases defense by an additional 5";
			player.GetDamage(DamageClass.Generic) += 0.20f;
			player.GetCritChance(DamageClass.Generic) += 10f;
			player.statDefense += 5;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SilverBar, 18);
			recipe.AddIngredient(ItemID.SpiderFang, 7);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TungstenBar, 18);
			recipe2.AddIngredient(ItemID.SpiderFang, 7);
			recipe2.AddIngredient(ItemID.SoulofNight, 6);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class GutterChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 170);
			Item.maxStack = 1;
			Item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.12f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) -= 2f;
			player.extraFall += 10;
			player.maxFallSpeed *= 1.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SilverBar, 22);
			recipe.AddIngredient(ItemID.SpiderFang, 8);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TungstenBar, 22);
			recipe2.AddIngredient(ItemID.SpiderFang, 8);
			recipe2.AddIngredient(ItemID.SoulofNight, 6);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class GutterLeggings : ModItem
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
			Item.value = Item.sellPrice(silver: 160);
			Item.maxStack = 1;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed -= 0.1f;
			player.jumpSpeedBoost *= 1.2f;
			player.ClearBuff(BuffID.Slow);
			player.GetAttackSpeed(DamageClass.Generic) += 0.08f;
			player.GetCritChance(DamageClass.Generic) -= 8f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SilverBar, 18);
			recipe.AddIngredient(ItemID.SpiderFang, 4);
			recipe.AddIngredient(ItemID.SoulofNight, 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TungstenBar, 18);
			recipe2.AddIngredient(ItemID.SpiderFang, 4);
			recipe2.AddIngredient(ItemID.SoulofNight, 6);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}