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

namespace GMR.Items.Weapons.Magic.Staffs
{
	public class SpaceDoggoStaff : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(185, 185, 185),
			new Color(55, 75, 185),
			new Color(5, 75, 75),
		};

		public int flip;

		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 78;
			Item.rare = 4;
			Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 350);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 58;
			Item.crit = 10;
			Item.knockBack = 3.5f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.SpaceDoggoProj>();
			Item.shootSpeed = 6f;
			Item.mana = 16;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			flip = player.direction;
			const int max = 8;
			for (int i = 0; i < max; i++)
			{
				Projectile.NewProjectile(source, position, velocity.RotatedBy(2 * Math.PI / max * i), type,
					damage, knockback, player.whoAmI, 0, (player.Center - 350 * Vector2.UnitY - player.position).Length() * flip);
			}
			return false;
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
			float brightnessProgress = (Main.mouseTextColor - 210f) / (byte.MaxValue - 210f);
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