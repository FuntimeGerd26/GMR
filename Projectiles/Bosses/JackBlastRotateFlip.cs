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
	public class JackBlastRotateFlip : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronSaw";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acheron Saw");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 300;
			Projectile.alpha = 25;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override bool? CanDamage()
		{
			if (Projectile.timeLeft < 200)
				return true;
			else
				return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 85, 5);

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Projectile.rotation += -0.045f;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-2f));
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