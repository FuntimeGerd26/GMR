using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
    public class EqualBulletPrime : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_780";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(2);
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

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(225, 205, 35);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.6f, 0.2f));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (++Projectile.localAI[1] >= 15)
            {
                var target = Projectile.FindTargetWithLineOfSight(800f);
                if (target != -1)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.npc[target].Center - Projectile.Center) * 32f, 0.06f);
                }
            }
            else
            {
                AIType = ProjectileID.Bullet;

                Projectile.velocity.Normalize();
                Projectile.velocity *= 32f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 2f);
            dustId.noGravity = true;
            Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 60, default(Color), 2f);
            dustId3.noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(225, 205, 35);

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
                float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * Projectile.Opacity, num165, origin2, Projectile.scale / 2f, spriteEffects, 0);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.SmallExplosion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

            int dustId = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 64, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 64, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
            Main.dust[dustId3].noGravity = true;
        }
    }
}