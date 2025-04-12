using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Books
{
	public class SoulSnatchingTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.rare = 1;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 65);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 18;
			Item.crit = -2;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.SoulSnatchingHand>();
			Item.shootSpeed = 16f;
			Item.mana = 6;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = player.Center.Y + 600;
			for (int i = 0; i < 4; i++)
			{
				position.X = player.Center.X + (player.direction * 80 * (i + 1));

				Projectile.NewProjectile(source, position, new Vector2(0f, -16f), type, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Book, 2);
			recipe.AddIngredient(ItemID.DemoniteBar, 5);
			recipe.AddIngredient(null, "BossUpgradeCrystal");
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Book, 2);
			recipe2.AddIngredient(ItemID.CrimtaneBar, 5);
			recipe2.AddIngredient(null, "BossUpgradeCrystal");
			recipe2.AddTile(TileID.DemonAltar);
			recipe2.Register();
		}
	}
}