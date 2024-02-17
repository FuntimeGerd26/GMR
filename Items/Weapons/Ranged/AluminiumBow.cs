using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AluminiumBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Bow");
			Tooltip.SetDefault("'Bet those aliens won't know what hit them'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 56;
			Item.rare = 1;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 8;
			Item.crit = 4;
			Item.knockBack = 5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 8f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 16);
			recipe.AddIngredient(ItemID.Glass, 12);
			recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 16);
			recipe2.AddIngredient(ItemID.Glass, 12);
			recipe2.AddIngredient(ItemID.FallenStar, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 45);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}