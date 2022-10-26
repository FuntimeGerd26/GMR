using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class CrystalNeonSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Slashy slash'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 84;
			Item.rare = 3;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.reuseDelay = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.CrystalNeonSpin>();
			Item.shootSpeed = 1f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "NeonBlade");
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(ItemID.Pearlwood, 30);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}