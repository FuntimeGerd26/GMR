using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class GerdHandgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Handgun");
			Tooltip.SetDefault($" Bullets that don't pierce now pierce one time" +
				$"\nRight-Click to shoot 3 bursts of 2-4 bullets with increased velocity but deal only 60% of damage");
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
			Item.damage = 40;
			Item.knockBack = 10f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.reuseDelay = 30;
				Item.useTime = 2;
				Item.useAnimation = 12;
				Item.crit = -3;
			}
			else
			{
				Item.reuseDelay = 0;
				Item.useTime = 22;
				Item.useAnimation = 22;
				Item.crit = 2;
			}
			return true;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;

			if (player.altFunctionUse == 2)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(6f));

				for (int i = 0; i < 1; i++)
				{
					// Create a projectile.
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, (int)(damage * 0.4), knockback, player.whoAmI);
					Main.projectile[p].extraUpdates += 5;
					SoundEngine.PlaySound(SoundID.Item11, player.position);
				}
				for (int y = 0; y < 1; y++)
				{
					Dust dustId = Dust.NewDustDirect(position, 2, 2, 66, newVelocity.X / 2, newVelocity.Y / 2, 30, new Color(125, 185, 255), 0.5f);
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
					Dust dustId = Dust.NewDustDirect(position, 2, 2, 66, velocity.X / 2, velocity.Y / 2, 30, new Color(125, 185, 255), 0.5f);
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