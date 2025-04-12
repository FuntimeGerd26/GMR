using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
	public class EtherealSaw : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 10;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			if (Projectile.extraUpdates > 0 || Projectile.extraUpdates < 0)
				Projectile.extraUpdates = 0;
			Projectile.scale = 0.5f;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.2f, 0.5f, 0.85f));

			Projectile.rotation += -Projectile.direction * 0.3f * Projectile.velocity.Length();

			if (Projectile.scale < 1f)
				Projectile.scale += 1.025f;

            Projectile.velocity.Normalize();
			Projectile.velocity *= 24f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.damage -= (int)(Projectile.damage * 0.05f);
			SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
			target.immune[Projectile.owner] = 1;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			float opacity = Projectile.Opacity / 2;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				Color color26 = new Color(55, 105, 255, 55) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Vector2 value4 = Projectile.oldPos[i];
				float num165 = Projectile.oldRot[i];
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 1.1f, spriteEffects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// So the projectile can bounce at most 5 times
			if (++Projectile.ai[2] >= 6)
			{
				return true;
			}
			else
			{
				// If the projectile hits the left or right side of the tile, reverse the X velocity
				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X * 1.05f;
				}

				// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y * 1.05f;
				}
			}

			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item110, Projectile.position);
		}
	}
}