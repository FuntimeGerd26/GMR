using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Bosses
{
	public class AcheronOrb : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/GlowSprite";

		public Vector2? ForcedTargetPosition { get; set; }

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 900;
			Projectile.light = 0.75f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 0.25f;
		}

		public override void AI()
		{
			if (--Projectile.timeLeft < 880)
			{
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Rectangle hitbox = Projectile.Hitbox;

					int maxDistance = 1000;

					Rectangle areaCheck;

					Player player = Main.player[i];

					if (ForcedTargetPosition is Vector2 target)
						areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else if (player.active && !player.dead && !player.ghost)
						areaCheck = new Rectangle((int)player.position.X - maxDistance, (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else
						continue;  // Not a valid player


					Vector2 targetPlayer = player.Center;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(targetPlayer - Projectile.Center) * 8f, 0.09f);
				}
			}
			else
				Projectile.velocity = Projectile.velocity;

			if (Projectile.timeLeft < 120)
			{
				Projectile.alpha += 16;
				Projectile.velocity *= 0.95f;
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			Projectile.rotation += 18f * 0.03f;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var opacity = Projectile.Opacity;

			Color color26 = new Color(255, 55, 55, 0) * opacity;

			SpriteEffects spriteEffects = SpriteEffects.None;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = Color.Lerp(color26, Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			}
			return false;
		}
	}
}