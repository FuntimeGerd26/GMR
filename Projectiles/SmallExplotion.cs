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
    public class SmallExplotion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_687";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Small Explosion");
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.LunarFlare];
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = 0;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;

                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                for (int i = 0; i < 20; i++)
                {
                    Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 60, default(Color), 1f);
                    dustId.noGravity = true;
                }
            }

            if (++Projectile.frameCounter > 2)
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
            target.immune[Projectile.owner] = 1;
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * -0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;
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

            Color color26 = Color.White;

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26;
                color27.A = 0;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, spriteEffects, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color26, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}