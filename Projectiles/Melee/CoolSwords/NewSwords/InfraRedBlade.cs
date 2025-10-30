using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace GMR.Projectiles.Melee.CoolSwords.NewSwords
{
    public class InfraRedBlade : HeldSlashingSwordProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/Swords/InfraRedBlade";

        public override void SetStaticDefaults()
        {
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 76;
            Projectile.height = 76;
            swordHeight = 76;
            gfxOutOffset = -4;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }

        public override void AI()
        {
            base.AI();
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(SoundID.Item1.WithPitchOffset(-1f), Projectile.Center);
            }
            if (AnimProgress > 0.3f && AnimProgress < 0.6f)
            {
                int amt = Main.rand.Next(4) + 1;
                for (int i = 0; i < amt; i++)
                {
                    var velocity = AngleVector.RotatedBy(MathHelper.PiOver2 * -swingDirection) * Main.rand.NextFloat(2f, 8f);
                    var d = Dust.NewDustPerfect(Main.player[Projectile.owner].Center + AngleVector * Main.rand.NextFloat(10f, 70f * Projectile.scale), 60, velocity, newColor: Color.White.UseA(0));
                    d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    d.scale *= Projectile.scale;
                    d.fadeIn = d.scale + 0.1f;
                    d.noGravity = true;
                }
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_HeldItem(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f,
                        ModContent.ProjectileType<JackSwordExplode>(), (int)(Projectile.damage * 0.9f), Projectile.knockBack / 4f, Projectile.owner);
                else
                    Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_HeldItem(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 12f,
                    ModContent.ProjectileType<JackSwordThrow>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);

            }
        }

        public override Vector2 GetOffsetVector(float progress)
        {
            if (progress < 0.5f)
                return base.GetOffsetVector(progress);
            return BaseAngleVector.RotatedBy((progress * (MathHelper.Pi * 1.5f) - MathHelper.PiOver2 * 1.5f) * -swingDirection * (0.8f + 0.2f * Math.Min(Main.player[Projectile.owner].GetModPlayer<GerdPlayer>().itemUsage / 300f, 1f)));
        }

        public override float SwingProgress(float progress)
        {
            return SwingProgressGMR(progress);
        }
        public override float GetScale(float progress)
        {
            float scale = base.GetScale(progress);
            if (progress > 0.1f && progress < 0.9f)
            {
                return scale + 0.25f * (float)Math.Pow(Math.Sin((progress - 0.1f) / 0.9f * MathHelper.Pi), 2f);
            }
            return scale;
        }
        public override float GetVisualOuter(float progress, float swingProgress)
        {
            return 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            freezeFrame = 5;
            SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/SwordImpact", 3, 0.25f, 0f, 0.25f), Projectile.Center);

            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 120);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.5f, 60, Color.HotPink, 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var center = Main.player[Projectile.owner].Center;
            var drawColor = Projectile.GetAlpha(lightColor) * Projectile.Opacity;
            var glowColor = new Color(255, 110, 110, 0);
            float animProgress = AnimProgress;
            float swishProgress = 0f;
            float intensity = 0f;
            if (animProgress > 0.1f && animProgress < 0.95f)
            {
                swishProgress = (animProgress - 0.1f) / 0.15f;
                intensity = (float)Math.Sin(MathF.Pow(swishProgress, 2f) * MathHelper.Pi);
            }

            GetSwordDrawInfo(out var texture, out var handPosition, out var frame, out float rotationOffset, out var origin, out var effects);
            float size = texture.Size().Length();
            DrawSwordAfterImages(texture, handPosition, frame, glowColor * 0.75f * Projectile.Opacity, rotationOffset, origin, effects,
                loopProgress: 0.07f, interpolationValue: -0.01f);

            DrawSword(texture, handPosition, frame, Projectile.GetAlpha(lightColor) * Projectile.Opacity, rotationOffset, origin, effects);

            return false;
        }
    }
}