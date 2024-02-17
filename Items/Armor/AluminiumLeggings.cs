using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
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
