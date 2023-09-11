using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagmaticVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee and summon damage by 2%\nIncreases melee crit chance and summon attack speed by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = 3;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.02f;
			player.GetDamage(DamageClass.Summon) += 0.02f;
			player.GetCritChance(DamageClass.Melee) += 3f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagmaticChestplate>() && legs.type == ModContent.ItemType<MagmaticBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases armor penetration by 10%\nMakes you explode into a rain of fireballs when hit";
			player.GetArmorPenetration(DamageClass.Generic) += 0.1f;
			player.GPlayer().MagmaSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemHelmet");
			recipe.AddIngredient(ItemID.HellstoneBar, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}