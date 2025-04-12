using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Staffs
{
	public class AluminiumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots a small homing bolt");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.rare = 1;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 45);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 8;
			Item.crit = 6;
			Item.knockBack = 0.5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.AluminiumBolt>();
			Item.shootSpeed = 12f;
			Item.mana = 5;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 15);
			recipe.AddIngredient(ItemID.Glass, 25);
			recipe.AddIngredient(ItemID.FallenStar, 10);
			recipe.AddIngredient(null, "UpgradeCrystal", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 15);
			recipe2.AddIngredient(ItemID.Glass, 25);
			recipe2.AddIngredient(ItemID.FallenStar, 10);
			recipe2.AddIngredient(null, "UpgradeCrystal", 15);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}