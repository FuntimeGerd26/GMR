using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Summon
{
    public class ChaosAngelSoulHand0 : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Summon/ChaosAngelSoulHand";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Hand");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 2;
			Projectile.penetrate = -1;
            Projectile.alpha = 125;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 80;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
			if (modPlayer.ChaosSoul)
			{
				Projectile.timeLeft = 2;
			}
			AIType = -1;
			float maxDetectRadius = 640f; // The maximum radius at which a projectile can detect a target in pixels
			float projSpeed = 6f; // The speed at which the projectile moves towards the target

			// Trying to find NPC closest to the projectile
			NPC closestNPC = FindClosestNPC(maxDetectRadius);
			if (closestNPC != null)
			{
				// If found, change the velocity of the projectile and turn it in the direction of the target
				// Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
				Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			}
			else
            {
				Vector2 target = player.Center + 100 * Vector2.UnitX - 20 * Vector2.UnitY;
				Projectile.velocity = (target - Projectile.Center) * 0.085f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
			Projectile.spriteDirection = Projectile.direction;

			if (ModLoader.TryGetMod("FargowiltasSouls", out Mod mutantMod))
				mutantMod.Call("SummonCrit", true);
		}

		// Finding the closest NPC to attack within maxDetectDistance range
		// If not found then returns null
		public NPC FindClosestNPC(float maxDetectDistance)
		{
			NPC closestNPC = null;

			// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			// Loop through all NPCs(max always 200)
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC target = Main.npc[k];
				// Check if NPC able to be targeted. It means that NPC is
				// 1. active (alive)
				// 2. chaseable (e.g. not a cultist archer)
				// 3. max life bigger than 5 (e.g. not a critter)
				// 4. can take damage (e.g. moonlord core after all it's parts are downed)
				// 5. hostile (!friendly)
				// 6. not immortal (e.g. not a target dummy)
				if (target.CanBeChasedBy())
				{
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChaosBurnt>(), 60);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 120, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 120, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = Color.White;
			color26 = Projectile.GetAlpha(color26);

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(color26), Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}
	}
}