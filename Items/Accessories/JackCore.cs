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
	public class JackCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 42;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.8f;
			player.moveSpeed += 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 30);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(null, "InfraRedBar", 30);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PalladiumBar, 30);
			recipe2.AddIngredient(ItemID.SoulofLight, 18);
			recipe2.AddIngredient(null, "InfraRedBar", 30);
			recipe2.AddIngredient(null, "InfraRedCrystalShard", 4);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}