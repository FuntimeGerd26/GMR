using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class MagnumFoxBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnum Boots");
			if (Main.hardMode)
			{
				Tooltip.SetDefault($"Increases movement speed by 10%\nIncreases all weapon attack speed by 12%\nIncreases time in lava by 1 second\n[i:{ModContent.ItemType<UI.ItemEffectIcon>()}] Increases jump height and negates fall damage");
			}
			else
			{
				Tooltip.SetDefault("Increases melee attack speed by 4%\nIncreases movement speed by 5%");
			}
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = 4;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			if (Main.hardMode)
            {
				player.GetAttackSpeed(DamageClass.Generic) += 0.12f;
				player.moveSpeed += 0.1f;
				player.lavaMax += 60;
				player.jumpBoost = true;
				player.noFallDmg = true;
				Item.defense = 12;
			}
			else
            {
				player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
				player.moveSpeed += 0.05f;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmorMoldBoots");
			recipe.AddIngredient(ItemID.TungstenBar, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ArmorMoldBoots");
			recipe2.AddIngredient(ItemID.SilverBar, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddIngredient(ItemID.Ruby, 1);
			recipe2.AddIngredient(ItemID.Silk, 15);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
