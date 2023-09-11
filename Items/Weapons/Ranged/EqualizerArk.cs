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
	public class EqualizerArk : ModItem
	{
		private static readonly Color[] itemNameCycleColors = {
			new Color(0, 255, 0),
			new Color(0, 255, 225),
			new Color(125, 255, 0),
		};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ark's Equalizer");
			Tooltip.SetDefault($"'Balance though force'\nShoots homing bullets incredibly fast" +
				$"\n When piercing more than 5 enemies or after some time the projectiles will split into 3 smaller non-homing projectiles with random spread" +
				$"\n Has a chance to shoot a secondary projectile which will explode in a small area that deals 150% damage");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 68;
			Item.height = 30;
			Item.rare = 8;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 325);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 38;
			Item.crit = 10;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ArkBullet>();
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.ChlorophyteBullet || type == ProjectileID.BulletHighVelocity || type == ProjectileID.VenomBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet || type == ProjectileID.MoonlordBullet)
			{
					type = ModContent.ProjectileType<Projectiles.Ranged.ArkBullet>();
				if (Main.rand.NextBool(7))
                {
					Projectile.NewProjectile(Item.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ArkRocket>(), (int)(damage * 1.5), 3f, player.whoAmI);
				}
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
			recipe.AddIngredient(null, "Equalizer");
			recipe.AddIngredient(ItemID.SpectreBar, 14);
			recipe.AddIngredient(ItemID.Wire, 30);
			recipe.AddIngredient(ItemID.SoulofSight, 14);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 5);
			recipe.AddIngredient(null, "ScrapFragment", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "Equalizer");
			recipe2.AddIngredient(ItemID.SpectreBar, 4);
			recipe2.AddIngredient(ItemID.Wire, 30);
			recipe2.AddIngredient(ItemID.SoulofSight, 14);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal", 5);
			recipe2.AddIngredient(null, "ScrapFragment", 6);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}