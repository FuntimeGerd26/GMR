using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class Unbending : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 86;
			Item.rare = 6;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.reuseDelay = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 175);
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 75;
			Item.crit = 6;
			Item.knockBack = 10f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.Unbending>();
			Item.shootSpeed = 12f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 3;
				Item.useAnimation = 10;
				Item.reuseDelay = 30;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noUseGraphic = true;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.UnbendingStab>();
			}
			else
			{
				Item.useTime = 24;
				Item.useAnimation = 24;
				Item.reuseDelay = 0;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noUseGraphic = false;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.Unbending>();
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(20f)), type, (int)(damage * 1.5f), knockback, player.whoAmI);
			}
			else
			{
				Projectile.NewProjectile(source, position, velocity * 0f, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ShadowSword");
			recipe.AddIngredient(ItemID.SpectreBar, 26);
			recipe.AddIngredient(null, "InfraRedBar", 30);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}