using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Renderers;
using Terraria.GameContent.Events;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using GMR.Items.Weapons;
using GMR.Items.Weapons.Melee;

namespace GMR
{
	public partial class GerdPlayer : ModPlayer
	{
		#region Feature Stuff
		
		public int originalDefense;
		public bool FirstTick;
		public ushort itemCombo;
		public ushort itemUsage;
		float screenShakeStrenght;
		float screenShakeDesolve;

		#endregion

		#region Accessories

		public Item JackExpert;
		public Item DevPlush;
		public bool DevInmune;
		public Item Thunderblade;
		public bool BLBook;
		public bool MayDress;
		public Item Halu;
		public Item AluminiumCharm;
		public Item NajaCharm;
		public Item ChargedArm;
        public Item MaskedPlagueCloak;
		public bool ChaosSoul;
		public bool AwakeMayDress;
		public Item AncientCore;
		public Item AncientBullets;
		public Item GambleCrown;

		#endregion

		#region Armor Sets

		public Item InfraRedSet;
		public bool MagnumSet;
		public bool AmethystSet;
		public Item BoostSet;
		public bool MagmaSet;
		public bool BullSet;

		#endregion

		#region Buffs / Debuffs

		public bool Glimmering;
		public bool Thoughtful;
		public bool PainHeal;
		public bool PartialCrystal;
		public bool Devilish;
		public int DevilishDamage;
		public bool DamnSun;
		public bool YinEmpower;
		public bool YangEmpower;
		public bool IllusionOfLove;

		#endregion

		#region Enchantment Stuff

		public Item AlumArmor;
		public Item AlloybloodEnch;
		public bool AmalgamateEnch;
		public Item IcePrincessEnch;

		#endregion


		public void ShakeScreen(float strenght, float desolve = 0.95f)
		{
			if (!ClientConfig.Instance.EnabledScreenShake)
			{
				return;
			}

			screenShakeStrenght = strenght;
			screenShakeDesolve = Math.Clamp(desolve, 0, 0.9999f);
		}

		public override void ModifyScreenPosition()
		{
			if (screenShakeStrenght > 0.001f)
			{
				Main.screenPosition += screenShakeStrenght * Main.rand.NextVector2Unit();
				screenShakeStrenght *= screenShakeDesolve;
			}
		}

		public override void ResetEffects()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			Thunderblade = null;
			BLBook = false;
			MayDress = false;
			Halu = null;
			Glimmering = false;
			Thoughtful = false;
			PainHeal = false;
			InfraRedSet = null;
			PartialCrystal = false;
			MagnumSet = false;
			AmethystSet = false;
			BoostSet = null;
			AluminiumCharm = null;
			NajaCharm = null;
			ChargedArm = null;
			Devilish = false;
			DevilishDamage = 0;
			DamnSun = false;
			MaskedPlagueCloak = null;
			ChaosSoul = false;
			AwakeMayDress = false;
			AlumArmor = null;
			AlloybloodEnch = null;
			AncientCore = null;
			AncientBullets = null;
			MagmaSet = false;
			BullSet = false;
			GambleCrown = null;
			YinEmpower = false;
			YangEmpower = false;
			IcePrincessEnch = null;
			AmalgamateEnch = false;
			IllusionOfLove = false;
		}

		public override void UpdateDead()
		{
			JackExpert = null;
			DevPlush = null;
			DevInmune = false;
			Thunderblade = null;
			BLBook = false;
			MayDress = false;
			Halu = null;
			Glimmering = false;
			Thoughtful = false;
			PainHeal = false;
			InfraRedSet = null;
			PartialCrystal = false;
			MagnumSet = false;
			AmethystSet = false;
			BoostSet = null;
			AluminiumCharm = null;
			NajaCharm = null;
			ChargedArm = null;
			Devilish = false;
			DevilishDamage = 0;
			DamnSun = false;
			MaskedPlagueCloak = null;
			ChaosSoul = false;
			AwakeMayDress = false;
			AlumArmor = null;
			AlloybloodEnch = null;
			AncientCore = null;
			AncientBullets = null;
			MagmaSet = false;
			BullSet = false;
			GambleCrown = null;
			YinEmpower = false;
			YangEmpower = false;
			IcePrincessEnch = null;
			AmalgamateEnch = false;
			IllusionOfLove = false;
		}

		public void UpdateItemFields()
		{
			if (itemCombo > 0)
			{
				itemCombo--;
			}
			if (Player.itemTime > 0)
			{
				itemUsage++;
			}
			else
			{
				itemUsage = 0;
			}
		}

