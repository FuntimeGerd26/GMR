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
	public class JackClawProj : ModProjectile
	{
		public override string Texture => "GMR/NPCs/Bosses/Jack/AcheronArmClaw";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Claw");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 1200;
			Projectile.light = 0.45f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(-90f);
			Projectile.velocity += 0.25f * Vector2.Normalize(Projectile.velocity);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int y3 = frameHeight * Projectile.frame;
			Rectangle rectangle = new Rectangle(0, y3, texture.Width, frameHeight);
			Vector2 origin = rectangle.Size() / 2f;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Color drawColor = new Color(255, 55, 85, 5);

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				rectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

			return false;
		}
	}
}