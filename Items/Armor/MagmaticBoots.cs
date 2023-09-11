using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class MagmaticBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee and summoner attack speed by 5%\nIncreases movement speed by 15%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 85);
			Item.rare = 3;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
			player.moveSpeed += 0.15f;
			player.runAcceleration += 0.02f;
			player.maxRunSpeed += 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemBoots");
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
