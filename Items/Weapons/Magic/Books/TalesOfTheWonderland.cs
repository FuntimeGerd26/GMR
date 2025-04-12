using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Books
{
	public class TalesOfTheWonderland : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 26;
			Item.rare = 3;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 30;
			Item.crit = 0;
			Item.knockBack = 18f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.WonderBlade>();
			Item.shootSpeed = 24f;
			Item.mana = 20;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 4; i++)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(30f)), type, damage, knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item43, player.Center);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Book, 3);
			recipe.AddIngredient(ItemID.BlackThread, 2);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddIngredient(ItemID.Deathweed, 5);
			recipe.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Book, 3);
			recipe2.AddIngredient(ItemID.BlackThread, 2);
			recipe2.AddIngredient(ItemID.ShadowScale, 10);
			recipe2.AddIngredient(ItemID.Deathweed, 5);
			recipe2.AddIngredient(ItemID.MeteoriteBar, 4);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddTile(TileID.DemonAltar);
			recipe2.Register();
		}
	}
}