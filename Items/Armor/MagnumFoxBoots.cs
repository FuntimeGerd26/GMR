using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class MagnumFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Boots");
			Tooltip.SetDefault("Increases melee attack speed by 4%\nIncreases movement speed by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 2;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TungstenBar, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.SilverBar, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddIngredient(ItemID.Ruby, 1);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
