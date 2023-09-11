using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee.CoolSwords
{
    public class UltraBlueChainsaw : CoolSwordBase
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 74;
            Projectile.height = 74;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 70;
            rotationOffset = -MathHelper.PiOver4 * 3f;
            Projectile.extraUpdates = 4;
        }

        protected override void Initialize(Player player, GerdPlayer gerd)
        {
            base.Initialize(player, gerd);
            if (gerd.itemCombo > 0)
            {
                swingDirection *= -1;
            }
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].itemAnimation <= 1)
            {
                player.GPlayer().itemCombo = (ushort)(combo == 0 ? 32 : 0);
            }
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center,
                    AngleVector * Projectile.velocity.Length() * 9f,
                    ModContent.ProjectileType<Projectiles.Melee.XShotBlade>(), (int)(Projectile.damage * 0.90f), Projectile.knockBack / 4f, Projectile.owner);

                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);
            }
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
            /*if (progress > 0.8f)
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
            }*/
            return 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item22, Projectile.position);
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Thoughtful>(), 1200);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var center = Main.player[Projectile.owner].Center;
            var handPosition = Main.GetPlayerArmPosition(Projectile) + AngleVector * visualOutwards;
            var drawColor = Projectile.GetAlpha(lightColor) * Projectile.Opacity;
            var drawCoords = handPosition - Main.screenPosition;
            float size = texture.Size().Length();
            var effects = SpriteEffects.None;
            bool flip = Main.player[Projectile.owner].direction == 1 ? combo > 0 : combo == 0;
            if (flip)
            {
                texture = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/CoolSwords/UltraBlueChainsawFlip", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
            var origin = new Vector2(0f, texture.Height);

            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            if (AnimProgress > 0.35f && AnimProgress < 0.75f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);

                var swish = Swish2Texture.Value;
                var swishOrigin = swish.Size() / 2f;
                var swishColor = new Color(100, 120, 140, 80) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor.UseA(0), r + MathHelper.PiOver2, swishOrigin, 1f, effects, 0);
            }
            return false;
        }
    }
}