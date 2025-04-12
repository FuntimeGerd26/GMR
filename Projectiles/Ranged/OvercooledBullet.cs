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
	public class OvercooledBullet : ModProjectile
	{
		//public override string Texture => "GMR/Projectiles/Ranged/ShotgunBullet";
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overcooled Bullet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(1);
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(55, 75, 255, 55);

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.2f, 0.2f, 0.85f));

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 900);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
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
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, new Vector2(Projectile.scale, Projectile.scale * 0.4f), spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 80, Projectile.velocity.X * -0.2f,
				Projectile.velocity.Y * -0.2f, 30, Color.White, 1f);
			Main.dust[dustId].noGravity = false;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 80, Projectile.velocity.X * -0.2f,
				Projectile.velocity.Y * -0.2f, 30, Color.White, 1f);
			Main.dust[dustId3].noGravity = false;
		}
	}
}