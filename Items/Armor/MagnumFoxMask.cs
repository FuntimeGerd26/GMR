using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MagnumFoxMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Mask");
			Tooltip.SetDefault("Increases all damage by 10%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 85);
			Item.rare = 2;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.10f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagnumFoxChestplate>() && legs.type == ModContent.ItemType<MagnumFoxBoots>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = $"Increases the speed of ranged velocity\nIncreases base ranged damage by 8 and endurance by 2%\nIncreases max health by 5%";
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Ranged).Base += 8f;
			player.statLifeMax2 += player.statLifeMax / 20;
			player.GPlayer().MagnumSet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe.AddIngredient(ItemID.Goggles);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.DemoniteBar, 25);
			recipe2.AddIngredient(ItemID.Goggles);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddIngredient(ItemID.ShadowScale, 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}