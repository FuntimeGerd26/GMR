using System;
using System.IO;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace GMR.Projectiles.Melee.SpecialSwords
{
    public class DreamingSiltcurrent : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_972";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool flag4 = false;
            if (Projectile.DamageType.UseStandardCritCalcs && Main.rand.Next(100) < Projectile.CritChance)
            {
                flag4 = true;
            }

            float dam = Projectile.damage;
            float num21 = Projectile.knockBack;

            dam *= Main.rand.NextFloat(1.5f, 1f);
            NPC.HitModifiers modifiers = target.GetIncomingStrikeModifiers(Projectile.DamageType, Projectile.direction);
            int? num26 = Main.player[Projectile.owner].Center.X < target.Center.X ? 1 : -1;
            modifiers.Knockback *= num21 / Projectile.knockBack;
            if (num26.HasValue)
            {
                modifiers.HitDirectionOverride = num26;
            }
            NPC.HitInfo strike = modifiers.ToHitInfo((int)dam, flag4, num21, damageVariation: true, Main.player[Projectile.owner].luck);

            Vector2 projPos = new Vector2(target.Center.X + Main.rand.Next(-30, 30), target.Center.Y + Main.rand.Next(-30, 30));
            Vector2 velocityToTarget = ((new Vector2(target.Center.X, target.Center.Y)) - projPos) * 0.15f;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, velocityToTarget, ModContent.ProjectileType<DreamingSiltcurrentCut>(), Projectile.damage / 2, 0f, Projectile.owner);

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), projPos, Vector2.Zero, ModContent.ProjectileType<DreamingSiltcurrentSpark>(), 0, 0f, Projectile.owner);

            if (Main.netMode != 0)
            {
                NetMessage.SendStrikeNPC(target, in strike);
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0f, 1f, 0.85f));

            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num4 = Projectile.ai[0];
            float num5 = Projectile.velocity.ToRotation();
            Projectile.rotation = (float)Math.PI * num4 * num + num5 + num4 * (float)Math.PI + player.fullRotation;
            float num7 = 0.8f;
            float num8 = 1.2f;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num8 + num * num7;
            float num10 = Projectile.rotation + Main.rand.NextFloatDirection() * ((float)Math.PI / 2f) * 0.7f;
            Vector2 vector2 = Projectile.Center + num10.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 vector3 = (num10 + Projectile.ai[0] * ((float)Math.PI / 2f)).ToRotationVector2();
            if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Dust dust8 = Dust.NewDustPerfect(Projectile.Center + num10.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 156, vector3 * 1f);
                dust8.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust8.noGravity = true;
            }
            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
            {
                var dust9 = Dust.NewDustPerfect(vector2, 156, vector3 * 1f);
                dust9.noGravity = true;
            }
            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill();
            }

            if (!Projectile.noEnchantmentVisuals)
            {
                UpdateEnchantmentVisuals();
            }
        }
        public override void CutTiles()
        {
            Vector2 vector2 = (Projectile.rotation - (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 vector3 = (Projectile.rotation + (float)Math.PI / 4f).ToRotationVector2() * 60f * Projectile.scale;
            float num2 = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + vector2, Projectile.Center + vector3, num2, DelegateMethods.CutTiles);
        }

        public override bool? CanCutTiles() => true;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength2 = 94f * Projectile.scale;
            float num21 = (float)Math.PI * 2f / 25f * Projectile.ai[0];
            float maximumAngle2 = (float)Math.PI / 4f;
            float num22 = Projectile.rotation + num21;
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, num22, maximumAngle2))
            {
                return true;
            }
            float num23 = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (num23 > 0f)
            {
                float coneRotation2 = num22 - (float)Math.PI / 4f * Projectile.ai[0] * num23;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength2, coneRotation2, maximumAngle2))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateEnchantmentVisuals()
        {
            if (Projectile.npcProj)
            {
                return;
            }
            Vector2 boxPosition = Projectile.position;
            int boxWidth = Projectile.width;
            int boxHeight = Projectile.height;
            for (float num = -(float)Math.PI / 4f; num <= (float)Math.PI / 4f; num += (float)Math.PI / 2f)
            {
                Rectangle r = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + num).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(r.TopLeft(), r.Width, r.Height);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
            playerArmPosition.Y -= player.gfxOffY;

            Vector2 vector = Projectile.Center - Main.screenPosition;
            Asset<Texture2D> asset = TextureAssets.Projectile[Projectile.type];
            Rectangle rectangle = asset.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = Projectile.scale * 1.1f;
            SpriteEffects effects = (SpriteEffects)(!(Projectile.ai[0] >= 0f) ? 2 : 0);
            float num2 = Projectile.localAI[0] / Projectile.ai[1] * 1.25f;
            float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
            float num4 = 0.975f;
            Color color6 = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
            Vector3 val = color6.ToVector3();
            float fromValue = val.Length() / (float)Math.Sqrt(3.0);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Color color = new Color(0, 50, 60, 255);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)rectangle, color * fromValue * num3, Projectile.rotation + Projectile.ai[0] * ((float)Math.PI / 4f) * -1f * (1f - num2), origin, num, effects, 0f);
            Color color2 = new Color(25, 100, 125, 100);
            //Color color3 = new(227, 182, 245, 80);
            Color color4 = new Color(0, 200, 255, 0) * num3;
            color4.A = (byte)(float)(int)(color4.A * (1f - fromValue));
            Color color5 = color4 * fromValue * 0.5f;
            color5.G = (byte)(color5.G * fromValue);
            color5.B = (byte)(color5.R * (0.25f + fromValue * 0.75f));
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)rectangle, color5 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)rectangle, color2 * fromValue * num3 * 0.3f, Projectile.rotation, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)rectangle, color2 * fromValue * num3 * 0.5f, Projectile.rotation, origin, num * num4, effects, 0f);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)asset.Frame(1, 4, 0, 3), color4 * 0.75f * num3, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)asset.Frame(1, 4, 0, 3), color4 * 0.6f * num3, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(asset.Value, vector, (Rectangle?)asset.Frame(1, 4, 0, 3), color4 * 0.55f * num3, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, num * 0.6f, effects, 0f);
            for (float num5 = 0f; num5 < 8f; num5 += 1f)
            {
                float num6 = Projectile.rotation + Projectile.ai[0] * num5 * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * Projectile.ai[0];
                Vector2 drawpos = vector + num6.ToRotationVector2() * (asset.Width() * 0.5f - 6f) * num;
                float num7 = num5 / 9f;
                DrawPrettyStarSparkle(Projectile.Opacity, 0, drawpos, new Color(155, 155, 255, 0) * num3 * num7, color2, num2, 0f, 0.5f, 0.5f, 1f,
                    num6, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num, Vector2.One * num);
            }
            Vector2 drawpos2 = vector + (Projectile.rotation + Utils.Remap(num2, 0f, 1f, 0f, (float)Math.PI / 4f) * Projectile.ai[0]).ToRotationVector2() * (asset.Width() * 0.5f - 4f) * num;
            DrawPrettyStarSparkle(Projectile.Opacity, 0, drawpos2, new Color(155, 155, 255, 0) * num3 * 0.5f, color2, num2, 0f, 0.5f, 0.5f, 1f,
                0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f)) * num, Vector2.One * num);
            return false;
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart,
            float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D value = TextureAssets.Extra[98].Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = (byte)0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawpos, null, color, (float)Math.PI / 2f + rotation, origin, vector, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color, 0f + rotation, origin, vector2, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir);
            Main.EntitySpriteDraw(value, drawpos, null, color2, 0f + rotation, origin, vector2 * 0.6f, dir);
        }
    }

    public class DreamingSiltcurrentSpark : ModProjectile
    {
        public override string Texture => "GMR/Empty";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 7;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0f, 1f, 0.85f));

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
            Color color26 = new Color(55, 255, 255, 100) * opacity;
            Scaling += -0.025f;

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
                color26, MathHelper.ToRadians(90f), origin2, new Vector2(Projectile.scale * 0.4f * (1f + Scaling), Projectile.scale * 0.5f), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null,
                color26, 0f, origin2, new Vector2(Projectile.scale * 0.3f * (1f + Scaling), Projectile.scale * 0.4f), SpriteEffects.None, 0);
            return false;
        }
    }

    public class DreamingSiltcurrentCut : ModProjectile
    {
        public override string Texture => "GMR/Projectiles/SpearTrail";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(2);
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        float decreaseScaleY;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0f, 1f, 0.8f));

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

            Color color26 = new Color(0, 255, 200, 10);

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
                    num165, origin2, new Vector2(Projectile.scale * 0.75f, Projectile.scale * 0.15f * (1f - decreaseScaleY)), SpriteEffects.None, 0);
            }
            return false;
        }
    }
}