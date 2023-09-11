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
	public class MaskedPlagueEmblemRanged : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Ranger Emblem");
			Tooltip.SetDefault("Increases ranged attack speed by 2%\nIncreases ranged damage by 3%\nIncreases movement speed by 5%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 62;
			Item.value = Item.sellPrice(silver: 105);
			Item.rare = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetAttackSpeed(DamageClass.Ranged) += 0.02f;
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 14);
			recipe.AddIngredient(ItemID.Silk, 12);
			recipe.AddIngredient(ItemID.Emerald, 6);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LeadBar, 14);
			recipe2.AddIngredient(ItemID.Silk, 12);
			recipe2.AddIngredient(ItemID.Emerald, 6);
			recipe2.AddIngredient(null, "UpgradeCrystal", 45);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}