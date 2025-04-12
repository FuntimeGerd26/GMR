using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
    public class CyberNeonAxe : ModProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/Others/CyberNeonAxe";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        float spinVal;
        float spinValAdd;
        Vector2 shoulderPosition;
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Player player = Main.player[Projectile.owner];

            if (spinValAdd < 6f)
                spinValAdd += 0.025f;
            else
                spinValAdd = 6f;
            spinVal += (4f + spinValAdd) * player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic);

            shoulderPosition = player.ShoulderPosition();
            Projectile.Center = shoulderPosition - (56 * Vector2.UnitY * player.gravDir).RotatedBy(MathHelper.ToRadians(player.direction * spinVal));

            Vector2 toPlayer = Projectile.Center - shoulderPosition;
            Projectile.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(45f);

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(player.direction == -1 ? 25f : 70f) - MathHelper.PiOver2);

            if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                if (spinVal > 0f)
                   spinVal += -0.5f;
                Projectile.alpha += 16;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
                return;
            }

            if (Projectile.owner == Main.myPlayer && player.controlUseItem)
            {
                Projectile.timeLeft = 1200;
            }

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;

            Projectile.localNPCHitCooldown = 30 - (int)(15 * player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic));
            if (Projectile.localNPCHitCooldown < 5)
                Projectile.localNPCHitCooldown = 5;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 projPos = new Vector2(target.Center.X + Main.rand.Next(-30, 30), target.Center.Y + Main.rand.Next(-30, 30));
            Vector2 velocityToTarget = ((new Vector2(target.Center.X, target.Center.Y)) - projPos) * 0.15f;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, velocityToTarget, ModContent.ProjectileType<CyberNeonCut>(),
                Projectile.damage / 2, Projectile.knockBack / 4f, Projectile.owner);

            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 speed = Main.MouseWorld - player.MountedCenter;
            if (speed.Length() < 400)
                speed = Vector2.Normalize(speed) * 400;
            if (spinValAdd >= 5f)
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Normalize(speed) * 24f, ModContent.ProjectileType<CyberNeonAxeThrow>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity;

            Color color26 = new Color(255, 55, 55, 80);

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
                float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
                Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity,
                    num165, origin2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture2D13, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor) * opacity,
                Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }

    public class CyberNeonAxeThrow : ModProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/Others/CyberNeonAxe";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }

        int enemiesHit;
        float spinVal;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            spinVal += 12f * player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic);
            Projectile.rotation = MathHelper.ToRadians(spinVal);

            if (++Projectile.ai[0] <= 80)
            {
                Projectile.velocity *= 0.98f;
            }
            else if (enemiesHit >= 3 || ++Projectile.ai[0] >= 80)
            {
                Projectile.velocity = (Main.MouseWorld - Projectile.Center);
                Projectile.velocity.Normalize();
                Projectile.velocity *= 8f;
                Projectile.alpha += 3;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;

            Projectile.localNPCHitCooldown = 30 - (int)(15 * player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic));
            if (Projectile.localNPCHitCooldown < 5)
                Projectile.localNPCHitCooldown = 5;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            enemiesHit++;

            Vector2 projPos = new Vector2(target.Center.X + Main.rand.Next(-30, 30), target.Center.Y + Main.rand.Next(-30, 30));
            Vector2 velocityToTarget = ((new Vector2(target.Center.X, target.Center.Y)) - projPos) * 0.15f;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, velocityToTarget, ModContent.ProjectileType<CyberNeonCut>(),
                Projectile.damage / 2, Projectile.knockBack / 4f, Projectile.owner);

            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity;

            Color color26 = new Color(255, 55, 55, 80);

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
                float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
                Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity,
                    num165, origin2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture2D13, Projectile.position + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor) * opacity,
                Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }

    public class CyberNeonCut : ModProjectile
    {
        public override string Texture => "GMR/Projectiles/SpearTrail";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        float decreaseScaleY;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(1f, 0.25f, 0.25f));

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.99f;
            Projectile.alpha += 16;
            decreaseScaleY += 0.1f;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity;

            Color color26 = new Color(255, 55, 55, 80);

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
                float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
                Main.spriteBatch.Draw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity,
                    num165, origin2, new Vector2(Projectile.scale, Projectile.scale * 0.2f * (1f - decreaseScaleY)), SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
