using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AmethystGolemHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Visor");
			Tooltip.SetDefault("Increases melee and minion damage by 5% and ranged & magic speed by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 30);
			Item.rare = 1;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Magic) += 0.03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AmethystGolemChestplate>() && legs.type == ModContent.ItemType<AmethystGolemBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $" When damaged you release crystal shards\nIncreases damage reduction by 1%\nIncreases max minions by 1\nIncreases melee speed by 5%\nIncreases all damage by 8%\nYou take 20% less damage";
			player.endurance += 0.01f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.maxMinions++;
			player.GPlayer().AmethystSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 8);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AmethystGolemChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Chestplate");
			Tooltip.SetDefault("Increases all damage by 4%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 40);
			Item.rare = 1;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.04f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 10);
			recipe.AddIngredient(ItemID.Amethyst, 6);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AmethystGolemBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Boots");
			Tooltip.SetDefault("Increases ranged speed by 3%, and melee speed by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.03f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 7);
			recipe.AddIngredient(ItemID.Amethyst, 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
