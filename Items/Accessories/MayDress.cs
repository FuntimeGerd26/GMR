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
	public class MayDress : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 255, 255),
			new Color(225, 200, 255),
			new Color(0, 0, 0),
		};

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 64;
			Item.value = Item.sellPrice(silver: 240);
			Item.DamageType = DamageClass.Generic;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().DevInmune = true;
			if (player.GPlayer().EnchantToggles["MultipleProjectile"])
			{
				player.GPlayer().DevPlush = Item;
			}

			if (player.GPlayer().EnchantToggles["MayDress"])
			{
				player.GPlayer().MayDress = true;
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.MaySaw>()] < 1)
				{
					const int max = 6;
					Vector2 velocity = new Vector2(0f, -6f);
					for (int i = 0; i < max; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(2 * Math.PI / max * i);
						Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Summon.MaySaw>(),
							Item.damage, Item.knockBack, player.whoAmI, 0, (player.Center - 180 * Vector2.UnitX - player.position).Length());
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
			recipe.AddIngredient(ItemID.BlackDye, 1);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(null, "DevPlushie");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}