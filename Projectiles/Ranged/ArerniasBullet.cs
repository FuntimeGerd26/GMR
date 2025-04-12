using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
    public class ArerniasBullet : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_927";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(3);
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 6;
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
            Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 1f, 0.25f));

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
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + (Main.rand.NextFloat(-6f, 6f)), Projectile.Center.Y + (Main.rand.NextFloat(-6f, 6f))),
                Vector2.Zero, ModContent.ProjectileType<ArerniasBulletSpark>(), 0, 0f, Projectile.owner);

            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 1f);
            dustId.noGravity = true;
            Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 1.5f);
            dustId3.noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(25, 255, 25);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(25, 255, 25, 85);

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


    public class ArerniasBulletSpark : ModProjectile
    {
        public override string Texture => "GMR/Empty";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 9;
        }

        public override void AI()
        {
            Projectile.scale -= 0.05f;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        public float Scaling;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
            Texture2D texture2D13 = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
            Vector2 origin2 = texture2D13.Size() / 2f;
            var opacity = Projectile.Opacity;

            Color color26 = new Color(55, 255, 55, 85) * opacity;
            Scaling += -0.01f;

            for (int i = 0; i < 2; i++)
            {
                Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
                    color26, 0, origin2, Projectile.scale * (1f + Scaling * i) * 0.25f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
                    color26, MathHelper.ToRadians(90f), origin2, Projectile.scale * (1f + Scaling * i) * 0.25f, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}