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
	public class PrismaticBullet : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Bullet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 360;
			Projectile.penetrate = 20;
			Projectile.light = 0.75f; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 9;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			var target = Projectile.FindTargetWithinRange(1200f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 10f, 0.095f);
			}
			if (Projectile.timeLeft <= 30)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.95f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 0;
			Color disco = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * -0.5f, 30, disco, 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);

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
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, Projectile.scale / 2f, spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Color disco = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * -0.5f, 120, disco, 2f);
			Main.dust[dustId].noGravity = true;
		}
	}
}