using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class NeonSaberSlash : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_972";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 3;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			Main.projFrames[Type] = 4;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 130;
			Projectile.height = 130;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.tileCollide = false;
			Projectile.scale = 1.25f;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		float VelMult = 1f;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.1f, 0.3f));
			Player player = Main.player[Projectile.owner];

			Projectile.localAI[0] += 1f;

			if (++Projectile.ai[2] > 20)
			{
				Projectile.scale -= 0.01f;
				VelMult *= 0.88f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			int size = 120;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding || Projectile.penetrate <= 1)
			{
				Projectile.alpha += 16;
				if (++Projectile.ai[2] < 20)
					Projectile.scale -= 0.01f;
				VelMult *= 0.80f;
			}
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			Projectile.frame = 0;

			Projectile.velocity.Normalize();
			Projectile.velocity *= 18f * VelMult;

			Projectile.localNPCHitCooldown = 30 - (int)(15 * (player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic)));
			if (Projectile.localNPCHitCooldown < 5)
				Projectile.localNPCHitCooldown = 10;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage /= 2;
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: new Color(255, 55, 55, 0), Scale: 1f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: new Color(255, 55, 55, 0), Scale: 1f);
					d.velocity *= 0.4f;
					d.velocity += Projectile.velocity * 0.5f;
					d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
					d.scale *= Projectile.scale * 0.6f;
					d.fadeIn = d.scale + 0.1f;
					d.noGravity = true;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipVertically;

			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(255, 20, 85) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swishFrame = swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			var swishColor = new Color(255, 70, 135, 20) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			var flareColor = new Color(205, 105, 185, 50) * opacity;
			var flareOrigin = flare.Size() / 2f;
			float flareOffset = (swish.Width / 2f + 12f);
			var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
			float flareDirectionDistance = 200f;
			float swishRotation = Projectile.rotation;
			for (int i = 0; i < 1; i++)
			{
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation + MathHelper.ToRadians(5f),
					swishOrigin,
					swishScale, spriteEffects, 0);

				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor * 0.3f,
					swishRotation + MathHelper.ToRadians(2f),
					swishOrigin,
					swishScale, spriteEffects, 0);

				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					new Color(255, 255, 255, 125) * 0.6f * 0.3f * opacity,
					swishRotation + MathHelper.ToRadians(-2f),
					swishOrigin,
					swishScale, spriteEffects, 0);

				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					new Color(255, 255, 255, 125) * 0.6f * opacity,
					swishRotation + MathHelper.ToRadians(-5f),
					swishOrigin,
					swishScale, spriteEffects, 0);

				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
					new Color(255, 255, 255, 125) * 0.6f * opacity,
					swishRotation,
					swishOrigin,
					swishScale, spriteEffects, 0);

				Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
					new Color(255, 255, 255, 125) * opacity * 0.5f, swishRotation, swishOrigin, swishScale * 0.8f, spriteEffects, 0);

				Main.EntitySpriteDraw(swish, swishPosition, swish.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame + 3),
					new Color(255, 255, 255, 125) * opacity * 0.5f, swishRotation, swishOrigin, swishScale * 0.65f, spriteEffects, 0);
			}

			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					var flarePosition = (swishRotation + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
					float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						flareColor * flareIntensity * 3f * 0.4f,
						0f,
						flareOrigin,
						new Vector2(swishScale * 0.35f, swishScale * 1.25f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						 flare,
						 swishPosition + flarePosition,
						 null,
						 flareColor * flareIntensity * 3f * 0.35f,
						 MathHelper.ToRadians(45f),
						 flareOrigin,
						 new Vector2(swishScale * 0.25f, swishScale * 1f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						 flare,
						 swishPosition + flarePosition,
						 null,
						 flareColor * flareIntensity * 3f * 0.35f,
						 MathHelper.ToRadians(-45f),
						 flareOrigin,
						 new Vector2(swishScale * 0.25f, swishScale * 1f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						flareColor * flareIntensity * 3f * 0.4f,
						MathHelper.PiOver2,
						flareOrigin,
						new Vector2(swishScale * 0.4f, swishScale * 1.5f) * flareIntensity, SpriteEffects.None, 0);
				}
			}
			return false;
		}
	}
}