using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;
using GMR.Items.Accessories;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
	public class AlloybloodEnchantment : ModItem
	{
		public bool flip;

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases movement speed by 25%\nIncreases knockback by 20%\nIncreases armor penetration and attack speed of all weapons by 5%\nIncreases max minions and sentries by 2" +
				$"\nGrants the 'Blood Overflow' buff" +
				$"\nWhen under 50% of health: increased armor penetration, crit chance, damage by 7% and decreased mana cost by 7%" +
				$"\nSummons 8 orbiting BL Books and Psyco Axes around the player and increases damage taken by 10%, you can hide the accessory to distable this effect" + 
				$"\nMelee weapons shoot an Alloyblood Dagger that inflicts 'Devilish' to enemies, hitting enemies with this dagger has a chance to drop Alloyblood Cans above you which heal 15% life" +
				"\n'Conclusion, Zero'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.DamageType = DamageClass.Generic;
			Item.damage = 100;
			Item.crit = 4;
			Item.knockBack = 14f;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.25f;
			player.maxMinions += 2;
			player.maxTurrets += 2;
			player.manaCost -= 0.18f;
			player.GetKnockback(DamageClass.Generic) += 0.20f;
			player.GetArmorPenetration(DamageClass.Generic) += 5f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			if (ClientConfig.Instance.AlloybloodDagger)
			{
				player.GPlayer().AlloybloodEnch = Item;
			}


			if (player.statLife < player.statLifeMax / 2)
			{
				player.manaCost -= 0.07f;
				player.GetDamage(DamageClass.Generic) += 0.07f;
				player.GetCritChance(DamageClass.Generic) += 7f;
				player.GetArmorPenetration(DamageClass.Generic) += 7f;
			}

			if (!hideVisual)
			{
				player.GPlayer().BLBook = true;
				flip = !flip;
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>()] < 1)
				{
					const int max = 6;
					Vector2 velocity = new Vector2(0f, -2f);
					for (int i = 0; i < max; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.BLFujoshi>(), Item.damage, 14f,
							player.whoAmI, 0, (player.Center - 100 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
					}
				}
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>()] < 1)
				{
					const int max2 = 2;
					Vector2 velocity = new Vector2(0f, -6f);
					for (int y = 0; y < max2; y++)
					{
						Vector2 perturbedSpeed2 = velocity.RotatedBy(2 * Math.PI / max2 * y);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed2, ModContent.ProjectileType<Projectiles.Melee.BLPsycopathAxe>(), Item.damage + 10, 4f,
							player.whoAmI, 0, (player.Center - 200 * Vector2.UnitX - player.position).Length() * (flip ? 1 : 1));
					}
				}
			}
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AlloybloodHelmet");
			recipe.AddIngredient(null, "AlloybloodChestplate");
			recipe.AddIngredient(null, "AlloybloodLeggings");
			recipe.AddIngredient(null, "AlloybloodSword");
			recipe.AddIngredient(null, "AlloybloodShotgun");
			recipe.AddIngredient(null, "DualBlasterShooter");
			recipe.AddIngredient(null, "AlloybloodGenerator");
			recipe.AddIngredient(null, "BLFujoshi");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AlloybloodHelmet");
			recipe2.AddIngredient(null, "AlloybloodChestplate");
			recipe2.AddIngredient(null, "AlloybloodLeggings");
			recipe2.AddIngredient(null, "AlloybloodSword");
			recipe2.AddIngredient(null, "AlloybloodShotgun");
			recipe2.AddIngredient(null, "DualSlashBlade");
			recipe2.AddIngredient(null, "AlloybloodGenerator");
			recipe2.AddIngredient(null, "BLFujoshi");
			recipe2.AddTile(TileID.CrystalBall);
			recipe2.Register();
		}
	}
}