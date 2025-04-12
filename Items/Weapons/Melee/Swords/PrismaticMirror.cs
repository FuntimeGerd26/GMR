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

namespace GMR.Items.Weapons.Melee.Swords
{
	public class PrismaticMirror : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Shoots beams that when hitting an enemy, makes the player get healed by [c/44FF44:0.1%] of their heath\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.PrismMirrorProj>(30);
			Item.useTime /= 2;
			Item.SetWeaponValues(50, 2f, 8);
			Item.width = 92;
			Item.height = 92;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 7;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.buyPrice(silver: 315);
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
			Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
				Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value, position, frame,
				color27, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
			Vector2 position = new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f);
			Color color27 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 125), Color.Transparent, 0 / 3 + 0.3f);

			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color27, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
			recipe.AddIngredient(ItemID.CrystalShard, 20);
			recipe.AddIngredient(ItemID.SoulofLight, 16);
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}