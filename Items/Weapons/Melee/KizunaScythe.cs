using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class KizunaScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Projectiles increase their damage when hitting an enemy");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 88;
			Item.height = 90;
			Item.rare = 5;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 30);
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.KizunaScythe>();
			Item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 18);
			recipe.AddIngredient(null, "AlloyBox", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.AdamantiteBar, 18);
			recipe2.AddIngredient(null, "AlloyBox", 2);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}