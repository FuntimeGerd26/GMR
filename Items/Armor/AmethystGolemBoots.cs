using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class AmethystGolemBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Boots");
			Tooltip.SetDefault("Increases ranged speed by 3%, and melee speed by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.03f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 7);
			recipe.AddIngredient(ItemID.Amethyst, 3);
			recipe.AddIngredient(null, "UpgradeCrystal", 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
