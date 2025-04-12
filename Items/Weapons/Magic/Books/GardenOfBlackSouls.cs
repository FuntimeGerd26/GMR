using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Magic.Books
{
	public class GardenOfBlackSouls : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.rare = 5;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 50;
			Item.crit = 0;
			Item.knockBack = 18f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.BlackSoulBlade>();
			Item.shootSpeed = 24f;
			Item.mana = 20;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 8; i++)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(45f)), type, damage, knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item43, player.Center);
			player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.25f);

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "TalesOfTheWonderland");
			recipe.AddIngredient(ItemID.SoulofNight, 50);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}