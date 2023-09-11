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
	public class GerdSlash : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordHalfSpin";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Umbra Del Slash");
			ProjectileID.Sets.TrailCacheLength[Type] = 2;
			ProjectileID.Sets.TrailingMode[Type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.scale = 1.1f;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(40, 120, 200, 0);
		}

		public override void AI()
		{
			if ((int)Projectile.ai[0] == 0)
			{
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
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
			Projectile.velocity *= 0.99f;
			Projectile.rotation += 18f * 0.03f * Projectile.direction;
			int size = 140;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.88f;
			}
			Projectile.alpha += 8;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.damage = (int)(Projectile.damage * 0.95);
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 1200);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Devilish>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 600);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, new Color(Main.DiscoR, 125, 250, 0), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, -oldVelocity.X, 0.75f);
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -oldVelocity.Y, 0.75f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			float numberProjectiles = 7;
			float rotation = MathHelper.ToRadians(90f);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 0.5f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.GerdBlade>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Main.myPlayer);
			}

			if (Projectile.alpha < 200)
			{
				for (int i = 0; i < 30; i++)
				{
					var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, newColor: new Color(Main.DiscoR, 125, 250, 0), Scale: 2f);
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
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			float opacity = Projectile.Opacity;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i++)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(Main.DiscoR, 125, 250, 0) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale * 0.5f, SpriteEffects.None, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale * 0.75f, SpriteEffects.None, 0);

			var swish = TextureAssets.Projectile[Type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(Main.DiscoR, 125, 250, 0) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale * 1f;
			var swishPosition = Projectile.position + drawOffset;

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			var flareOrigin = flare.Size() / 2f;
			float flareOffset = (swish.Width / 2f - 4f);
			var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
			float flareDirectionDistance = 200f;
			for (int i = 0; i < 2; i++)
			{
				float swishRotation = Projectile.rotation + MathHelper.Pi * i;
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(
					swish,
					swishPosition,
					swishFrame,
					swishColor,
					swishRotation,
					swishOrigin,
					swishScale, SpriteEffects.None, 0);

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
						new Vector2(swishScale * 0.7f, swishScale * 2f) * flareIntensity, SpriteEffects.None, 0);

					Main.EntitySpriteDraw(
						flare,
						swishPosition + flarePosition,
						null,
						swishColor * flareIntensity * 3f * 0.4f,
						MathHelper.PiOver2,
						flareOrigin,
						new Vector2(swishScale * 0.8f, swishScale * 2.5f) * flareIntensity, SpriteEffects.None, 0);
				}
			}
			return false;
		}
	}
}