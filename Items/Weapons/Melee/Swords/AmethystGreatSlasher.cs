using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class AmethystGreatSlasher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Great Slasher");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.VioletGreatSword2>(30);
			Item.SetWeaponValues(70, 12f, 0);
			Item.scale = 1f;
			Item.width = 68;
			Item.height = 68;
			Item.rare = 4;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 190);
			Item.autoReuse = true;
		}

		float flipDir;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (flipDir <= -1)
				flipDir = 1;
			else if (flipDir >= 1)
				flipDir = -1;
			else if (flipDir == 0)
				flipDir = -1;

			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.VioletGreatSword>(), damage,
				knockback, player.whoAmI, player.direction * player.gravDir * flipDir,
				(int)(player.itemAnimationMax * 0.8f));

			return true;
		}

		public override bool? UseItem(Player player)
		{
			Item.FixSwing(player);
			return null;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystSword");
			recipe.AddIngredient(ItemID.OrichalcumBar, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddIngredient(ItemID.PearlstoneBlock, 45);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AmethystSword");
			recipe2.AddIngredient(ItemID.MythrilBar, 10);
			recipe2.AddIngredient(ItemID.SoulofLight, 16);
			recipe2.AddIngredient(ItemID.PearlstoneBlock, 45);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}