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

namespace GMR.Items.Weapons.Magic
{
	public class PrismaticSwordDance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Sword Staff");
			Tooltip.SetDefault("Shoots homing swords");
			Item.staff[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 58;
			Item.rare = 5;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 75);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item43;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 28;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.PlagueBolt>();
			Item.shootSpeed = 6f;
			Item.mana = 4;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			type = ModContent.ProjectileType<Projectiles.Magic.PrismaticSwordDance>();
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
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
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddRecipeGroup("Wood", 25);
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddRecipeGroup("GMR:AnyGem", 10);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}