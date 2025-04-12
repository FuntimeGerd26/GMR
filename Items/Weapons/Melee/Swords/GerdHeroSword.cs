using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.Projectiles.Melee;
using GMR.Projectiles.Melee.SpecialSwords;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class GerdHeroSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hero Training Sword");
			Tooltip.SetDefault("Shoots swords that rotate around you and over time shoot homing swords\nIncreases melee speed and all damage by 5% and increases critical chance by 7% while in your inventory");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 70;
			Item.rare = 4;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 275);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 35;
			Item.crit = 1;
			Item.knockBack = 7f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.GerdHeroSword>();
			Item.shootSpeed = 15f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.GerdHeroSwordSpin>()] > 0)
            {
				return false;
            }
			else if (player.altFunctionUse == 2)
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.UseSound = SoundID.Item25;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useTime = 35;
				Item.useAnimation = 35;
				Item.noUseGraphic = false;
				Item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				for (int i = 0; i < 8; i++)
				{
					Projectile.NewProjectile(source, position, velocity * 0, ModContent.ProjectileType<Projectiles.Melee.GerdHeroSwordSpin>(), damage, knockback, player.whoAmI, i);
				}					
			}
			else
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(7);
				position += Vector2.Normalize(velocity);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 0.45f;
					Projectile.NewProjectile(Item.GetSource_FromThis(), position, perturbedSpeed, type, (int)(damage / 2), knockback, player.whoAmI);
				}

				Projectile.NewProjectile(source, position, velocity * 0, ModContent.ProjectileType<HeroTrainingSwordSwing>(), (int)(damage * 0.75f),
					knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			}
			return false;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(ItemID.PearlstoneBlock, 45);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}