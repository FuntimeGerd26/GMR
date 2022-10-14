using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AluminiumHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Helmet");
			Tooltip.SetDefault("Increases all weapon speed by 2%\nIncreases crit chance by 3%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 16;
			Item.rare = 1;
			Item.value = Item.sellPrice(silver: 30);
			Item.maxStack = 1;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetCritChance(DamageClass.Generic) += 3f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AluminiumBreastplate>() && legs.type == ModContent.ItemType<AluminiumLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases all weapon speed by 4%\nIncreases all damage by 2%";
			player.GetAttackSpeed(DamageClass.Generic) += 0.04f;
			player.GetDamage(DamageClass.Generic) += 0.02f;
			player.maxMinions++;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 6);
			recipe2.AddIngredient(ItemID.Silk, 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
