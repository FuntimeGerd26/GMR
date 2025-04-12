using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
    public class EyeBullet : ModProjectile
    {
        public override string Texture => "GMR/Projectiles/Melee/SpazEye";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 4;
            Projectile.AddElement(1);
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.35f, 0.6f, 0.2f));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (++Projectile.localAI[1] >= 15)
            {
                var target = Projectile.FindTargetWithinRange(400f);
                if (target != null)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 18f, 0.09f);
                }
            }
            else
            {
                AIType = ProjectileID.Bullet;

                Projectile.velocity.Normalize();
                Projectile.velocity *= 24f;
            }

            if (++Projectile.frameCounter > 10)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + origin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            int dustId = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 75, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 75, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
            Main.dust[dustId3].noGravity = true;
        }
    }
}