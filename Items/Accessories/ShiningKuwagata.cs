using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class ShiningKuwagata : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shining Beetle Emblem");
			Tooltip.SetDefault($"Increases attack speed and movement speed by 6%\nIncreases max minion slots by 1\nDecreases knockback by 10%\n'You are the King!'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 44;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.06f;
			player.moveSpeed += 0.06f;
			player.runAcceleration += 0.06f;
			player.maxRunSpeed += 0.06f;
            player.maxMinions += 1;
			player.GetKnockback(DamageClass.Generic) += 0.1f;
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.IronBar, 18);
			recipe.AddIngredient(null, "UpgradeCrystal", 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Diamond);
			recipe2.AddIngredient(ItemID.LeadBar, 18);
			recipe2.AddIngredient(null, "UpgradeCrystal", 30);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}