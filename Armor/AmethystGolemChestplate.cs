using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class AmethystGolemChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Chestplate");
			Tooltip.SetDefault("Increases all damage by 4%\nIncreases damage reduction by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 40);
			Item.rare = 2;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.endurance = 1f - (0.03f * (1f - player.endurance));
			player.GetDamage(DamageClass.Generic) += 0.04f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 10);
			recipe.AddIngredient(ItemID.Amethyst, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 10);
			recipe2.AddIngredient(ItemID.Amethyst, 6);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}