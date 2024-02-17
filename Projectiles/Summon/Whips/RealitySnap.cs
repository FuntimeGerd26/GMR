using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Summon.Whips
{
	public class RealitySnap : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_927";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 20;
			Projectile.light = 0.75f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(40, 120, 200);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0f, 0.45f, 1f));

			var target = Projectile.FindTargetWithinRange(350f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 8f, 0.09f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.timeLeft -= 120;
			Projectile.damage /= 2;
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Thoughtful>(), 600);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 40;
			Projectile.position.X -= (float)(Projectile.width / 2);
			Projectile.position.Y -= (float)(Projectile.height / 2);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(40, 120, 200);

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
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, new Vector2(Projectile.scale, Projectile.scale * 0.4f), spriteEffects, 0);
			}
			return false;
		}
	}
}