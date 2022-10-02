using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;

namespace GMR.Items.Weapons
{
	public class DualSlashShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'A weapon in beta stage, handle it carefuly'\nRight-click to transform into Sword Mode");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 36;
			Item.value = Item.sellPrice(silver: 125);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.autoReuse = true;
			Item.UseSound = SoundID.NPCHit4;
		}

		public override bool? UseItem(Player player)
		{
			Main.NewText("[c/666666:ERROR]");
			return true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			Main.NewText("[c/0000FF:Blade Mode]");
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.DualSlashCutter>(), 1));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.SoulofNight, 22);
			recipe.AddIngredient(ItemID.SoulofMight, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}