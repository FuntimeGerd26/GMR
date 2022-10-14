using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class ArkChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Body"); //憎悪, Wrath
			Tooltip.SetDefault("'憎悪'\nIncreases all weapon damage and armor penetration by 10%, increases attack speed by 15%\nIncreases knockback of weapons\nIncreases max minion slots by 2, increases damage reduction by 5%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.rare = 9;
			Item.value = Item.sellPrice(silver: 260);
			Item.maxStack = 1;
			Item.defense = 22;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetKnockback(DamageClass.Generic) += 1f;
			player.GetArmorPenetration(DamageClass.Generic) += 10f;
			player.endurance = 1f - (0.05f * (1f - player.endurance));
			player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
			player.GetDamage(DamageClass.Generic) += 0.10f;
			player.maxMinions = 2;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodChestplate");
			recipe.AddIngredient(ItemID.FragmentNebula, 20);
			recipe.AddIngredient(ItemID.FragmentStardust, 20);
			recipe.AddIngredient(ItemID.FragmentVortex, 20);
			recipe.AddIngredient(ItemID.FragmentSolar, 20);
			recipe.AddIngredient(3467, 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
