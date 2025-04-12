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
	public class SlabBullet : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 12;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 34;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(125, 185, 255);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.65f, 1f));

			if (Projectile.penetrate <= 1)
			{
				Projectile.alpha++;
				Projectile.velocity *= 0.95f;
				if (Projectile.alpha >= 255)
					Projectile.Kill();
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity.Normalize();
			Projectile.velocity *= 3.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * -0.5f, 30, new Color(125, 185, 255, 40), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(125, 185, 255);

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * 0.6f * Projectile.Opacity, Projectile.rotation, origin2, new Vector2(Projectile.scale * 1.5f, Projectile.scale * 0.25f), spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * -0.5f, 120, new Color(125, 185, 255, 40), 2f);
			Main.dust[dustId].noGravity = true;
		}
	}
}