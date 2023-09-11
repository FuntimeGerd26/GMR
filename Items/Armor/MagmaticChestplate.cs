using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class MagmaticChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee damage by 7%\nIncreases summon damage and armor penetration by 5%\nIncreases max minions and sentries by 1");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = 3;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.07f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetArmorPenetration(DamageClass.Summon) += 0.05f;
			player.maxMinions += 1;
			player.maxTurrets += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemChestplate");
			recipe.AddIngredient(ItemID.HellstoneBar, 14);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}