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
    public class TheWretched : ModProjectile
	{
		// Define the range of the Spear Projectile. These are overrideable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 40f;
		protected virtual float HoldoutRangeMax => 160f;

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(-1);
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Spear);
			Projectile.ownerHitCheck = true;
			Projectile.height = 20;
			Projectile.width = 20;
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.ArmorPenetration = 15;
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

			for (int i = 0; i < 1; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 60, default(Color), 2f);
				dustId.noGravity = true;
			}

			return false; // Don't execute vanilla AI.
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 180);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2),
				target.Center.Y + Main.rand.Next(-target.height / 2, target.height / 2)), Vector2.Zero, ModContent.ProjectileType<Melee.SpecialSwords.BlasphemyHitSpark>(), Projectile.damage, 0f, Projectile.owner);

			for (int i = 0; i < 3; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1.5f);
				dustId.noGravity = true;
			}
		}
	}
}