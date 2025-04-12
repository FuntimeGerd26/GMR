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
	public class ShotgunCore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shotgun Core");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.light = 0.15f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.JackExplosion>(), damageDone, Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item62.WithPitchOffset(-1f), Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = lightColor;
			color26 = Projectile.GetAlpha(color26);

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 2f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.Explosion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.Center);
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1.5f, Projectile.velocity.Y * 1.5f, 60, default(Color), 1f);
			dustId.noGravity = true;
			Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1.5f, Projectile.velocity.Y * 1.5f, 60, default(Color), 1.5f);
			dustId2.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 1.5f, Projectile.velocity.Y * 1.5f, 60, default(Color), 2.5f);
			dustId3.noGravity = true;
		}
	}
}