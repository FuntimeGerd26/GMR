using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AmethystGolemHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Visor");
			Tooltip.SetDefault("Increases melee and minion damage by 5% and ranged & magic speed by 3%\nIncreases damage reduction by 2%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 30);
			Item.rare = 2;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.endurance = 1f - (0.02f * (1f - player.endurance));
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Magic) += 0.03f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AmethystGolemChestplate>() && legs.type == ModContent.ItemType<AmethystGolemBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases damage reduction by 2%\nIncreases max minions by 1\nIncreases melee speed by 5%\nIncreases all damage by 8%";
			player.endurance = 1f - (0.02f * (1f - player.endurance));
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.maxMinions++;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 8);
			recipe.AddIngredient(ItemID.Amethyst, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 8);
			recipe2.AddIngredient(ItemID.Amethyst, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}