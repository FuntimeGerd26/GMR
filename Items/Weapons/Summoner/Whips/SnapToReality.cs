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
	public class SnapToReality : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Snapper");
			Tooltip.SetDefault("'Oops there goes gravity'\nInflicts 'Chillburn' to enemies" +
			$"\n Shoots an extra wave that inflicts 'Thoughtful' to enemies");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 40;
			Item.rare = 8;
			Item.value = Item.sellPrice(silver: 265);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Summon.Whips.SnapToReality>(), 68, 2, 4);
			Item.shootSpeed = 6f;
			Item.useTime /= 2;
			Item.useAnimation /= 2;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpectreBar, 28);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(ItemID.SoulofFright, 14);
			recipe.AddIngredient(ItemID.SoulofMight, 14);
			recipe.AddIngredient(ItemID.SoulofSight, 14);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}