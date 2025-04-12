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

namespace GMR.Items.Weapons.Melee.Others
{
	public class PrismaticHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Hitting enemies inflicts Frostburn on them\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.PrismaticHammer>(42);
			Item.useTime /= 2;
			Item.SetWeaponValues(67, 4f, 14);
			Item.width = 110;
			Item.height = 110;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.buyPrice(silver: 850);
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
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
			recipe.AddIngredient(ItemID.TitaniumBar, 14);
			recipe.AddIngredient(ItemID.CrystalShard, 26);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.AdamantiteBar, 14);
			recipe2.AddIngredient(ItemID.CrystalShard, 26);
			recipe2.AddIngredient(ItemID.SoulofLight, 18);
			recipe2.AddRecipeGroup("GMR:AnyGem", 3);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}