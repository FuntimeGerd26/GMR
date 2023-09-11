using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
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
