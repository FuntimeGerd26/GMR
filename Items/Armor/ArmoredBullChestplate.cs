using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
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
			player.aggro *= (int)(1.5);
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
}