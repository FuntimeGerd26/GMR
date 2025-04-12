using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
	public class MayAwakeDress : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 155, 155),
			new Color(225, 255, 55),
			new Color(125, 25, 0),
		};

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 68;
			Item.value = Item.sellPrice(silver: 280);
			Item.DamageType = DamageClass.Generic;
			Item.damage = 80;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.rare = 5;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.12f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
			player.GPlayer().DevInmune = true;
			player.GPlayer().AwakeMayDress = true;
			if (player.GPlayer().EnchantToggles["MultipleProjectile"])
			{
				player.GPlayer().DevPlush = Item;
			}

			if (player.GPlayer().EnchantToggles["MayDress"])
			{
				player.GPlayer().MayDress = true;
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.MayAwakeSaw>()] < 1)
				{
					const int max = 6;
					Vector2 velocity = new Vector2(0f, -6f);
					for (int i = 0; i < max; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Summon.MayAwakeSaw>(),
							Item.damage, Item.knockBack, player.whoAmI, 0, (player.Center - 250 * Vector2.UnitX - player.position).Length());
					}
				}
			}
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 40) / 40f;
					int index = (int)((Main.GameUpdateCount / 40) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MayDress");
			recipe.AddIngredient(ItemID.HallowedBar, 14);
			recipe.AddIngredient(ItemID.SoulofSight, 8);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}