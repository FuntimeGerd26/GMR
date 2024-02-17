using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class UltraBlueEnergyGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 28;
			Item.rare = 5;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 240);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item36;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 28;
			Item.crit = 1;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueBullet>();
			Item.shootSpeed = 5f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -2);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			position.Y = position.Y - 3;
			type = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueBullet>();

			for (int i = 0; i < 2; i++)
			{
				Projectile.NewProjectile(source, position, velocity * (i + 1 * 0.6f), type, damage, knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(GMR.GetSounds("Items/Ranged/railgunVariant", 2, 0.5f, -0.1f, 0.75f), player.Center);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "UltraBlueShotgun");
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddIngredient(ItemID.SoulofSight, 12);
			recipe.AddIngredient(ItemID.SoulofMight, 12);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}