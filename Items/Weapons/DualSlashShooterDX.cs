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
	public class DualSlashShooterDX : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dual Slash Shooter DX");
			Tooltip.SetDefault("'Testing Complete'");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 36;
			Item.value = Item.sellPrice(silver: 125);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.rare = 7;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.autoReuse = true;
			Item.UseSound = SoundID.NPCHit4;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DualSlashShooter");
			recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
			recipe.AddIngredient(ItemID.Ectoplasm, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualSlashBlade");
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "DualBlasterShooter");
			recipe3.Register();
		}
	}
}