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
	public class AlloybloodWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($" Inflicts 'Devilish' to enemies\n'Do you remember'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 42;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 65);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.Summon.Whips.AlloybloodWhip>(), 50, 3, 6);
			Item.shootSpeed = 6f;
			Item.channel = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "SunBurntWhip");
			recipe.AddIngredient(ItemID.SoulofFlight, 14);
			recipe.AddIngredient(ItemID.SoulofFright, 18);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 28);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}