using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using GMR;
using GMR.Items.CustomStuff;

namespace GMR.Items.Weapons.Melee
{
	public class GerdSword : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(205, 205, 205),
			new Color(205, 105, 255),
			new Color(205, 205, 205),
			new Color(105, 205, 255),
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra del Solei");
			Tooltip.SetDefault($"Shoots homing projectiles\nInflicts 'Crystal Sickness', 'Devilish', and 'Sun Burnt' to enemies\n'There's no shadows without light, but what about light without shadows?'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.GerdSword>(18);
			Item.useTime /= 2;
			Item.SetWeaponValues(56, 4f, 14);
			Item.width = 110;
			Item.height = 110;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 8;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.sellPrice(silver: 315);
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AstraLux");
			recipe.AddIngredient(null, "LuxLunae");
			recipe.AddIngredient(null, "AmethystGreatSlasher");
			recipe.AddIngredient(null, "BossUpgradeCrystal", 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.MythrilAnvil);
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