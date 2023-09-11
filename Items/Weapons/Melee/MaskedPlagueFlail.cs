using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class MaskedPlagueFlail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Flail");
			Tooltip.SetDefault("Inflicts poison to enemies\nHitting enemies shoots Masked Plague Stingers");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 54;
			Item.rare = 3;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 133);
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item7;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.damage = 33;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.MaskedPlagueFlail>();
			Item.shootSpeed = 6f;
			Item.channel = true;
			Item.noMelee = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 24);
			recipe.AddIngredient(ItemID.Silk, 18);
			recipe.AddIngredient(ItemID.Bone, 28);
			recipe.AddRecipeGroup("GMR:AnyGem", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}