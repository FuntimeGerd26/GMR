using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using GMR.Items.Weapons.Melee;

namespace GMR
{
	public partial class GerdPlayer : ModPlayer
	{
		public Item JackExpert;
		public Item DevPlush;
		public bool DevInmune;
		public Item OverlordBlade;

		public override void OnEnterWorld(Player player)
		{
			Main.NewText($"[i:{ModContent.ItemType<Items.Vanity.GerdHead>()}] You are playing a beta version or an early release, Please remember that", Color.Cyan);
		}

		public override void ResetEffects()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			OverlordBlade = null;
		}

		public override void UpdateDead()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			OverlordBlade = null;
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (OverlordBlade != null)
			{
				damage = (int)(damage * 1.45);

				float numberProjectiles = 8;
				float rotation = MathHelper.ToRadians(Main.rand.NextFloat(180f, -180f));
				Vector2 velocity;
				velocity = new Vector2(0f, -40f);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed + Player.velocity, ModContent.ProjectileType<Projectiles.OverlordOrbHurt>(), 100, 3f, Main.myPlayer);
				}
			}
			return true;
		}
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (!DevInmune)
			{
				return;
			}

			// Don't apply extra immunity time to pvp damage (like vanilla)
			if (!pvp && DevInmune)
			{
				Player.AddImmuneTime(cooldownCounter, 120);
			}
		}

		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Player player = Main.player[0];

			if (DevPlush != null)
			{
				if (Main.rand.NextBool(5))
				{
					float numberProjectiles = 3; //3 shots
					float rotation = MathHelper.ToRadians(22);
					position += Vector2.Normalize(velocity) * 5f;
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
						Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, perturbedSpeed, type, (damage / 2) + (damage / 4), knockback, player.whoAmI);
						Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.JackClaw>(), damage, knockback, player.whoAmI);
					}
					return true;
				}
				else
				{

				}
			}

			if ((JackExpert != null) && (type == ProjectileID.FireArrow))
			{
				float numberProjectiles = 3; //3 shots
				float rotation = MathHelper.ToRadians(5);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .5f;
					Projectile.NewProjectile(Player.GetSource_Accessory(JackExpert), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShot>(), damage / 2, knockback, player.whoAmI);
				}
				return false;
			}
			else if ((JackExpert != null) && (type == ProjectileID.WoodenArrowFriendly))
			{
				float numberProjectiles = 3; //3 shots
				float rotation = MathHelper.ToRadians(25);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .5f;
					Projectile.NewProjectile(Player.GetSource_Accessory(JackExpert), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShot>(), damage, knockback, player.whoAmI);
				}
				return false;
			}
			else
			{

			}

			return true;
		}
	}
}
