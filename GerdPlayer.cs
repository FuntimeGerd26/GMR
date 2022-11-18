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
		#region Feature Stuff
		public int originalDefense;
		public bool FirstTick;
		#endregion

		#region Accessories
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
		public Item AluminiumCharm;
		public Item NajaCharm;
        #endregion

		#region Armor Sets
        public bool InfraRedSet;
		public bool MagnumSet;
		public bool AmethystSet;
		public Item BoostSet;
        #endregion

		#region Debuffs
        public bool Glimmering;
		public bool Thoughtful;
		public bool PainHeal;
		public bool PartialCrystal;
        #endregion

        public override void OnEnterWorld(Player player)
		{
			//player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<Items.Accessories.BLBook>());
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
			Glimmering = false;
			Thoughtful = false;
			PainHeal = false;
			InfraRedSet = false;
			PartialCrystal = false;
			MagnumSet = false;
			AmethystSet = false;
			BoostSet = null;
			AluminiumCharm = null;
			NajaCharm = null;
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
			Glimmering = false;
			Thoughtful = false;
			PainHeal = false;
			InfraRedSet = false;
			PartialCrystal = false;
			MagnumSet = false;
			AmethystSet = false;
			BoostSet = null;
            AluminiumCharm = null;
			NajaCharm = null;
		}

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
		{
			Player player = Main.player[0];
			player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<Items.Weapons.Melee.GerdDagger>(), 1); //Should add the item to the player's inventory while generating a character
		}

		public override void UpdateBadLifeRegen()
		{
			void DamageOverTime(int badLifeRegen, bool affectLifeRegenCount = false)
			{
				if (Player.lifeRegen > 0)
					Player.lifeRegen = 0;

				if (affectLifeRegenCount && Player.lifeRegenCount > 0)
					Player.lifeRegen = 0;

				Player.lifeRegenTime = 0;
				Player.lifeRegen -= badLifeRegen;
			}

			if (Glimmering)
				DamageOverTime(5, true);

			if (PartialCrystal)
			{
				DamageOverTime(Player.statLifeMax2 / 40, true);
				Player.lifeRegen += -35;
			}

			if (!FirstTick)
			{
				originalDefense = Player.statDefense;
				FirstTick = true;
			}

			if (Thoughtful)
				Player.statDefense += -14;

			if (PainHeal)
				Player.lifeRegen = 0;
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (BLBook || MayDress)
			{
				damage = (int)(damage * 1.10); //Take 10% more damage
			}

			if (PainHeal)
            {
				damage = (int)(damage * 0.75); //Take 25% less damage
            }

			if (AmethystSet)
			{
				damage = (int)(damage * 0.80);
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
			if (!pvp && InfraRedSet)
			{
				Player.AddImmuneTime(cooldownCounter, 180);
			}
			if (!pvp && AmalgamInmune)
            {
				Player.AddImmuneTime(cooldownCounter, 240);
			}
		}

		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Player player = Main.player[0];

			if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Melee) == true) // If the damage class is melee, Check for the armor set being true 
			{
				if (BoostSet != null && item.damage > 0)
				{
					Projectile.NewProjectile(Player.GetSource_Accessory(BoostSet), position, velocity * 2, ModContent.ProjectileType<Projectiles.BoostFlame>(), damage / 2, knockback / 2, Main.myPlayer);
				}

				if (NajaCharm != null && item.damage > 0)
				{
					Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);
				}
				return true;
			}
			
			if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Ranged) == true)
			{
				if (AluminiumCharm != null && item.damage > 0)
				{
					Projectile.NewProjectile(Player.GetSource_Accessory(AluminiumCharm), position, velocity * 1.5f, ModContent.ProjectileType<Projectiles.Ranged.AluminiumShot>(), damage / 2, knockback / 2, Main.myPlayer);
				}
				return true;
			}

			if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Magic) == true)
			{
				if (NajaCharm != null && item.damage > 0)
				{
					Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);
				}
				return true;
			}

			if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Summon) == true)
			{
				if (NajaCharm != null && item.damage > 0)
				{
					Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);
				}
				return true;
			}

			if (item.damage > 0 && AmalgamPlush != null) //If projectile deals over 0 damage and Has the item on
			{
				if (Main.rand.NextBool(3)) // 1 in 3 (33.333333333333333333333333333333333333333333333333333333%) (Funny)
				{
					float numberProjectiles = 5; //5 shots
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
			else if (DevPlush != null)
			{
				if (item.damage > 0 && Main.rand.NextBool(5)) // 1 in 5 (20%)
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

			if (item.damage > 0 && (JackExpert != null) && (type == ProjectileID.FireArrow) && AmalgamPlush != null)
			{
				float numberProjectiles = 7;
				float rotation = MathHelper.ToRadians(7);
				position += Vector2.Normalize(velocity) * 5f;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1.5f;
					Projectile.NewProjectile(Player.GetSource_Accessory(AmalgamPlush), position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.JackShard>(), damage, knockback, player.whoAmI);
				}
				return false;
			}
			else if (item.damage > 0 && (JackExpert != null) && (type == ProjectileID.FireArrow))
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
			else if (item.damage > 0 && (JackExpert != null) && (type == ProjectileID.WoodenArrowFriendly))
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
