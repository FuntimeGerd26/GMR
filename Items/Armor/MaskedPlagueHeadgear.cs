using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MaskedPlagueHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Headgear");
			Tooltip.SetDefault("Increases movement speed by 3%\nIncreases magic and ranged crit chance by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 60);
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.03f;
			player.GetCritChance(DamageClass.Magic) += 5f;
			player.GetCritChance(DamageClass.Ranged) += 5f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MaskedPlagueBreastplate>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases magic and ranged weapon speed by 5%\nIncreases minion damage by 5%";
			player.GetAttackSpeed(DamageClass.Magic) += 0.05f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.maxMinions++;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 20);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 1);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
