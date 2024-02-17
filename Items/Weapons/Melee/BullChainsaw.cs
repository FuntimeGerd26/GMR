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

namespace GMR.Items.Weapons.Melee
{
	public class BullChainsaw : ModItem
    {
		public int Charge;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie Breaker");
			Tooltip.SetDefault("'Poison Charge'\nInflicts Poison to enemies\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.BullChainsaw>(28);
			Item.useTime /= 2;
			Item.SetWeaponValues(18, 5f, 4);
			Item.width = 78;
			Item.height = 78;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 270);
			Item.rare = 1;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0 && player.altFunctionUse != 2;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (Charge == 0)
					Charge++;
				else
					Charge = 0;

				SoundEngine.PlaySound(SoundID.Item23, player.position);

				Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);
				if (Charge == 1) // Cool Swing
				{
					CombatText.NewText(displayPoint, Color.Orange, "Tactical Break");
				}
				else if (Charge == 0)
				{
					CombatText.NewText(displayPoint, Color.Purple, "Break");
				}
			}
			else if (Charge == 0)
			{
				Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.BullChainsaw>(28);
				Item.useTime /= 2;
				Item.SetWeaponValues(42, 3.5f, 15);
			}
			else
            {
				Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.BullChainsawShoot>(54);
				Item.useTime /= 2;
				Item.SetWeaponValues(84, 5.5f, 4);
			}
		
			return true;
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			Texture2D texture = Charge == 1 ? ModContent.Request<Texture2D>($"GMR/Items/Weapons/Melee/BullChainsawCharge", AssetRequestMode.ImmediateLoad).Value :
				ModContent.Request<Texture2D>($"{Texture}", AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw(texture, position, frame,
				drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PlatinumBar, 16);
			recipe.AddIngredient(ItemID.Vertebrae, 12);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GoldBar, 16);
			recipe2.AddIngredient(ItemID.Vertebrae, 12);
			recipe2.AddIngredient(null, "UpgradeCrystal", 45);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.PlatinumBar, 16);
			recipe3.AddIngredient(ItemID.RottenChunk, 12);
			recipe3.AddIngredient(null, "UpgradeCrystal", 45);
			recipe3.AddTile(TileID.Anvils);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.GoldBar, 16);
			recipe4.AddIngredient(ItemID.RottenChunk, 12);
			recipe4.AddIngredient(null, "UpgradeCrystal", 45);
			recipe4.AddTile(TileID.Anvils);
			recipe4.Register();
		}
	}
}