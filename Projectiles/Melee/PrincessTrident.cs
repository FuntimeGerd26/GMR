using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
    public class PrincessTrident : ModProjectile
    {
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 40f;
		protected virtual float HoldoutRangeMax => 120f;

		// Play a custom swing sound
		//public static SoundStyle HeavySwing => GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f);
		
		public bool playedSound;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlantis Princess Trident");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
			Projectile.DamageType = DamageClass.Melee;
			Projectile.height = 40;
			Projectile.width = 40;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(125, 255, 255, 55);

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;

			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Apply proper rotation to the sprite.
			// This does nothing if the sprite isn't facing to the left
			if (Projectile.spriteDirection == -1)
			{
				// If sprite is facing left, rotate 135 degrees
				Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			}
			else
			{
				// If sprite is facing right, rotate 45 degrees
				Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			}

			if (Projectile.timeLeft < 2 && Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(25f)) * 2f, ModContent.ProjectileType<Projectiles.Melee.PrincessTridentEnergy>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.ToRadians(-25f)) * 2f, ModContent.ProjectileType<Projectiles.Melee.PrincessTridentEnergyFlip>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
			}

			if (!playedSound && Projectile.timeLeft < halfDuration)
			{
				playedSound = true;
				SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(1f), Projectile.Center);
			}
			return false; // Don't execute vanilla AI.
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.AtlantisTridentBuff>(), 1200);
		}
	}
}