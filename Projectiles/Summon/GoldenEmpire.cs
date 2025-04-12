using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Summon
{
    public class GoldenEmpire : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 78;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (modPlayer.AmalgamateEnch)
            {
                Projectile.alpha = 0;
                Projectile.timeLeft = 180;
            }
            else
            {
                Projectile.alpha += 8;
                Projectile.ai[2] = 2;
            }

            Vector2 standPos = player.MountedCenter + 32 * Vector2.UnitX * -player.direction - 40 * Vector2.UnitY * player.gravDir;
            Projectile.velocity = (standPos - Projectile.Center) * 0.075f;
            Projectile.rotation = player.gravDir == 1 ? 0f : MathHelper.Pi;
            Projectile.frame = (int)Projectile.ai[2];

            var target = Projectile.FindTargetWithinRange(700f);
            if (target != null)
            {
                Projectile.ai[2] = 1;

                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.GoldenEmpireGun>()] < 1)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GoldenEmpireGun>(), Projectile.damage * 2, Projectile.knockBack, Main.myPlayer);
                }

                if (Projectile.Center.X > target.Center.X)
                    Projectile.ai[1] = -1;
                else
                    Projectile.ai[1] = 1;
            }
            else
            {
                Projectile.ai[2] = 0;
                if (player.gravDir != 1)
                    Projectile.ai[1] = -player.direction;
                else
                    Projectile.ai[1] = player.direction;
            }

            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }

        public override void Kill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 59, Projectile.velocity.X, Projectile.velocity.Y, 60, default(Color), 1.5f);
                dustId.noGravity = true;
                Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 59, -Projectile.velocity.X, Projectile.velocity.Y, 60, default(Color), 1.5f);
                dustId2.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = new Color(70, 140, 255, 26);

            SpriteEffects spriteEffects = Projectile.ai[1] == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
                    new Microsoft.Xna.Framework.Rectangle?(rectangle), color27 * Projectile.Opacity, num165, origin2, Projectile.scale, spriteEffects, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
                Projectile.GetAlpha(lightColor) * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);

            Main.EntitySpriteDraw(GMR.Instance.Assets.Request<Texture2D>("Projectiles/Summon/GoldenEmpire_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
                Color.White * Projectile.Opacity, Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }


    public class GoldenEmpireGun : ModProjectile
    {
        public override string Texture => "GMR/NPCs/Enemies/JackGunDrone";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        float speed;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            speed += 0.125f;
            Vector2 velToTarg = player.MountedCenter + Vector2.One.RotatedBy(speed) * 45f;
            Projectile.Center = velToTarg;

            var target = Projectile.FindTargetWithinRange(700f);
            if (target != null)
            {
                if (++Projectile.ai[2] % 10 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(target.Center) * 16f, ModContent.ProjectileType<GoldenEmpireBullet>(),
                        Projectile.damage, Projectile.knockBack, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
                }

                Vector2 toTarget = target.Center - Projectile.Center;


                if (target.Center.X < Projectile.Center.X)
                    Projectile.rotation = toTarget.ToRotation() + MathHelper.ToRadians(180f);
                else
                    Projectile.rotation = toTarget.ToRotation();


                if (target.Center.X < Projectile.Center.X)
                    Projectile.spriteDirection = -1;
                else
                    Projectile.spriteDirection = 1;

                Projectile.timeLeft = 10;
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.GoldenEmpire>()] < 1)
                Projectile.Kill();

            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod mutantMod))
                mutantMod.Call("SummonCrit", true);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;

            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}