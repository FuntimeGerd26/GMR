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
    public class LunarNovaAxe : CoolSwordBase
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 60;
            rotationOffset = -MathHelper.PiOver4 * 3.14f;
            Projectile.extraUpdates = 9;
            Projectile.localNPCHitCooldown = 30;
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
            Lighting.AddLight(Projectile.Center, new Vector3(0.85f, 0.25f, 1f));
            Player player = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].itemAnimation <= 1)
            {
                player.GPlayer().itemCombo = (ushort)(combo == 0 ? 32 : 0);
            }
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(0.75f), Projectile.Center);
            }
        }

        public override void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, AngleVector * Projectile.velocity.Length() * 12f,
                    ModContent.ProjectileType<Projectiles.Melee.LunarCut>(), Projectile.damage, Projectile.knockBack / 2f, Projectile.owner);

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
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 179, Projectile.velocity.X * 0.5f,
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
            if (flip)
            {
                Main.instance.LoadItem(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>());
                texture = TextureAssets.Item[ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>()].Value;
            }
            var origin = new Vector2(0f, texture.Height);

            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            if (AnimProgress > 0.35f && AnimProgress < 0.75f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);

                var swish = Swish2Texture.Value;
                var swishOrigin = swish.Size() / 2f;
                var swishColor = new Color(175, 55, 255) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor.UseA(0), r + MathHelper.PiOver2, swishOrigin, 1f, effects, 0);
            }
            return false;
        }
    }
}