using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class ArkBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts 'Devilish' to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 70;
			Item.rare = 8;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.buyPrice(silver: 495);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 98;
			Item.crit = 8;
			Item.knockBack = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.ArkBladeSwing>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodSword");
			recipe.AddIngredient(null, "CrystalNeonSword");
			recipe.AddIngredient(ItemID.SpectreBar, 16);
			recipe.AddIngredient(ItemID.SoulofFright, 14);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddIngredient(null, "ScrapFragment", 24);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}