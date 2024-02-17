using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee.CoolSwords
{
    public class PrismMirrorProj : CoolSwordBase
    {
        public override string Texture => "GMR/Items/Weapons/Melee/PrismaticMirror";
        public float colorProgress;

        public override void SetStaticDefaults()
        {
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 92;
            Projectile.height = 92;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 92;
            rotationOffset = -MathHelper.PiOver4 * 3f;
            Projectile.extraUpdates = 14;
        }

        protected override void Initialize(Player player, GerdPlayer gerd)
        {
            base.Initialize(player, gerd);
            if (gerd.itemCombo > 0)
            {
                swingDirection *= -1;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].itemAnimation <= 1)
            {
                player.GPlayer().itemCombo = (ushort)(combo == 0 ? 20 : 0);
            }
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(0.5f), Projectile.Center);
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 38f, ModContent.ProjectileType<Projectiles.Melee.PrismaticBeam>(), (int)(Projectile.damage * 0.8f), Projectile.knockBack, Projectile.owner);

                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);
            }
        }

        public override Vector2 GetOffsetVector(float progress)
        {
            Player player = Main.player[Projectile.owner];
            if (progress < 0.5f)
                return base.GetOffsetVector(progress);
            return BaseAngleVector.RotatedBy((progress * (MathHelper.Pi * 1.5f) - MathHelper.PiOver2 * 1.5f) * -swingDirection * (0.8f + 0.2f * Math.Min(player.GPlayer().itemUsage / 300f, 1f)));
        }

        public override float SwingProgress(float progress)
        {
            return GenericSwing2(progress);
        }
        public override float GetScale(float progress)
        {
            float scale = base.GetScale(progress);
            if (progress > 0.4f && progress < 0.6f)
            {
                return scale;
            }
            return scale;
        }
        public override float GetVisualOuter(float progress, float swingProgress)
        {
            if (progress > 0.8f)
            {
                float p = 1f - (1f - progress) / 0.2f;
                Projectile.alpha = (int)(p * 255);
                return -10f * p;
            }
            if (progress < 0.35f)
            {
                float p = 1f - (progress) / 0.35f;
                Projectile.alpha = (int)(p * 255);
                return -10f * p;
            }
            return 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 1f,
                Projectile.velocity.Y * 1f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * -1f,
                Projectile.velocity.Y * -1f, 60, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
            Main.dust[dustId2].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            var texture = TextureAssets.Projectile[Type].Value;
            var center = Main.player[Projectile.owner].Center;
            var handPosition = Main.GetPlayerArmPosition(Projectile) + AngleVector * visualOutwards;
            var drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 25) * Projectile.Opacity;
            var drawCoords = handPosition - Main.screenPosition;
            float size = texture.Size().Length();
            var effects = SpriteEffects.None;
            bool flip = Main.player[Projectile.owner].direction == 1 ? combo > 0 : combo == 0;
            var origin = new Vector2(0f, texture.Height);

            foreach (var v in GerdHelper.CircularVector(4, Main.GlobalTimeWrappedHourly))
            {
                Main.EntitySpriteDraw(texture, drawCoords + v * 2f * Projectile.scale, null, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);
                Main.EntitySpriteDraw(texture, drawCoords + v * 2f * Projectile.scale, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
            }

            if (AnimProgress > 0.45f && AnimProgress < 0.65f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.25f) * 2f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity, Projectile.rotation, origin, Projectile.scale, effects, 0);

                Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
                var shine = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
                var shineOrigin = shine.Size() / 2f;
                var shineColor = drawColor * intensity * intensity * Projectile.Opacity;
                var shineLocation = handPosition - Main.screenPosition + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * ((size - 8f) * Projectile.scale);
                Main.EntitySpriteDraw(shine, shineLocation, null, shineColor, 0f, shineOrigin, new Vector2(Projectile.scale * 0.5f, Projectile.scale) * intensity, effects, 0);
                Main.EntitySpriteDraw(shine, shineLocation, null, shineColor, MathHelper.PiOver2, shineOrigin, new Vector2(Projectile.scale * 0.5f, Projectile.scale * 2f) * intensity, effects, 0);
            }

            if (AnimProgress > 0.10f && AnimProgress < 0.90f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);
                effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                var swishOrigin = swish.Size() / 2;
                var swishColor = drawColor * intensity * intensity * Projectile.Opacity * 0.5f;
                var swishColor2 = new Color(255, 255, 255, 25) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor2, r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 1.5f, effects, 0);
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor, r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 1.5f, effects, 0);
                Main.EntitySpriteDraw(GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlashLight", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
                    swishLocation, null, swishColor, r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 1.5f, effects, 0);
            }
            return false;
        }
    }
}