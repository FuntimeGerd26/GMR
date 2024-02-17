using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace GMR.Items.Weapons.Magic
{
	public class InfraRedBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Tome");
			Tooltip.SetDefault("Creates a crate that stops moving after a second, that on hit with enemies explodes into 4 Infra-Red Beams");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 36;
			Item.rare = 3;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 18;
			Item.crit = 12;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.InfraRedCrate>();
			Item.shootSpeed = 6f;
			Item.mana = 3;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 4);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ScrapFragment", 20);
			recipe.AddIngredient(ItemID.FallenStar, 12);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}