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
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 26;
			Item.rare = 4;
			Item.reuseDelay = 0;
			Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 150);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 62;
			Item.crit = 4;
			Item.knockBack = 2.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 6f;
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
				Item.reuseDelay = 24;
				Item.useTime = 3;
				Item.useAnimation = 12;
			}
			else
			{
				Item.reuseDelay = 0;
				Item.useTime = 16;
				Item.useAnimation = 16;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				int ProjectileCount = 2 + Main.rand.Next(2);
				for (int i = 0; i < ProjectileCount; i++)
				{               
					// Rotate the velocity randomly by 10 degrees at max.
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));

					// Decrease velocity randomly for nicer visuals.
					newVelocity *= 1f - Main.rand.NextFloat(0.75f);

					// Create a projectile.
					int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, newVelocity, type, (int)(damage * 0.6), knockback, player.whoAmI);
					Main.projectile[p].extraUpdates += 10;
					Main.projectile[p].penetrate += 1;
					Main.projectile[p].usesLocalNPCImmunity = true;
					SoundEngine.PlaySound(SoundID.Item11, player.position);
				}
			}
			else
			{
				for (int i = 0; i < 1; i++)
                {
                    int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity * 1.5f, type, damage, knockback, player.whoAmI);
					if (Main.projectile[p].penetrate == 1)
	                    Main.projectile[p].penetrate += 1;
					Main.projectile[p].usesLocalNPCImmunity = true;
				}
			}
			type = 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TheUndertaker);
			recipe.AddIngredient(ItemID.SoulofNight, 30);
			recipe.AddIngredient(ItemID.CobaltBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TheUndertaker);
			recipe2.AddIngredient(ItemID.SoulofNight, 30);
			recipe2.AddIngredient(ItemID.PalladiumBar, 15);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.Musket);
			recipe3.AddIngredient(ItemID.SoulofNight, 30);
			recipe3.AddIngredient(ItemID.CobaltBar, 15);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.Musket);
			recipe4.AddIngredient(ItemID.SoulofNight, 30);
			recipe4.AddIngredient(ItemID.PalladiumBar, 15);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}