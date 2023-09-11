using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class BoostFoxMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Mask");
			Tooltip.SetDefault("Increases melee damage by 10%\nIncreases melee speed and magic damage by 6%\nIncreases mana regen and reduces mana cost by 5%\nIncreases movement speed by 15%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 95);
			Item.rare = 4;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
				player.GetDamage(DamageClass.Melee) += 0.1f;
				player.GetAttackSpeed(DamageClass.Melee) += 0.06f;
				player.GetDamage(DamageClass.Magic) += 0.06f;
				player.manaRegen += 5;
				player.manaCost -= 0.05f;
				player.moveSpeed += 0.15f;
				player.runAcceleration += 0.2f;
				player.maxRunSpeed += 0.2f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<BoostFoxChestplate>() && legs.type == ModContent.ItemType<BoostFoxBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases all damage by 8%\nIncreases max mana by 10%\n Melee weapons that shoot projectiles now shoot an aditional fire ball";
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.statManaMax2 += player.statManaMax / 10;
			player.GPlayer().BoostSet = Item;
		}

		public override void AddRecipes()
		{
				Recipe recipe = CreateRecipe();
				recipe.AddIngredient(ItemID.PalladiumBar, 20);
				recipe.AddIngredient(ItemID.Goggles);
				recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
				recipe.AddIngredient(ItemID.Amber, 2);
				recipe.AddTile(TileID.MythrilAnvil);
				recipe.Register();

				Recipe recipe2 = CreateRecipe();
				recipe2.AddIngredient(ItemID.CobaltBar, 20);
				recipe2.AddIngredient(ItemID.Goggles);
				recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
				recipe2.AddIngredient(ItemID.Amber, 2);
				recipe2.AddTile(TileID.MythrilAnvil);
				recipe2.Register();
		}
	}
}