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
	public class ArcaneBeam : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Melee/SwordSlash2";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.scale = 1.25f;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		float velMult = 1f;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.35f, 0.7f, 1f));

			if (Projectile.alpha > 40)
			{
				if (Projectile.extraUpdates > 0)
				{
					Projectile.extraUpdates = 0;
				}
				if (Projectile.scale > 1f)
				{
					Projectile.scale -= 0.025f;
					if (Projectile.scale < 1f)
					{
						Projectile.scale = 1f;
					}
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			velMult *= 0.97f;
			int size = 60;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), 2, 2);
			if (collding)
			{
				Projectile.alpha += 8;
				velMult *= 0.8f;
			}
			Projectile.alpha += 4;
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
			Projectile.velocity.Normalize();
			Projectile.velocity *= 20f * velMult;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dustId = Dust.NewDustDirect(target.position, target.width, target.height, 111, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 1.5f);
				dustId.noGravity = true;
			}
		}

		public override void Kill(int timeleft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 111, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 1.5f);
				dustId.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			float opacity = Projectile.Opacity;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == 1)
				spriteEffects = SpriteEffects.FlipVertically;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Player player = Main.player[Projectile.owner];
				Texture2D glow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
				Color color27 = Color.Lerp(new Color(50, 125, 255), Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				float scale = (Projectile.scale * 0.95f) - (float)Math.Cos(Projectile.ai[0]) / 5;
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
				Main.spriteBatch.Draw(glow, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity, Projectile.rotation, glow.Size() / 2, scale, spriteEffects, 0f);
			}

			var texture = TextureAssets.Projectile[Type].Value;
			var drawPosition = Projectile.Center;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			lightColor = Projectile.GetAlpha(lightColor);
			var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
			frame.Height -= 2;
			var origin = frame.Size() / 2f;
			int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
			for (int i = 0; i < trailLength; i +=2)
			{
				float progress = 1f - 1f / trailLength * i;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(50, 125, 255, 40) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale * 0.9f, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, new Color(50, 125, 255, 25) * opacity, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			var swishFrame = swish.Frame(verticalFrames: 1);
			var swishColor = new Color(50, 125, 255, 25) * opacity;
			var swishOrigin = swishFrame.Size() / 2;
			float swishScale = Projectile.scale;
			var swishPosition = Projectile.position + drawOffset;
			float swishRotation = Projectile.rotation;
			Main.EntitySpriteDraw(swish, swishPosition, swishFrame, swishColor, swishRotation, swishOrigin, swishScale, spriteEffects, 0);
			Main.EntitySpriteDraw(GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash2_Light", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				swishPosition, swishFrame, swishColor * 1.2f, swishRotation, swishOrigin, swishScale, spriteEffects, 0);

			return false;
		}
	}
}