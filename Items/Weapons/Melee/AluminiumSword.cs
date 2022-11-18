using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class AluminiumSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Anything, but it...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 54;
			Item.rare = 1;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 60);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 15;
			Item.crit = 4;
			Item.knockBack = 3f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 18);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ItemID.FallenStar, 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 18);
			recipe2.AddIngredient(ItemID.Glass, 20);
			recipe2.AddIngredient(ItemID.FallenStar, 8);
			recipe2.AddIngredient(null, "UpgradeCrystal", 20);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}