		public override void ModifyLuck(ref float luck)
		{
			if (BoostSet != null)
				luck += 0.5f;
			if (GambleCrown != null)
				luck += 0.2f;
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
				Player.lifeRegen += -15;
			}

			if (!FirstTick)
			{
				originalDefense = Player.statDefense;
				FirstTick = true;
			}

			if (Thoughtful)
				Player.statDefense += -30;

			if (PainHeal && Player.lifeRegen > 0)
				Player.lifeRegen = 0;

			if (Devilish)
			{
				DevilishDamage = Player.statLifeMax2 / 20;
				DamageOverTime(DevilishDamage, true);
			}
		}

		public override void PostHurt(Player.HurtInfo info)
		{
			// Don't apply extra immunity time to pvp damage (like vanilla)
			if (!info.PvP && DevInmune)
			{
				Player.AddImmuneTime(info.CooldownCounter, 120); // Cooldown ID, in this case, -1 (General) and Cooldown Time, 120 (2 seconds)
			}

			if (MagmaSet)
			{
				Vector2 velocity = Player.velocity;
				velocity.X *= 2f;
				float numberProjectiles = 5;
				float angle = Player.direction == -1 ? 45f : -45f;
				float rotation = MathHelper.ToRadians(15);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 speed = velocity.RotatedBy(MathHelper.ToRadians(angle));
					Vector2 perturbedSpeed = speed.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Magic.FallFireballFriendly>(), info.Damage, 1f, Main.myPlayer);
				}
			}

			if (AmethystSet)
			{
				// The code below makes you release crystal shards upon being hit
				Vector2 velocity; // There's no velocity to reference in here, so we make one
				velocity = new Vector2(0f, -12f); // Vector2(X axis, Y axis), -X is left, X is right, -Y is up, Y is down
				for (int i = 0; i < 8; i++) // 8 is the number of times this will shoot a projectile, change it to whatever you wish to
				{
					Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(75f)); // Rotating by random degrees for more randomness
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, perturbedSpeed, ProjectileID.CrystalShard, 20, 2f, Main.myPlayer);
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

			if (JackExpert != null)
			{
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Explotion>(), 100, 3f, Main.myPlayer);
			}

			if (!DevInmune || !AwakeMayDress)
			{
				return; //Is this for using usual frames
			}
		}

		public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
		{
			if (proj.friendly)
			{
				modifiers.IncomingDamageMultiplier *= 0.5f;
			}
		}

		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Player player = Main.LocalPlayer;

			void ClassOnlyEffects()
			{
				if (item.shoot == 0 && item.damage > 0 || item.shoot != 0 && item.damage > 0)
				{
					// Aluminium Armor is special like that
					if (AlumArmor != null)
					{
						if (Main.rand.NextBool(2))
							Projectile.NewProjectile(Player.GetSource_Accessory(AlumArmor), position, velocity * 2, ModContent.ProjectileType<Projectiles.Ranged.AluminiumShuriken>(), (int)(damage * 1.5), knockback, player.whoAmI);
					}

					#region Warrior
					if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Melee) == true) // If the damage class is melee, Check for the armor set being true 
					{
						if (BoostSet != null)
							Projectile.NewProjectile(source, position, velocity * 3f, ModContent.ProjectileType<Projectiles.BoostFlame>(), damage / 2, knockback / 2, Main.myPlayer);

						if (Main.rand.NextBool(4) && NajaCharm != null)
							Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);

						if (AlloybloodEnch != null)
							Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.AlloybloodDagger>(), damage, knockback, Main.myPlayer);
					}
					#endregion

					#region Ranger
					if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Ranged) == true)
					{
						if (MagnumSet != null)
						{
							velocity = velocity * 2f;
							base.Shoot(item, source, position, velocity, type, damage, knockback);
						}

						if (AluminiumCharm != null)
							Projectile.NewProjectile(Player.GetSource_Accessory(AluminiumCharm), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.AluminiumShot>(), damage / 2, knockback / 2, Main.myPlayer);

						if (Main.rand.NextBool(5) && ChargedArm != null && item.damage > 0)
						{
							SoundEngine.PlaySound(SoundID.Item36, Player.position);
							Projectile.NewProjectile(Player.GetSource_Accessory(ChargedArm), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.ChargedRocket>(), damage * 2, 4f, Main.myPlayer);
						}
						if (Main.rand.NextBool(4) && AncientCore != null && item.damage > 0 && item.useAmmo == AmmoID.Bullet)
						{
							SoundEngine.PlaySound(SoundID.Item41, Player.position);
							Projectile.NewProjectile(Player.GetSource_Accessory(AncientCore), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.JackExplosiveBullet>(), damage / 2, knockback / 2, Main.myPlayer);
						}
						if (Main.rand.NextBool(6) && AncientBullets != null && item.damage > 0 && item.useAmmo == AmmoID.Bullet)
						{
							SoundEngine.PlaySound(SoundID.Item41, Player.position);
							Projectile.NewProjectile(Player.GetSource_Accessory(AncientBullets), position, velocity / 2, ModContent.ProjectileType<Projectiles.Ranged.JackBullet>(), damage / 2, knockback / 2, Main.myPlayer);
						}
					}
					#endregion

					#region Mage
					if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Magic) == true)
					{
						if (Main.rand.NextBool(4) && NajaCharm != null)
							Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);

						if (MaskedPlagueCloak != null)
							Projectile.NewProjectile(Player.GetSource_Accessory(MaskedPlagueCloak), position, velocity / 2, ModContent.ProjectileType<Projectiles.Magic.PlagueCloackProj>(), damage, knockback, Main.myPlayer);
					}
					#endregion

					#region Summoner
					if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Summon) == true)
					{
						if (Main.rand.NextBool(4) && NajaCharm != null )
							Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, velocity / 2, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);

						if (MaskedPlagueCloak != null )
							Projectile.NewProjectile(Player.GetSource_Accessory(MaskedPlagueCloak), position, velocity / 2, ModContent.ProjectileType<Projectiles.Magic.PlagueCloackProj>(), damage, knockback, Main.myPlayer);
					}
					#endregion

					#region Volcano Charm Fireball
					if (player.HeldItem?.DamageType?.CountsAsClass(DamageClass.Ranged) != true && Main.rand.NextBool(4) && NajaCharm != null)
					{
						Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
						// Loop these functions 2 times.
						for (int i = 0; i < 2; i++)
						{
							position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
							position.Y -= 100 * i;
							Vector2 heading = target - position;

							if (heading.Y < 0f)
							{
								heading.Y *= -1f;
							}

							if (heading.Y < 20f)
							{
								heading.Y = 20f;
							}

							heading.Normalize();
							heading *= velocity.Length();
							Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, heading, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);
						}

						// Two times again
						for (int x = 0; x < 2; x++)
						{
							// Shoot under the player
							position = player.Center + new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
							position.Y += 100 * x;
							Vector2 heading = target - position;

							if (heading.Y > 0f)
							{
								heading.Y *= -1f;
							}

							if (heading.Y > 20f)
							{
								heading.Y = -20f;
							}

							heading.Normalize();
							heading *= velocity.Length();
							Projectile.NewProjectile(Player.GetSource_Accessory(NajaCharm), position, heading, ModContent.ProjectileType<Projectiles.NajaFireball>(), damage, knockback, Main.myPlayer);
						}
					}
                    #endregion

                    #region Ice Princess Shuriken
                    if (Main.rand.NextBool(3) && IcePrincessEnch != null)
					{
						Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
						float ceilingLimit = target.Y;
						if (ceilingLimit > player.Center.Y - 200f)
						{
							ceilingLimit = player.Center.Y - 200f;
						}

						// Loop these functions 2 times.
						for (int i = 0; i < 2; i++)
						{
							position = player.Center - new Vector2(Main.rand.NextFloat(350) * player.direction, 600f);
							position.Y -= 100 * i;
							Vector2 heading = target - position;

							if (heading.Y < 0f)
							{
								heading.Y *= -1f;
							}

							if (heading.Y < 20f)
							{
								heading.Y = 20f;
							}

							heading.Normalize();
							heading *= velocity.Length();
							Projectile.NewProjectile(Player.GetSource_Accessory(IcePrincessEnch), position, heading, ModContent.ProjectileType<Projectiles.Ranged.IceShuriken>(), damage / 2, knockback, Main.myPlayer);
						}
					}
					#endregion
				}
			}

            if (item.damage > 0 && DevPlush != null) //If the weapon's damage deals over 0 damage and Has the accessory on
			{
				if (Main.rand.NextBool(3)) // 1 in 3 (33%)
				{
					float numberProjectiles = 5; //5 shots
					float rotation = MathHelper.ToRadians(14);
					position += Vector2.Normalize(velocity) * 5f;
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f;
						Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, perturbedSpeed, type, damage, knockback, player.whoAmI);
					}

					Projectile.NewProjectile(Player.GetSource_Accessory(DevPlush), position, velocity, ModContent.ProjectileType<Projectiles.Ranged.JackClaw>(), damage, knockback, player.whoAmI);
				}
			}
			ClassOnlyEffects(); // Run the code for the class only effects
			if (BullSet)
				return base.Shoot(item, source, position, velocity, type, (int)(damage * 1.05), knockback);
			else
				return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
	}
}