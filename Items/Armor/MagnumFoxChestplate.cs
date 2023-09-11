using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class MagnumFoxChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Chestplate");
			Tooltip.SetDefault("Increases all weapon attack speed by 5%\nIncreases all knockback slightly");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = 2;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.GetKnockback(DamageClass.Generic) += 0.5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TungstenBar, 30);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.SilverBar, 30);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddIngredient(ItemID.Ruby, 1);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}