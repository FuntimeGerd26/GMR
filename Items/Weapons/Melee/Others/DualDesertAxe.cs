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
	public class DualDesertAxe : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(255, 125, 0),
			new Color(0, 255, 0),
			new Color(0, 0, 0),
		};

		public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"'It's coarse and rough and it gets everywhere'\nHitting enemies will cause explosions\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
			Item.AddElement(0);
			Item.AddElement(3);
		}

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.DualDesertAxe>(32);
            Item.useTime /= 4;
            Item.SetWeaponValues(50, 8f, 14);
            Item.width = 94;
            Item.height = 94;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = 6;
            Item.autoReuse = true;
            Item.reuseDelay = 4;
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
			recipe.AddIngredient(null, "DesertAxe");
			recipe.AddIngredient(ItemID.HallowedBar, 28);
			recipe.AddIngredient(ItemID.SoulofFright, 15);
			recipe.AddIngredient(ItemID.Emerald, 6);
			recipe.AddIngredient(ItemID.GoldBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DesertAxe");
			recipe2.AddIngredient(ItemID.HallowedBar, 28);
			recipe2.AddIngredient(ItemID.SoulofFright, 15);
			recipe2.AddIngredient(ItemID.Emerald, 6);
			recipe2.AddIngredient(ItemID.PlatinumBar, 15);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();


			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "DualDesertSword");
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("GMR/Items/Weapons/Melee/Others/DualDesertAxe_Glow", AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			int numColors = itemNameCycleColors.Length;

			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.Mod == "Terraria" && line2.Name == "ItemName")
				{
					float fade = (Main.GameUpdateCount % 60) / 60f;
					int index = (int)((Main.GameUpdateCount / 60) % numColors);
					int nextIndex = (index + 1) % numColors;

					line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
				}
			}
		}
	}
}