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
	public class AngelOfJudgementSpear : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/Swords/AngelOfJudgement";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.8f, 0.25f));

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			Projectile.velocity *= 1.025f;

			int size = 10;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), 2, 2);
			if (collding)
			{
				Projectile.Kill();
			}
		}

		public override void Kill(int timeleft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Explosion>(), (int)(Projectile.damage / 3), Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * -0.7f,
				Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color26 = new Color(255, 205, 85) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
					color27 * 0.9f * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
			}
			return false;
		}
	}
}