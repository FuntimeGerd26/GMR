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
	public class JackBlastFlip : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/JackBlastBad";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Blast");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 1200;
			Projectile.alpha = 55;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			AIType = ProjectileID.Bullet;
		}

		public override bool? CanDamage()
		{
			if (Projectile.ai[0] >= 120)
				return true;
			else
				return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 85, 5);

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Projectile.velocity += 0.05f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);

			if (++Projectile.ai[0] == 120)
			{
				Projectile.velocity = -Projectile.velocity;

				int dustType = 60;
				for (int i = 0; i < 40; i++)
				{
					Vector2 velocityDust = Projectile.velocity + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f));
					Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocityDust, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

					dust.noLight = false;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
	}
}