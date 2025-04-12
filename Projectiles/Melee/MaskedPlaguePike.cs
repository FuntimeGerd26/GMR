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
    public class MaskedPlaguePike : ModProjectile
    {

		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 40f;
		protected virtual float HoldoutRangeMax => 156f;

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(2);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.ownerHitCheck = true;
			Projectile.height = 20;
			Projectile.width = 20;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			int duration = player.itemAnimationMax;

			player.heldProj = Projectile.whoAmI;

			if (Projectile.timeLeft > duration)
			{
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity);

			float halfDuration = duration * 0.5f;
			float progress;

			if (Projectile.timeLeft < halfDuration)
			{
				progress = Projectile.timeLeft / halfDuration;

			}
			else
			{
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);

			// Apply proper rotation to the sprite.
			// This does nothing if the projectile's sprite isn't facing to the left
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			}
			else
			{
				Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			}

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.75f,
				Projectile.velocity.Y * 0.75f, 60, default(Color), 1.5f);
			Main.dust[dustId].noGravity = true;

			return false; // Don't execute vanilla AI.
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.MaskedPlagueStrike>(), 180);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.75f, 60, default(Color), 1f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.75f,
				Projectile.velocity.Y * 0.5f, 60, default(Color), 1.5f);
			Main.dust[dustId3].noGravity = true;
		}
	}
}