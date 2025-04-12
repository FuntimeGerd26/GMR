using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagmaticVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee and summon damage by 2%\nIncreases melee crit chance and summon attack speed by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = 3;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.02f;
			player.GetDamage(DamageClass.Summon) += 0.02f;
			player.GetCritChance(DamageClass.Melee) += 3f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagmaticChestplate>() && legs.type == ModContent.ItemType<MagmaticBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases armor penetration by 5\nMakes you explode into a rain of fireballs when hit";
			player.GetArmorPenetration(DamageClass.Generic) += 5f;
			player.GPlayer().MagmaSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemHelmet");
			recipe.AddIngredient(null, "MagmaticShard", 9);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MagmaticChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 7%\nIncreases summon damage and armor penetration by 5%\nIncreases max minions and sentries by 1");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = 3;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.07f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetArmorPenetration(DamageClass.Summon) += 5f;
			player.maxMinions += 1;
			player.maxTurrets += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemChestplate");
			recipe.AddIngredient(null, "MagmaticShard", 11);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MagmaticBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee and summoner attack speed by 5%\nIncreases movement speed by 15%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 85);
			Item.rare = 3;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
			player.moveSpeed += 0.15f;
			player.runAcceleration += 0.02f;
			player.maxRunSpeed += 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemBoots");
			recipe.AddIngredient(null, "MagmaticShard", 10);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
