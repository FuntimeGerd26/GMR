using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles
{
    public class TremorBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 360;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            for (int i = 0; i < 2; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 87, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1f);
                dustId.noGravity = true;
            }

            Projectile.scale += 0.1f;
            Projectile.alpha += 16;
            Projectile.rotation = 0f;

            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 87, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1f);
                dustId.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(255, 235, 125, 20);

            SpriteEffects spriteEffects = SpriteEffects.None;

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color26 * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color26 * 0.8f * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale * 0.6f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), color26 * 0.6f * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale * 0.4f, spriteEffects, 0);
            return false;
        }
    }

    public class TremorBurstH : TremorBurst
    {
        public override string Texture => "GMR/Projectiles/TremorBurst";

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
    }
}