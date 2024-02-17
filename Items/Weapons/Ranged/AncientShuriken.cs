using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AncientShuriken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Shuriken");
			Tooltip.SetDefault("Inflicts 'Partially Crystalized' to enemies, and damage increases by 5% for every enemy struck");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.rare = 3;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 135);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 38;
			Item.crit = 4;
			Item.knockBack = 2f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.AncientShuriken>();
			Item.shootSpeed = 24f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AluminiumShuriken");
			recipe.AddIngredient(ItemID.Wire, 20);
			recipe.AddIngredient(ItemID.Bone, 35);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}