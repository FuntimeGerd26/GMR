using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class UltraBlueShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 82;
			Item.height = 24;
			Item.rare = 4;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 200);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 28;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueBullet>();
			Item.shootSpeed = 2f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-14, -2);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = position.Y - 6;

			for (int i = 0; i < 5; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));

				newVelocity *= 1f - Main.rand.NextFloat(0.5f);

				Projectile.NewProjectile(source, position, newVelocity, ModContent.ProjectileType<Projectiles.Ranged.UltraBlueBullet>(), damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MythrilBar, 16);
			recipe.AddIngredient(ItemID.SoulofNight, 25);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.OrichalcumBar, 16);
			recipe2.AddIngredient(ItemID.SoulofNight, 25);
			recipe2.AddIngredient(null, "AncientInfraRedPlating", 30);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}