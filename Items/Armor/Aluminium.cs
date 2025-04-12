using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AluminiumHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 16;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.01f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AluminiumBreastplate>() && legs.type == ModContent.ItemType<AluminiumLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases all weapon speed by 2%\nIncreases all damage by 2%\nAttacks have a chance to shoot a shuriken";
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetDamage(DamageClass.Generic) += 0.02f;
			player.maxMinions++;
			player.GPlayer().AlumArmor = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 8);
			recipe.AddIngredient(ItemID.Glass, 15);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 8);
			recipe2.AddIngredient(ItemID.Glass, 15);
			recipe2.AddIngredient(ItemID.FallenStar, 5);
			recipe2.AddIngredient(null, "UpgradeCrystal", 10);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AluminiumBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Breastplate");
			Tooltip.SetDefault("Increases weapon speed by 1%\nIncreases magic damage by 2%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.01f;
			player.GetDamage(DamageClass.Magic) += 0.2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 14);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ItemID.FallenStar, 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 14);
			recipe2.AddIngredient(ItemID.Glass, 20);
			recipe2.AddIngredient(ItemID.FallenStar, 8);
			recipe2.AddIngredient(null, "UpgradeCrystal", 25);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AluminiumLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Boots");
			Tooltip.SetDefault("Increases all weapon speed by 2%\nIncreases damage by 2%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.02f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 10);
			recipe.AddIngredient(ItemID.Glass, 18);
			recipe.AddIngredient(ItemID.FallenStar, 6);
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 10);
			recipe2.AddIngredient(ItemID.Glass, 18);
			recipe2.AddIngredient(ItemID.FallenStar, 6);
			recipe2.AddIngredient(null, "UpgradeCrystal", 20);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
