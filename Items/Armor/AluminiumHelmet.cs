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
			player.GetAttackSpeed(DamageClass.Generic) += 0.01f;
			player.GetCritChance(DamageClass.Generic) += 2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AluminiumBreastplate>() && legs.type == ModContent.ItemType<AluminiumLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases max minions by 1\nIncreases all weapon speed by 2%\nIncreases all damage by 2%\nAttacks have a chance to shoot a shuriken";
			player.GetAttackSpeed(DamageClass.Generic) += 0.02f;
			player.GetDamage(DamageClass.Generic) += 0.02f;
			player.maxMinions++;
			player.GPlayer().AlumArmor = Item;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 8);
			recipe.AddIngredient(ItemID.Glass, 15);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 8);
			recipe2.AddIngredient(ItemID.Glass, 15);
			recipe2.AddIngredient(ItemID.FallenStar, 5);
			recipe2.AddIngredient(null, "UpgradeCrystal", 10);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
