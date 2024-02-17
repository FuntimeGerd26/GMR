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
        public override string Texture => "Terraria/Images/Projectile_687";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ark Explosion");
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.LunarFlare];
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 1;
            Projectile.scale = 2f;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

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
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = new Color(255, 25, 125, 25);
            return color;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2,
                Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}