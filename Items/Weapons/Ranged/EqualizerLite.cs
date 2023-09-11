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

namespace GMR.Items.Weapons.Ranged
{
	public class EqualizerLite : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(0, 0, 255),
			new Color(0, 125, 255),
		};

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"'I think i forgot something...'\n Projectiles bounce on enemies and tiles if they haven't hit an enemy yet\n When hitting an enemy they will split into 3 other non-bouncing projectiles");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 30;
			Item.rare = 4;
			Item.useTime = 5;
			Item.useAnimation = 15;
			Item.reuseDelay = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 165);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 7;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.EqualBulletLite>();
				SoundEngine.PlaySound(SoundID.Item41, player.position);
			}
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
			recipe.AddIngredient(null, "YinGun");
			recipe.AddIngredient(null, "YangGun");
			recipe.AddIngredient(null, "AlloyBox");
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.CobaltBar, 14);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "YinGun");
			recipe2.AddIngredient(null, "YangGun");
			recipe2.AddIngredient(null, "AlloyBox");
			recipe2.AddIngredient(ItemID.SoulofNight, 12);
			recipe2.AddIngredient(ItemID.PalladiumBar, 14);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}