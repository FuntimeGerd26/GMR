using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class OvercooledSniperRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overcooled Sniper Rifle");
			Tooltip.SetDefault($" Replaces normal bullets with overcooled bullets that inflict Chillburn\n Bullets are also far quicker than normal bullets and pierce 3 more enemies(Piercing dosen't work on Chlorophyte bullets)" +
				$"\n Right-Click to shoot a faster bullet that deals significantly damage and pierces 5 times instead of 3 (Unless you're using Chlorophyte bullets)");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 104;
			Item.height = 32;
			Item.rare = 5;
			Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 160);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 66;
			Item.crit = 6;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -6);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.damage = 115;
				Item.knockBack = 3f;
				Item.useTime = 80;
				Item.useAnimation = 80;
			}
			else
			{
				Item.damage = 66;
				Item.knockBack = 2.5f;
				Item.useTime = 45;
				Item.useAnimation = 45;
			}

			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			if (player.altFunctionUse == 2)
			{
				for (int i = 0; i < 1; i++)
				{
					if (type == ProjectileID.Bullet)
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>();
					}
					else
						type = type;

					velocity = velocity * 2;
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, (int)(damage * 1.75), knockback + 0.5f, player.whoAmI);
					Main.projectile[p].extraUpdates += 25;
					if (type != ProjectileID.ChlorophyteBullet)
						Main.projectile[p].penetrate += 5;
				}
			}
			else
			{
				for (int i = 0; i < 1; i++)
				{
					if (type == ProjectileID.Bullet)
					{
						type = ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>();
					}
					else
						type = type;

					velocity = velocity * 2;
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, (int)(damage * 1.25), knockback, player.whoAmI);
					Main.projectile[p].extraUpdates += 20;
					if (type != ProjectileID.ChlorophyteBullet)
						Main.projectile[p].penetrate += 3;
				}
			}
            SoundEngine.PlaySound(SoundID.Item40, player.position);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ChargeRifle");
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(null, "InfraRedBar", 8);
			recipe.AddIngredient(ItemID.HallowedBar, 22);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}