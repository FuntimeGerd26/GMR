using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Magic
{
    public class SpaceDoggoProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_927";

        public int rotationDirection;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space Doggo Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 800;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 4;
            Projectile.localNPCHitCooldown = 5;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.rotation = Projectile.velocity.ToRotation();
                rotationDirection = Math.Sign(Projectile.ai[1]);
                SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
            }

            if (Projectile.localAI[0] == 1) //extend out, locked to move around player
            {
                Projectile.ai[0] += Projectile.velocity.Length();
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];

                if (Projectile.Distance(Main.player[Projectile.owner].Center) > Math.Abs(Projectile.ai[1]))
                {
                    Projectile.localAI[0]++;
                    Projectile.localAI[1] = Math.Sign(Projectile.ai[1]);
                    Projectile.ai[0] = Math.Abs(Projectile.ai[1]) - Projectile.velocity.Length();
                    Projectile.ai[1] = 0;
                    Projectile.netUpdate = true;
                }
            }
            else if (Projectile.localAI[0] == 2) //orbit player, please dont ask how this code works i dont know either
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
                Projectile.Center += Projectile.velocity.RotatedBy(Math.PI / 2 * Projectile.localAI[1]);
                Projectile.velocity = Projectile.DirectionFrom(Main.player[Projectile.owner].Center) * Projectile.velocity.Length();

                if (++Projectile.ai[1] > 180)
                {
                    Projectile.localAI[0]++;
                    Projectile.localAI[1] = 0;
                    Projectile.ai[0] = 0;
                    Projectile.ai[1] = 0;
                    Projectile.netUpdate = true;
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
            }
            else if (Projectile.timeLeft > 60) //now flying away, go into homing mode
            {
                var target = Projectile.FindTargetWithinRange(1200f);
                if (target != null)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 10f, 0.125f);
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 1;
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.5f, 60, new Color(Main.DiscoR, 125, 255, 0), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var drawPosition = Projectile.Center;
            var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
            lightColor = Projectile.GetAlpha(lightColor);
            var frame = texture.Frame(verticalFrames: Main.projFrames[Projectile.type], frameY: Projectile.frame);
            frame.Height -= 2;
            var origin = frame.Size() / 2f;
            float opacity = Projectile.Opacity;
            int trailLength = ProjectileID.Sets.TrailCacheLength[Type];
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f - 1f / trailLength * i;
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + drawOffset, frame, new Color(Main.DiscoR, 125, 255, 125) * progress * opacity, Projectile.oldRot[i], origin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.position + drawOffset, frame, Projectile.GetAlpha(lightColor) * opacity, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            var swish = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            var swishFrame = swish.Frame(verticalFrames: 1);
            var swishColor = new Color(Main.DiscoR, 125, 255, 0) * opacity;
            var swishOrigin = swishFrame.Size() / 2;
            float swishScale = Projectile.scale * 1f;
            var swishPosition = Projectile.position + drawOffset;

            Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
            var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
            var flareOrigin = flare.Size() / 2f;
            float flareOffset = (swish.Width / 2f - 4f);
            var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
            float flareDirectionDistance = 200f;
            float swishRotation = Projectile.rotation;
            for (int i = 0; i < 1; i++)
            {
                Main.EntitySpriteDraw(
                    swish,
                    swishPosition,
                    swishFrame,
                    swishColor,
                    swishRotation,
                    swishOrigin,
                    swishScale, SpriteEffects.None, 0);
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    var flarePosition = (swishRotation + MathHelper.ToRadians(33f) + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
                    float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
                    Main.EntitySpriteDraw(
                        flare,
                        swishPosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        0f,
                        flareOrigin,
                        new Vector2(swishScale * 0.7f, swishScale * 2f) * flareIntensity, SpriteEffects.None, 0);

                    Main.EntitySpriteDraw(
                        flare,
                        swishPosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        MathHelper.PiOver2,
                        flareOrigin,
                        new Vector2(swishScale * 0.8f, swishScale * 2.5f) * flareIntensity, SpriteEffects.None, 0);
                }
            }

            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            // Trail funny
            float modifier = Projectile.localAI[1] * MathHelper.Pi / 45;

            for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
            {
                Player player = Main.player[Projectile.owner];
                Texture2D glow = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Color color27 = Color.Lerp(new Color(Main.DiscoR, 125, 255, 125), Color.Transparent, (float)Math.Cos(Projectile.ai[0]) / 3 + 0.3f);
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                float scale = Projectile.scale - (float)Math.Cos(Projectile.ai[0]) / 5;
                scale *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                int max0 = Math.Max((int)i - 1, 0);
                Vector2 center = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], (1 - i % 1));
                float smoothtrail = i % 1 * (float)Math.PI / 3.45f;
                bool withinangle = Projectile.rotation > -Math.PI / 2 && Projectile.rotation < Math.PI / 2;
                if (withinangle && player.direction == 1)
                    smoothtrail *= -1;
                else if (!withinangle && player.direction == -1)
                    smoothtrail *= -1;
                center += Projectile.Size / 2;

                Vector2 offset = (Projectile.Size / 12).RotatedBy(Projectile.oldRot[(int)i] - smoothtrail * (-Projectile.direction));
                Main.spriteBatch.Draw(glow, center - offset - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, color27 * opacity, Projectile.rotation, glow.Size() / 2, scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, 125, 255, 0);
    }
}