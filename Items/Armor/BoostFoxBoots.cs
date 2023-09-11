using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BoostFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Boots");
			Tooltip.SetDefault($"Increases movement speed by 25%\nIncreases all weapon attack speed by 15%\nIncreases wing time by 10%\n Allows walking on water and lava");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 4;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
				player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
				player.moveSpeed += 0.25f;
			    player.wingTimeMax += player.wingTimeMax / 10;
				player.waterWalk = true;
				player.fireWalk = true;
				player.runAcceleration += 0.5f;
				player.maxRunSpeed += 0.5f;
		}

		public override void AddRecipes()
		{
				Recipe recipe = CreateRecipe();
				recipe.AddIngredient(ItemID.PalladiumBar, 20);
				recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
				recipe.AddIngredient(ItemID.Amber, 1);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.Register();

				Recipe recipe2 = CreateRecipe();
				recipe2.AddIngredient(ItemID.CobaltBar, 20);
				recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
				recipe2.AddIngredient(ItemID.Amber, 1);
				recipe2.AddTile(TileID.MythrilAnvil);
				recipe2.Register();
		}
	}
}
