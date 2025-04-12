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
    public class ArkBomb : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_85";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.scale = 1.25f;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 60, default(Color), 1f);
                dustId.noGravity = true;
            }

            Projectile.scale += 0.025f;
            Projectile.alpha += 8;

            if (++Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame--;
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 600);
            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f), 60, default(Color), 1f);
            dustId.noGravity = true;
            Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f), 60, default(Color), 1.5f);
            dustId2.noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), new Color(155, 25, 25, 255) * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale * 1.05f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), new Color(200, 35, 35, 100) * 0.8f * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), new Color(255, 125, 125, 125) * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale * 0.6f, spriteEffects, 0);
            return false;
        }
    }
}