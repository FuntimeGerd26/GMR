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
	public class AcheronShuriken : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acheron Shuriken");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 1200;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
		}

		public override bool? CanDamage()
		{
			if (Projectile.ai[0] >= 120)
				return true;
			else
				return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Projectile.rotation += 0.045f;
			Projectile.velocity -= 0.05f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);

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