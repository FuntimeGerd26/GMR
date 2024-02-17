using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class OvercooledRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overcooled Rifle");
			Tooltip.SetDefault($" Shoots 1-2 bullets with some spread that move faster than normal bullets" +
			$"\nReplaces normal bullets with overcooled bullets that inflict Chillburn");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 90;
			Item.height = 32;
			Item.rare = 5;
			Item.useTime = 3;
            Item.useAnimation = 12;
			Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 160);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 38;
			Item.crit = 4;
			Item.knockBack = 1.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -6);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = position - 10 * Vector2.UnitY;
			int ProjectileCount = 1 + Main.rand.Next(1);
			for (int i = 0; i < ProjectileCount; i++)
			{
				if (type == ProjectileID.Bullet)
				{
					type = ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>();
				}
				else
					type = type;

				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity.RotatedByRandom(MathHelper.ToRadians(8)), type, (int)(damage * 0.8), knockback, player.whoAmI);
				Main.projectile[p].extraUpdates += 5;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ChargeRifle");
			recipe.AddIngredient(ItemID.SoulofNight, 18);
			recipe.AddIngredient(null, "InfraRedBar", 8);
			recipe.AddIngredient(ItemID.HallowedBar, 14);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}