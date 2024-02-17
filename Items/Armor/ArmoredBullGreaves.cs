using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
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