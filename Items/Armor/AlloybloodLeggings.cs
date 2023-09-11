using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class AlloybloodLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloyblood Greaves"); //破滅, Ruin
			Tooltip.SetDefault("'破滅'\nIncreases movement speed by 25%\nIncreases knockback of weapons\nIncreases armor penetration and attack speed of melee and summoner weapons by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 120);
			Item.maxStack = 1;
			Item.defense = 18;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.25f;
			player.GetKnockback(DamageClass.Generic) += 1f;
			player.GetArmorPenetration(DamageClass.Melee) += 5f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetArmorPenetration(DamageClass.Summon) += 5f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.Ectoplasm, 12);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
