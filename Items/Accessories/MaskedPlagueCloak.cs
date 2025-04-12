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
			if (player.GPlayer().EnchantToggles["MaskedPlagueCloak"])
				player.GPlayer().MaskedPlagueCloak = Item;
			player.GetDamage(DamageClass.Magic) += 0.03f;
			player.GetDamage(DamageClass.Summon) += 0.08f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 8);
			recipe.AddIngredient(ItemID.ShadowScale, 12);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddIngredient(null, "UpgradeCrystal", 25);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Feather, 8);
			recipe2.AddIngredient(ItemID.TissueSample, 12);
			recipe2.AddIngredient(ItemID.Silk, 14);
			recipe2.AddIngredient(null, "UpgradeCrystal", 25);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}