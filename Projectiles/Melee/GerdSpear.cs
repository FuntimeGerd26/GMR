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
    public class GerdSpear : ModProjectile
    {
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 80f;
		protected virtual float HoldoutRangeMax => 180f;

		// Play a custom swing sound
		public bool playedSound;
		public bool shootProj;
		public bool returning;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stella Salutis");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
			Projectile.DamageType = DamageClass.Melee;
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.usesLocalNPCImmunity = true;
		}

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

			if (Projectile.timeLeft >= halfDuration + 0.1f && !shootProj && Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 18f, ModContent.ProjectileType<Projectiles.Melee.GerdSpearBlade>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
				shootProj = true;
			}

			if (!playedSound && Projectile.timeLeft < halfDuration)
			{
				playedSound = true;
				SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.5f, 0f, 0.2f).WithPitchOffset(0.75f), Projectile.Center);
			}
			return false; // Don't execute vanilla AI.
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.CuttingEdge>(), 240);
		}
	}
}