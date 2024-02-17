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

namespace GMR.Items.Accessories
{
	public class BLFujoshi : ModItem
	{
		public bool flip;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BL Fujoshi");
			Tooltip.SetDefault("Summons 3 orbiting BL Books and 2 Psycopath Axes around the player\nIncreases all damage by 25%\nIncreases damage taken by 10%\nHide the accessory to only get the buffs\n'Endless BL, How shameless!'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.DamageType = DamageClass.Generic;
			Item.damage = 70;
			Item.crit = 6;
			Item.knockBack = 14f;
			Item.value = Item.sellPrice(silver: 100);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.25f;
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

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "BLBook");
			recipe.AddIngredient(null, "PsycopathAxe");
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}