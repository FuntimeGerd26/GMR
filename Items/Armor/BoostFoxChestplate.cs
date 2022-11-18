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
			if (Main.hardMode)
			{
			Tooltip.SetDefault($"Increases melee, magic and summon damage and attack speed by 7%\nDecreases ranged damage by 10%\nIncreases melee knockback by 20%\nIncreases movement speed by 10%");
		    }
			else
            {
				Tooltip.SetDefault("Increases melee and summon attack speed by 7%\nIncreases summon and magic damage by 5%\nIncreases melee damage by 9%\nIncreases movement speed by 5%");
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.hardMode)
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
				Item.defense = 10;
				player.runAcceleration += 0.2f;
				player.maxRunSpeed += 0.2f;
			}
			else
            {
				player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
				player.GetAttackSpeed(DamageClass.Summon) += 0.07f;
				player.GetDamage(DamageClass.Magic) += 0.05f;
				player.GetDamage(DamageClass.Summon) += 0.05f;
				player.GetDamage(DamageClass.Melee) += 0.09f;
				player.moveSpeed += 0.05f;
			}				
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmorMoldChestplate");
			recipe.AddIngredient(ItemID.TungstenBar, 28);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddIngredient(ItemID.Amber);
			recipe.AddIngredient(ItemID.Silk, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ArmorMoldChestplate");
			recipe2.AddIngredient(ItemID.SilverBar, 28);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe2.AddIngredient(ItemID.Amber);
			recipe2.AddIngredient(ItemID.Silk, 12);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}