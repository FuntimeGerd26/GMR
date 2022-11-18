using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class AlloybloodChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Torso"); //殺意, Bloodthirst
			Tooltip.SetDefault("'殺意'\nIncreases damage by 12% and movement speed by 5%\nIncreases weapon speed by 7%\nIncreases max minions by 1\nDecreases mana cost by 18%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = 8;
			Item.value = Item.sellPrice(silver: 140);
			Item.maxStack = 1;
			Item.defense = 28;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.07f;
			player.maxMinions += 1;
			player.manaCost -= 0.18f;
			player.GetDamage(DamageClass.Generic) += 0.12f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 24);
			recipe.AddIngredient(ItemID.Ectoplasm, 16);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddIngredient(null, "ArmorMoldChestplate");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.HallowedBar, 24);
			recipe2.AddIngredient(ItemID.Ectoplasm, 16);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe2.AddIngredient(null, "ArmorMoldChestplate");
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
