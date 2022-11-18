using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BoostFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Boots");
			if (Main.hardMode)
			{
				Tooltip.SetDefault($"Increases movement speed by 25%\nIncreases all weapon attack speed by 15%\nIncreases wing time by 10%\n[i:{ModContent.ItemType<UI.ItemEffectIcon>()}] Allows walking on water and lava");
			}
			else
			{
				Tooltip.SetDefault("Increases melee and magic attack speed by 4%\nIncreases movement speed by 15%\nIncreases max minions by 1");
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 4;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.hardMode)
            {
				player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
				player.moveSpeed += 0.25f;
			    player.wingTimeMax += player.wingTimeMax / 10;
				player.waterWalk = true;
				player.fireWalk = true;
				Item.defense = 8;
				player.runAcceleration += 0.5f;
				player.maxRunSpeed += 0.5f;
			}
			else
            {
				player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
				player.GetAttackSpeed(DamageClass.Magic) += 0.04f;
				player.moveSpeed += 0.15f;
				player.maxMinions++;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmorMoldBoots");
			recipe.AddIngredient(ItemID.TungstenBar, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(ItemID.Amber, 1);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ArmorMoldBoots");
			recipe2.AddIngredient(ItemID.SilverBar, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddIngredient(ItemID.Amber, 1);
			recipe2.AddIngredient(ItemID.Silk, 15);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
