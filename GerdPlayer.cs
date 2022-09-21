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
		public Item AmalgamPlush;
		public bool AmalgamInmune;
		public Item OverlordBlade;
		public Item OverlordBoots;
		public Item Thunderblade;
		public bool BLBook;
		public bool MayDress;
		public Item Halu;

		public override void OnEnterWorld(Player player)
		{
			//player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Accessories.BLBook>());
			//Main.NewText($"[i:{ModContent.ItemType<Items.Vanity.GerdHead>()}] Welcome to the Beta of Gerd's Lab", Color.Cyan); //To-do: Remove before release
		}

		public override void ResetEffects()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			AmalgamPlush = null;
			AmalgamInmune = false;
			OverlordBlade = null;
			OverlordBoots = null;
			Thunderblade = null;
			BLBook = false;
			MayDress = false;
			Halu = null;
		}

		public override void UpdateDead()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			AmalgamPlush = null;
			AmalgamInmune = false;
			OverlordBlade = null;
			OverlordBoots = null;
			Thunderblade = null;
			BLBook = false;
			MayDress = false;
			Halu = null;
		}

		public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
		{
			Player player = Main.player[0];
			player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<Items.Accessories.MayDress>(), 1); //Should hopefully summon the accessory
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (BLBook || MayDress)
			{
				damage = (int)(damage * 1.10); //Take 10% more damage
			}

			if (OverlordBlade != null)
			{
				if (OverlordBoots != null)
				{	
						damage = (int)(damage * 0.90);
				}
				float numberProjectiles = 8; //number of projectiles shot
				float rotation = MathHelper.ToRadians(Main.rand.NextFloat(180f, -180f)); //rotation, MathHelper.ToRadians(Main.rand.NextFloat(MAXf, MIN or -MAXf))

				Vector2 velocity; //Because there's none in here
				velocity = new Vector2(0f, -40f); // Vector2(Xf, Yf)
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f; //No idea, just don't touch any value, the last "* 1f" is ok to modify tho
					if(OverlordBoots != null)
					{
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed + Player.velocity, ModContent.ProjectileType<Projectiles.OverlordOrbHurt>(), 200, 4f, Main.myPlayer);
					}
					else
                    {
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed + Player.velocity, ModContent.ProjectileType<Projectiles.OverlordOrbHurt>(), 100, 2f, Main.myPlayer);
					}
				}
			}
			if (Thunderblade != null)
            {
				float numberProjectiles = 4;
				float rotation = MathHelper.ToRadians(Main.rand.NextFloat(25f, -25f));
				Vector2 velocity;
				velocity = new Vector2(0f, 80f);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center - 600 * Vector2.UnitY, perturbedSpeed + Player.velocity / 2, ModContent.ProjectileType<Projectiles.Ranged.Lightning>(), 50, 3f, Main.myPlayer);

				}
			}
			return true; //True for taking damage, maybe idk
		}
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (!DevInmune || !AmalgamInmune)
			{
				return; //Is this for using usual frames if not having these?
			}

			// Don't apply extra immunity time to pvp damage (like vanilla)
			if (!pvp && DevInmune)
			{
				Player.AddImmuneTime(cooldownCounter, 120);
			}
			if (!pvp && AmalgamInmune)
            {
				Player.AddImmuneTime(cooldownCounter, 240);
			}
		}

		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Player player = Main.player[0];

			if (DevPlush != null)
			{
				if (Main.rand.NextBool(5)) // 1 in 5 (20%)
				{
					float numberProjectiles = 3; //3 shots
					float rotation = MathHelper.ToRadians(22);
					position += Vector2.Normalize(velocity) * 5f;
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f;
						Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, perturbedSpeed, type, (damage / 2) + (damage / 4), knockback, player.whoAmI);
						Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.JackClaw>(), damage, knockback, player.whoAmI);
					}
					return true; //Shoot vanilla projectiles
				}
				else
				{
					return true; //DO NOT CHANGE TO FALSE, This makes weapon shoot projectiles without this accessory on
				}
			}

			if (AmalgamPlush != null)
			{
				if (Main.rand.NextBool(3)) // 1 in 3 (33.333333333333333333333333333333333333333333333333333333%) (Funny)
				{
					float numberProjectiles = 7; //7 shots
					float rotation = MathHelper.ToRadians(14);
					position += Vector2.Normalize(velocity) * 5f;
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f;
						Projectile.NewProjectile(Player.GetSource_Accessory(AmalgamPlush), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
						Projectile.NewProjectile(Player.GetSource_Accessory(AmalgamPlush), position, velocity * 2, ModContent.ProjectileType<Projectiles.Ranged.JackClaw>(), damage, knockback, player.whoAmI);
					}
					return true;
				}
				else if ((type == ProjectileID.FireArrow) || (type == ProjectileID.WoodenArrowFriendly))
				{
					float numberProjectiles = 3;
					float rotation = MathHelper.ToRadians(5);
					position += Vector2.Normalize(velocity) * 5f;
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f;
						Projectile.NewProjectile(Player.GetSource_Accessory(JackExpert), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShard>(), damage * 2, knockback, player.whoAmI);
					}
					return false;
				}
				else
				{
					return true;
				}
			}

			if ((JackExpert != null) && (type == ProjectileID.FireArrow))
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(5);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(Player.GetSource_Accessory(JackExpert), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShard>(), damage / 2, knockback, player.whoAmI);
				}
				return false;
			}
			else if ((JackExpert != null) && (type == ProjectileID.WoodenArrowFriendly))
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(25);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(Player.GetSource_Accessory(JackExpert), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShard>(), damage, knockback, player.whoAmI);
				}
				return false; //Do not shoot vanilla projectile, Since it's getting replaced by this
			}
			else
			{

			}

			return true;
		}
	}
}
