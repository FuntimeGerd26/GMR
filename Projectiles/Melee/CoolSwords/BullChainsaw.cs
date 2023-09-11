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
    public class BullChainsaw : CoolSwordBase
    {
        public override string Texture => "GMR/Items/Weapons/Melee/BullChainsaw";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 82;
            Projectile.height = 82;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 82;
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
                SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(-0.25f), Projectile.Center);
                SoundEngine.PlaySound(SoundID.Item23.WithPitchOffset(-0.25f), Projectile.Center);
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);

                if (modPlayer.BullSet == true)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 18f, ModContent.ProjectileType<Projectiles.Melee.ZombieSlash>(), Projectile.damage * 2, Projectile.knockBack / 2f, Projectile.owner);
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
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (modPlayer.BullSet != null)
                target.AddBuff(BuffID.Venom, 120);
            target.immune[Projectile.owner] = 7;
            SoundEngine.PlaySound(SoundID.Item22, Projectile.position);
            target.AddBuff(BuffID.Poisoned, 120);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, Projectile.velocity.X * 0.2f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
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
            var origin = new Vector2(0f, texture.Height);

            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            if (AnimProgress > 0.15f && AnimProgress < 0.85f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);

                effects = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                var Color26 = new Color(155, 55, 255, 0);
                var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                var swishOrigin = swish.Size() / 2;
                var swishColor = Color26 * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor, r + MathHelper.PiOver2, swishOrigin, 1.5f, effects, 0);


                Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
                var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
                var flareOrigin = flare.Size() / 2f;
                float flareOffset = (swish.Width / 2f - 4f);
                var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
                float flareDirectionDistance = 200f;

                for (int j = 0; j < 1; j++)
                {
                    var flarePosition = (r + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
                    float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
                    Main.EntitySpriteDraw(
                        flare,
                        swishLocation + flarePosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        0f,
                        flareOrigin,
                        new Vector2(Projectile.scale * 1.5f * 0.7f, Projectile.scale * 1.5f * 2f) * flareIntensity, SpriteEffects.None, 0);

                    Main.EntitySpriteDraw(
                        flare,
                        swishLocation + flarePosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        MathHelper.PiOver2,
                        flareOrigin,
                        new Vector2(Projectile.scale * 1.5f * 0.8f, Projectile.scale * 1.5f * 2.5f) * flareIntensity, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}