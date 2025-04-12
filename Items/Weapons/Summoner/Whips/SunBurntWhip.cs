using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Summoner.Whips
{
	public class SunBurntWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Made durning the peak light of the sun shined on it's maker's town'\nWhen hitting enemies causes an explosion\nInflicts Sun Burn to enemies and yourself");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 42;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 65);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Summon.Whips.SunBurntWhip>(), 28, 8, 4);
			Item.shootSpeed = 6f;
			Item.channel = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddIngredient(ItemID.Amber, 4);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PlatinumBar, 15);
			recipe2.AddIngredient(ItemID.Amber, 4);
			recipe2.AddIngredient(ItemID.Silk, 8);
			recipe2.AddIngredient(null, "UpgradeCrystal", 20);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}