using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;
using GMR.Projectiles.Ranged;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class EqualizerPrime : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(85, 85, 85),
			new Color(125, 125, 85),
			new Color(255, 255, 85),
		};

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
		}

		public override void SetDefaults()
		{
			Item.width = 70;
			Item.height = 32;
			Item.rare = 8;
			Item.useTime = 4;
			Item.useAnimation = 12;
			Item.reuseDelay = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 685);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 80;
			Item.crit = 15;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<EqualBulletPrime>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;

			type = ModContent.ProjectileType<Projectiles.Ranged.EqualBulletPrime>();
			SoundEngine.PlaySound(SoundID.Item41, player.position);
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

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "EqualizerLite");
			recipe.AddIngredient(null, "PrimeEnhancementModule");
			recipe.AddIngredient(null, "PrimePlating", 6);
			recipe.AddIngredient(ItemID.FragmentSolar, 15);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 4);
			recipe.AddIngredient(null, "SpecialUpgradeCrystal");
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}