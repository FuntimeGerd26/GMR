using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class DesertWave : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash2";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Desert Wave");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.scale = 1.1f;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(200, 120, 40, 0);

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
				if (Projectile.scale > 1f)
				{
					Projectile.scale -= 0.02f;
					if (Projectile.scale < 1f)
					{
						Projectile.scale = 1f;
					}
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 0.99f;
			int size = 140;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.8f;
			}
			Projectile.alpha += 6;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 300);
			player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 300);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == 1)
				spriteEffects = SpriteEffects.FlipVertically;
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(200, 120, 40, 0) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(200, 120, 40, 0) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;
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

			// Trail funny
			float modifier = Projectile.localAI[1] * MathHelper.Pi / 45;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Player player = Main.player[Projectile.owner];
				Color color27 = Color.Lerp(new Color(200, 120, 40, 0), Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
				scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = Math.Max((int)i - 1, 0);
				Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], (1 - i % 1));
				float smoothtrail = i % 1 * (float)Math.PI / 3.45f;
				bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
				if (withinangle && player.direction == 1)
					smoothtrail *= -1;
				else if (!withinangle && player.direction == -1)
					smoothtrail *= -1;
				center += Projectile.Size / 2;

				Vector2 offset = (Projectile.Size / 12).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * (-Projectile.direction));
				Main.spriteBatch.Draw(swish, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity, Projectile.rotation, swish.Size() / 2, scale, spriteEffects, 0f);
			}
			return false;
		}
	}
}