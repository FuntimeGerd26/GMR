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
    public class BoostFlame : ModProjectile
    {
        public override string Texture => "GMR/Projectiles/SpearTrail";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boost Flame");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 20;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 1.0125f;

            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * 0.4f, 120, default(Color), 2f);
            Main.dust[dustId].noGravity = false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 600);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.SmallExplotion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity;

            Color color26 = new Color(255, 205, 55, 55);

            for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
            {
                Color color27 = Color.Lerp(color26, Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
                scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                int max0 = (int)i - 1;
                if (max0 < 0)
                    continue;
                Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
                Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity, Projectile.rotation,
                    origin2, new Vector2(Projectile.scale * 0.45f, Projectile.scale * 1.25f), SpriteEffects.None, 0);
            }
            return false;
        }
    }
}