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
using GMR;

namespace GMR.Items.Accessories
{
	public class HopefulFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 58;
			Item.value = Item.sellPrice(silver: 300);
			Item.rare = 6;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.04f;
			player.GetCritChance(DamageClass.Magic) += 4f;
			player.endurance = player.endurance * 0.9f + 0.05f; // Increased by 5% after decreasing the rest by 10% (Can be a very large or tiny amounts of decrease depending on the original amount)
			if (player.lifeRegen <= 1)
				player.lifeRegen = player.lifeRegen + (int)(player.lifeRegen * 0.4f);
		}

		float rotation;
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			rotation += 0.025f;
			Texture2D texture = ModContent.Request<Texture2D>($"GMR/Items/Accessories/HopefulFlower_Glow", AssetRequestMode.ImmediateLoad).Value;
			Texture2D texture2 = ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw(texture, position, null, new Color(255, 80, 180, 20), rotation, texture.Size() / 2f, scale * 1.1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture2, position, frame, new Color(185, 0, 100, 50), 0f, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, position, null, new Color(80, 195, 255, 30), -rotation, texture.Size() / 2f, scale * 0.75f, SpriteEffects.None, 0f);
		}

		float rotation2;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			rotation2 += 0.025f;
			Texture2D texture = ModContent.Request<Texture2D>($"GMR/Items/Accessories/HopefulFlower_Glow", AssetRequestMode.ImmediateLoad).Value;
			Texture2D texture2 = ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
			Vector2 position = new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture2.Height * 0.5f);

			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), new Color(255, 80, 180, 20), rotation2, texture.Size() * 0.5f, scale * 1.1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, position, new Rectangle(0, 0, texture2.Width, texture2.Height), new Color(185, 0, 100, 50), rotation, texture2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), new Color(80, 195, 255, 30), -rotation2, texture.Size() * 0.5f, scale * 0.75f, SpriteEffects.None, 0f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SoulofLight, 30);
			recipe.AddIngredient(ItemID.SoulofMight, 12);
			recipe.AddIngredient(ItemID.Cloud, 45);
			recipe.AddRecipeGroup("GMR:AnyGem", 6);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}