using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;
using GMR.Items.Accessories;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
	public class MemerEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases magic damage by 25%\nStar Veil, Shiny Stone, Honeycomb, Mana Flower and Celestial Cuffs effects" +
				"\nIncreases falling speed and melee size\nIncreases defense by 10\n'You can make rules, but who's gonna follow them?'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.25f;
			player.honeyCombItem = Item;
			if (player.statLife <= player.statLifeMax2 / 2)
				player.AddBuff(BuffID.IceBarrier, 5, true);
			player.manaFlower = true;
			player.manaMagnet = true;
			player.maxFallSpeed += 1.5f;
			player.meleeScaleGlove = true;
			player.shinyStone = true;
			player.starCloakItem = Item;
			player.starCloakItem_starVeilOverrideItem = Item;
			player.statDefense += 10;

			player.GetJumpState<MemerEnchJump>().Enable();
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "SpazHatMask");
			recipe.AddIngredient(null, "SpazDress");
			recipe.AddIngredient(null, "SpazThighs");
			recipe.AddIngredient(null, "LostTime");
			recipe.AddIngredient(null, "RefinedSpazSniper");
			recipe.AddIngredient(null, "SpazChargeBow");
			recipe.AddIngredient(null, "CoreEjectShotgun");
			recipe.AddIngredient(null, "ScarletLauncher");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}

	public class MemerEnchJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new Before(SandstormInABottle);

		public override float GetDurationMultiplier(Player player)
		{
			// Use this hook to set the duration of the extra jump
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			return 2.85f;
		}

		public override void UpdateHorizontalSpeeds(Player player)
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.runAcceleration *= 1.5f;
			player.maxRunSpeed *= 3f;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			// Use this hook to trigger effects that should appear at the start of the extra jump
			// This example mimicks the logic for spawning the puff of smoke from the Cloud in a Bottle
			int offsetY = player.height;
			if (player.gravDir == -1f)
				offsetY = 0;

			offsetY -= 16;

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(player.position + new Vector2(-34f, offsetY), 102, 32, 163, -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 100, Color.White, 1.5f);
				dust.velocity = dust.velocity * 0.5f - player.velocity * new Vector2(0.1f, 0.3f);
			}

			offsetY = player.height - 6;
			if (player.gravDir == -1f)
				offsetY = 6;
			for (int i = 0; i < 10; i++)
			{
				Dust dust2 = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y + offsetY), player.width, 12, 163, player.velocity.X * 0.2f, player.velocity.Y * 0.05f, newColor: Color.White);
				Dust dust3 = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y + offsetY), player.width, 12, 163, -player.velocity.X * 0.2f, player.velocity.Y * 0.05f, newColor: Color.White);
			}

			Vector2 sparkSpeed = new Vector2(0f, -6f);
			for (int i = 0; i < 6; i++)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, sparkSpeed.RotatedByRandom(MathHelper.ToRadians(360f)), ModContent.ProjectileType<Projectiles.MemerEnchSpark>(), 60, 2f, Main.myPlayer);
			}
		}

		public override void ShowVisuals(Player player)
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
			int offsetY = player.height - 6;
			if (player.gravDir == -1f)
				offsetY = 6;

			Vector2 spawnPos = new Vector2(player.position.X, player.position.Y + offsetY);

			for (int i = 0; i < 1; i++)
			{
				SpawnBlizzardDust(player, spawnPos, 0.1f, i == 0 ? -0.07f : -0.13f);
			}

			for (int i = 0; i < 2; i++)
			{
				SpawnBlizzardDust(player, spawnPos, 0.6f, 0.8f);
			}

			for (int i = 0; i < 2; i++)
			{
				SpawnBlizzardDust(player, spawnPos, 0.6f, -0.8f);
			}
		}

		private static void SpawnBlizzardDust(Player player, Vector2 spawnPos, float dustVelocityMultiplier, float playerVelocityMultiplier)
		{
			Dust dust = Dust.NewDustDirect(spawnPos, player.width, 12, 163, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, newColor: Color.White);
			dust.fadeIn = 1.5f;
			dust.velocity *= dustVelocityMultiplier;
			dust.velocity += player.velocity * playerVelocityMultiplier;
			dust.noGravity = true;
			dust.noLight = true;
		}
	}
}