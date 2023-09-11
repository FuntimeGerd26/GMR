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
	public class MaskedPlagueCloak : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Cloak");
			Tooltip.SetDefault($"Using Magic or Summon weapons will shoot a homing projectile that inflicts 'Crystal Plague'\nWhen hitting enemies, magic projectiles have a 10% chance to heal 1% of your max health" +
				"\nIncreases magic damage by 3% and summon damage by 8%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 34;
			Item.value = Item.sellPrice(silver: 65);
			Item.rare = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().MaskedPlagueCloak = Item;
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.GetDamage(DamageClass.Summon) += 0.08f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddRecipeGroup("GMR:AnyGem", 2);
			recipe.AddIngredient(null, "UpgradeCrystal", 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}