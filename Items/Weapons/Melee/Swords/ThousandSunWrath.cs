using GMR;
using System;
using Terraria;
using System.IO;
using Terraria.UI;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Terraria.GameContent;
using GMR.Items.CustomStuff;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class ThousandSunWrath : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 125, 0),
			new Color(125, 125, 0),
		};

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Summons swords from the sides of your cursor\n\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.ThousandSunWrathProj>(38);
			Item.useTime /= 2;
			Item.SetWeaponValues(160, 2.5f, 16);
			Item.width = 110;
			Item.height = 110;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 10;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.buyPrice(silver: 1985);
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 4; i++)
			{
				int ProjType = ModContent.ProjectileType<Projectiles.Melee.ThousandEdge>();
				Vector2 target = Main.MouseWorld + 200 * Vector2.UnitX + 200 * Vector2.UnitY;
				Projectile.NewProjectile(player.GetSource_FromThis(), target, new Vector2(-38f, -38f), ProjType, (int)(Item.damage / 2), 1f, Main.myPlayer);
				Vector2 target2 = Main.MouseWorld + 200 * Vector2.UnitX - 200 * Vector2.UnitY;
				Projectile.NewProjectile(player.GetSource_FromThis(), target2, new Vector2(-38f, 38f), ProjType, (int)(Item.damage / 2), 1f, Main.myPlayer);
				Vector2 target3 = Main.MouseWorld - 200 * Vector2.UnitX + 200 * Vector2.UnitY;
				Projectile.NewProjectile(player.GetSource_FromThis(), target3, new Vector2(38f, -38f), ProjType, (int)(Item.damage / 2), 1f, Main.myPlayer);
				Vector2 target4 = Main.MouseWorld - 200 * Vector2.UnitX - 200 * Vector2.UnitY;
				Projectile.NewProjectile(player.GetSource_FromThis(), target4, new Vector2(38f, 38f), ProjType, (int)(Item.damage / 2), 1f, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.Item45, player.Center);
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "GerdSword");
			recipe.AddIngredient(null, "BiomeSword");
			recipe.AddIngredient(null, "ExolBlade");
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(null, "MagmaticShard", 20);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal", 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 30) / 30f;
					int index = (int)((Main.GameUpdateCount / 30) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Mod == "Terraria" && line.Name == "ItemName")
			{
				DrawDedicatedTooltip(line);
				return false;
			}
			return true;
		}

		public static void DrawDedicatedTooltip(string text, int x, int y, float rotation, Vector2 origin, Vector2 baseScale, Color color)
		{
			float brightness = Main.mouseTextColor / 255f;
			float brightnessProgress = (Main.mouseTextColor - 240f) / (byte.MaxValue - 240f);
			color = Colors.AlphaDarken(color);
			color.A = 0;
			var font = FontAssets.MouseText.Value;
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, new Vector2(x, y), new Color(0, 0, 0, 255), rotation, origin, baseScale);
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver2 + 0.01f)
			{
				var coords = new Vector2(x, y);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, coords, new Color(0, 0, 0, 255), rotation, origin, baseScale);
			}
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver2 + 0.01f)
			{
				var coords = new Vector2(x, y) + f.ToRotationVector2() * (brightness / 2f);
				ChatManager.DrawColorCodedString(Main.spriteBatch, font, text, coords, color * 0.8f, rotation, origin, baseScale);
			}
			for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver4 + 0.01f)
			{
				var coords = new Vector2(x, y) + (f + Main.GlobalTimeWrappedHourly).ToRotationVector2() * (brightnessProgress * 3f);
				ChatManager.DrawColorCodedString(Main.spriteBatch, font, text, coords, color * 0.2f, rotation, origin, baseScale);
			}
		}
		public static void DrawDedicatedTooltip(string text, int x, int y, Color color)
		{
			DrawDedicatedTooltip(text, x, y, 0f, Vector2.Zero, Vector2.One, color);
		}
		public static void DrawDedicatedTooltip(DrawableTooltipLine line)
		{
			DrawDedicatedTooltip(line.Text, line.X, line.Y, line.Rotation, line.Origin, line.BaseScale, line.OverrideColor.GetValueOrDefault(line.Color));
		}
	}
}