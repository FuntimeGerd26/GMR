using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class BraycoeSwordBeam : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/BraycoeSword";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Wood Beam");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 1.1f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			Projectile.velocity *= 0.99f;
			if (Projectile.alpha > 40 && Projectile.scale > 1f)
            {
				if (Projectile.scale < 1f)
					Projectile.scale = 1f;
				else
					Projectile.scale += -0.01f;
			}
			if (Projectile.width > 90 || Projectile.height > 90)
			{
				Projectile.width = 90;
				Projectile.height = 90;
			}

            Projectile.alpha += 16;
			if (Projectile.alpha > 255)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			var Opacity = Projectile.Opacity;

			Color color26 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * Opacity, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(color26) * Opacity, Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}
	}
}