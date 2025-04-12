using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class GerdHandgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 30;
			Item.rare = 3;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.reuseDelay = 0;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 150);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 34;
			Item.crit = 0;
			Item.knockBack = 10f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.crit = 6;
			}
			else
			{
				Item.useTime = 22;
				Item.useAnimation = 22;
				Item.crit = 0;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 8;

			if (player.altFunctionUse == 2)
			{
				for (int i = 0; i < 1; i++)
				{
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.UltraBlueBullet>(), damage * 2, knockback, player.whoAmI);
				}
				for (int y = 0; y < 4; y++)
				{
					Dust dustId = Dust.NewDustDirect(position + (Vector2.UnitX * 26f).RotatedBy(velocity.ToRotation()), 2, 2, 66, velocity.X / 2, velocity.Y / 2, 30, new Color(125, 185, 255), 1f);
					dustId.noGravity = true;
				}
			}
			else
			{
				for (int i = 0; i < 1; i++)
				{
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
					if (Main.projectile[p].penetrate == 1)
						Main.projectile[p].penetrate += 1;
					Main.projectile[p].usesLocalNPCImmunity = true;
				}
				for (int y = 0; y < 4; y++)
				{
					Dust dustId = Dust.NewDustDirect(position + (Vector2.UnitX * 26f).RotatedBy(velocity.ToRotation()), 2, 2, 66, velocity.X / 2, velocity.Y / 2, 30, new Color(125, 185, 255), 0.5f);
					dustId.noGravity = true;
				}
			}
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FlintlockPistol);
			recipe.AddIngredient(ItemID.SoulofNight, 14);
			recipe.AddIngredient(ItemID.CobaltBar, 30);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.FlintlockPistol);
			recipe2.AddIngredient(ItemID.SoulofNight, 14);
			recipe2.AddIngredient(ItemID.PalladiumBar, 30);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 7);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}