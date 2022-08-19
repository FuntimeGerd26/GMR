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
	public class JackClaw : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Claw");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 86;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.light = 0.45f; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.spriteDirection = Projectile.direction;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;

			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

			Vector2 origin = sourceRectangle.Size() / 2;

			if (Projectile.spriteDirection == 1)
			{
				float offsetY = 0f;
				float offsetX = 0f;
				origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);
				origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
			}
			else if (Projectile.spriteDirection == -1)
			{
				float offsetY2 = 0f;
				float offsetX2 = 0f;
				origin.Y = (float)(Projectile.spriteDirection == -1 ? sourceRectangle.Height - offsetY2 : offsetY2);
				origin.X = (float)(Projectile.spriteDirection == -1 ? sourceRectangle.Width - offsetX2 : offsetX2);
			}

			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
	}
}