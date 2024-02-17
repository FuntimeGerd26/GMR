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
	public class MaskedPlagueEmblemSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Summoner Emblem");
			Tooltip.SetDefault("Increases summon knockback by 2%\nIncreases summon damage by 3%\nIncreases max minion slots by 1");

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
			player.GetKnockback(DamageClass.Summon) += 0.02f;
			player.GetDamage(DamageClass.Summon) += 0.03f;
			player.maxMinions += 1;
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