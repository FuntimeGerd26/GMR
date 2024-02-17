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
	public class GungeonBullet : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
		}

		public override void SetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 14;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.75f, 0.15f, 0.15f));

			if (Projectile.penetrate == 1)
			{
				Projectile.alpha += 8;
				Projectile.velocity *= 0.8f;
				Projectile.damage = 0;
			}
			if (Projectile.alpha >= 255)
				Projectile.Kill();

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.YinEmpower>(), 240);

			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(35);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1.25f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Ranged.GungeonBulletShard>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * -0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId3.noGravity = true;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(255, 55, 55) * Projectile.Opacity;

			SpriteEffects spriteEffects = SpriteEffects.None;

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
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, new Vector2(Projectile.scale * 1.5f, Projectile.scale * 0.25f), spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * -0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId3.noGravity = true;
		}
	}
}