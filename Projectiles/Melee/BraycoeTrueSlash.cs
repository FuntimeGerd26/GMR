using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class BraycoeTrueSlash : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash2";
		public float colorProgress;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blessed Hallow Slash");
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 160;
			Projectile.height = 160;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
			Projectile.scale = 2.1f;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
		}

		public override void AI()
		{
			if ((int)Projectile.ai[0] == 0)
			{
				Projectile.ai[0]++;
			}

			if (Projectile.alpha > 40)
			{
				if (Projectile.extraUpdates > 0)
				{
					Projectile.extraUpdates = 0;
				}
				if (Projectile.scale > 2f)
				{
					Projectile.scale -= 0.02f;
					if (Projectile.scale < 2f)
					{
						Projectile.scale = 2f;
					}
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180f);
			Projectile.velocity *= 0.99f;
			Projectile.alpha += 16;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage * 0.95);

			Player player = Main.player[Projectile.owner];
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Melee.CoolSwords.BraycoeExplosion>()] < 1)
				Projectile.NewProjectile(player.GetSource_FromThis(), Projectile.Center, Projectile.velocity,
					ModContent.ProjectileType<Projectiles.Melee.CoolSwords.BraycoeExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame,
						newColor: new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), Scale: 2f);
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
			Player player = Main.player[Projectile.owner];
			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipVertically;

			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, Projectile.GetAlpha(lightColor) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = Projectile.GetAlpha(lightColor) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			var flareOrigin = flare.Size() / 2f;
			float flareOffset = (swish.Height - 2f);
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
					swishRotation,
					swishOrigin,
					swishScale, spriteEffects, 0);
			}

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					var flarePosition = (swishRotation + MathHelper.ToRadians(180f) + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
					float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						0f,
						flareOrigin,
						new Vector2(swishScale * 0.35f, swishScale * 1f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						MathHelper.PiOver2,
						flareOrigin,
						new Vector2(swishScale * 0.4f, swishScale * 1.75f) * flareIntensity, SpriteEffects.None, 0);
				}
			}
			return false;
		}
	}
}