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
	public class MaskedPlagueEmblemMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Mage Emblem");
			Tooltip.SetDefault("Increases magic attack speed by 2%\nIncreases magic damage by 3%\nIncreases mana by 20");

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
			player.GetAttackSpeed(DamageClass.Magic) += 0.02f;
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.statManaMax2 = player.statManaMax2 + 20;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("IronBar", 14);
			recipe.AddIngredient(ItemID.Silk, 12);
			recipe.AddIngredient(ItemID.Emerald, 6);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}