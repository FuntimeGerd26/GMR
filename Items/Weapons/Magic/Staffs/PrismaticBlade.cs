using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Magic.Staffs
{
	public class PrismaticBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prism Blade");
			Tooltip.SetDefault("Shoots homing swords\nHas a chance to throw swords from the sky and below the player");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 138;
			Item.height = 138;
			Item.rare = 8;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 265);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 48;
			Item.crit = 4;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.PrismaticBlade>();
			Item.shootSpeed = 32f;
			Item.mana = 4;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-40, 40);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
			// It gets real bad with so many projectiles
            if (Main.rand.NextBool(2))
            {
				// Loop these functions 2 times.
				for (int i = 0; i < 2; i++)
				{
					position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
					position.Y -= 100 * i;
					Vector2 heading = target - position;

					if (heading.Y < 0f)
					{
						heading.Y *= -1f;
					}

					if (heading.Y < 20f)
					{
						heading.Y = 20f;
					}

					heading.Normalize();
					heading *= velocity.Length();
					heading.Y += Main.rand.Next(-40, 41) * 0.02f;
					Projectile.NewProjectile(source, position, heading, Item.shoot, damage, knockback, player.whoAmI);
				}

				float ceilingLimit2 = target.Y;
				if (ceilingLimit2 < player.Center.Y + 200f)
				{
					ceilingLimit2 = player.Center.Y + 200f;
				}
				// Two times again
				for (int x = 0; x < 2; x++)
				{
					// Shoot under the player
					position = player.Center + new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
					position.Y += 100 * x;
					Vector2 heading = target - position;

					if (heading.Y > 0f)
					{
						heading.Y *= -1f;
					}

					if (heading.Y > 20f)
					{
						heading.Y = -20f;
					}

					heading.Normalize();
					heading *= velocity.Length();
					heading.Y -= Main.rand.Next(-40, 41) * 0.02f;
					Projectile.NewProjectile(source, position, heading, Item.shoot, damage, knockback, player.whoAmI);
				}
			}

			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position += Vector2.Normalize(velocity);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
				Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
				color27, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			Texture2D texture = ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
			Vector2 position = new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f);
			Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);

			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color27, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "PrismaticSwordDance");
			recipe.AddIngredient(null, "InfraRedBar", 30);
			recipe.AddIngredient(ItemID.Ectoplasm, 18);
			recipe.AddIngredient(ItemID.SoulofFlight, 25);
			recipe.AddIngredient(ItemID.Diamond, 5);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}