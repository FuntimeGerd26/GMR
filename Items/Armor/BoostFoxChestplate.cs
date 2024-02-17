using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class BoostFoxChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Chestplate");
			Tooltip.SetDefault($"Increases melee, magic and summon damage and attack speed by 7%\nDecreases ranged damage by 10%\nIncreases melee knockback by 20%\nIncreases movement speed by 10%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
				player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
				player.GetAttackSpeed(DamageClass.Magic) += 0.07f;
				player.GetAttackSpeed(DamageClass.Summon) += 0.07f;
				player.GetDamage(DamageClass.Melee) += 0.07f;
				player.GetDamage(DamageClass.Magic) += 0.07f;
				player.GetDamage(DamageClass.Summon) += 0.07f;
				player.GetDamage(DamageClass.Ranged) += -0.1f;
				player.GetKnockback(DamageClass.Melee) += 0.20f;
				player.moveSpeed += 0.10f;
				player.runAcceleration += 0.2f;
				player.maxRunSpeed += 0.2f;
		}

		public override void AddRecipes()
		{
				Recipe recipe = CreateRecipe();
				recipe.AddIngredient(ItemID.OrichalcumBar, 28);
				recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
				recipe.AddIngredient(ItemID.Amber);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.Register();

				Recipe recipe2 = CreateRecipe();
				recipe2.AddIngredient(ItemID.MythrilBar, 28);
				recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
				recipe2.AddIngredient(ItemID.Amber);
				recipe2.AddTile(TileID.MythrilAnvil);
				recipe2.Register();
		}
	}
}