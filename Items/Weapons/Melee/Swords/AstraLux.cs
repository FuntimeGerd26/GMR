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
	public class AstraLux : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Inflicts 'Devilish' to enemies\nHeals you by 0.5% of health when hitting an enemy\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.AstraLuxProj>(28);
			Item.useTime /= 2;
			Item.SetWeaponValues(75, 2.5f, 4);
			Item.width = 108;
			Item.height = 108;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 8;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.buyPrice(silver: 780);
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
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>($"{Texture}_Glow", AssetRequestMode.ImmediateLoad).Value, position, frame,
				Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			Texture2D texture = ModContent.Request<Texture2D>($"{Texture}_Glow", AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "DualDesertSword");
			recipe.AddIngredient(null, "DualSlashBlade");
			recipe.AddIngredient(null, "MaskedPlagueSword");
			recipe.AddIngredient(null, "Glaicey");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}