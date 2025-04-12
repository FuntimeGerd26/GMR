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

namespace GMR.Items.Weapons.Magic.Books
{
	public class AncientBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.rare = 3;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 10;
			Item.crit = -3;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.AncientInfraRedCrate>();
			Item.shootSpeed = 8f;
			Item.mana = 3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AncientInfraRedPlating", 20);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}