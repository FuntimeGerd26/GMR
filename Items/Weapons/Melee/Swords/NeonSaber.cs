using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class NeonSaber : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 76;
			Item.rare = 2;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 105);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item15;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 28;
			Item.crit = -2;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.NeonSaber>();
			Item.shootSpeed = 20f;
			Item.knockBack = 6f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity * 0, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.NeonSaberSlash>(), damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TissueSample, 20);
			recipe.AddIngredient(ItemID.CrimstoneBlock, 60);
			recipe.AddIngredient(null, "MagmaticShard", 10);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.ShadowScale, 20);
			recipe2.AddIngredient(ItemID.EbonstoneBlock, 60);
			recipe2.AddIngredient(null, "MagmaticShard", 10);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}