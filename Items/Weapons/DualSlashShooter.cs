using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons
{
	public class DualSlashShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A weapon in beta stage, handle it carefuly'");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 36;
			Item.rare = 6;
			Item.value = Item.sellPrice(silver: 125);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.autoReuse = true;
			Item.UseSound = SoundID.NPCHit4;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.SoulofNight, 22);
			recipe.AddIngredient(ItemID.SoulofMight, 18);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualSlashCutter");
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "DualGunShooter");
			recipe3.Register();
		}
	}
}