using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
    public class UltraBlueBullet : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_927";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 14;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        float ScaleX;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.8f, 1f));

            if (++Projectile.ai[0] >= 45)
                ScaleX += 0.025f;
            Projectile.alpha += 2;
            bool collding = Collision.SolidCollision(Projectile.position + new Vector2(4f, 4f), 2, 2);
            if (collding)
            {
                Projectile.alpha += 4;
                Projectile.velocity *= 0.8f;
            }
            if (Projectile.alpha >= 255)
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 1;
            Projectile.damage = (int)(Projectile.damage * 0.8);
            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 1f);
            dustId.noGravity = true;
            Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.4f, 60, default(Color), 1.5f);
            dustId3.noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(55, 185, 255);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(55, 185, 255);

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
                    new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * Projectile.Opacity, Projectile.rotation, origin2, new Vector2(Projectile.scale + ScaleX, Projectile.scale * 0.25f), spriteEffects, 0);
            }
            return false;
        }
    }
}