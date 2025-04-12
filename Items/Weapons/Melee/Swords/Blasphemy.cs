using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class Blasphemy : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 54;
			Item.rare = 2;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 220);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 28;
			Item.crit = 0;
			Item.noMelee = true;
			Item.useTurn = false;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.Blasphemy>();
			Item.shootSpeed = 6f;
			Item.knockBack = 6f;
		}

		float shootAlt;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (shootAlt == 0f)
				shootAlt = 1f;
			shootAlt *= -1f;

			Projectile.NewProjectile(source, position, velocity * 0, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);

			if (shootAlt >= 1f)
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.BlasphemySlash>(), damage,
				knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBroadsword);
			recipe.AddIngredient(ItemID.Obsidian, 30);
			recipe.AddIngredient(ItemID.CrimtaneBar, 12);
			recipe.AddIngredient(ItemID.TissueSample, 20);
			recipe.AddIngredient(ItemID.AshBlock, 30);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GoldBroadsword);
			recipe2.AddIngredient(ItemID.Obsidian, 30);
			recipe2.AddIngredient(ItemID.DemoniteBar, 12);
			recipe2.AddIngredient(ItemID.ShadowScale, 20);
			recipe2.AddIngredient(ItemID.AshBlock, 30);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PlatinumBroadsword);
			recipe3.AddIngredient(ItemID.Obsidian, 30);
			recipe3.AddIngredient(ItemID.CrimtaneBar, 12);
			recipe3.AddIngredient(ItemID.TissueSample, 20);
			recipe3.AddIngredient(ItemID.AshBlock, 30);
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.PlatinumBroadsword);
			recipe4.AddIngredient(ItemID.Obsidian, 30);
			recipe4.AddIngredient(ItemID.DemoniteBar, 12);
			recipe4.AddIngredient(ItemID.ShadowScale, 20);
			recipe4.AddIngredient(ItemID.AshBlock, 30);
			recipe4.AddTile(TileID.Anvils);
			recipe4.Register();
		}
	}
}