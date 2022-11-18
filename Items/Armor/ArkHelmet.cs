using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ArkHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark Headgear"); //絶滅, Extinction
			Tooltip.SetDefault("'絶滅'\nIncreases all weapon damage, crit chance, attack speed and armor penetration by 15% if the player is below 50% health\nIncreases damage and attack speed by 5%\nIncreases max minion slots by 2, increases max mana by 60");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.rare = 9;
			Item.value = Item.sellPrice(silver: 220);
			Item.maxStack = 1;
			Item.defense = 22;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.maxMinions += 2;
			player.statManaMax2 += 60;
			if (player.statLife < player.statLifeMax)
			{
				player.GetDamage(DamageClass.Generic) += 0.15f;
				player.GetCritChance(DamageClass.Generic) += 15f;
				player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
			}
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ArkChestplate>() && legs.type == ModContent.ItemType<ArkBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 2\nIncreases all weapon speed and damage by 10%\nWhen under 75% of health: increased armor penetration, movement speed, crit chance and damage by 7%";
			player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
			player.GetDamage(DamageClass.Generic) += 0.1f;
			player.maxMinions += 2;

			if (player.statLife < ((player.statLifeMax / 2) + (player.statLifeMax / 4)))
			{
				player.GetDamage(DamageClass.Generic) += 0.07f;
				player.GetCritChance(DamageClass.Generic) += 7f;
				player.GetArmorPenetration(DamageClass.Generic) += 7f;
				player.moveSpeed += 0.07f;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodHelmet");
			recipe.AddIngredient(null, "MagnumFoxMask");
			recipe.AddIngredient(3467, 12);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
