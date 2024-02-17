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
    public class DualDesertAxe : CoolSwordBase
    {
        public override string Texture => "GMR/Items/Weapons/Melee/DualDesertAxe";

        public override void SetStaticDefaults()
        {
            Projectile.AddElement(0);
            Projectile.AddElement(3);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 124;
            Projectile.height = 124;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 124;
            rotationOffset = -MathHelper.PiOver4 * 3f;
            Projectile.extraUpdates = 19;
            Projectile.localNPCHitCooldown = 30;
            Projectile.light = 0.5f;
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
                player.GPlayer().itemCombo = (ushort)(combo == 0 ? 64 : 0);
            }
            if (!playedSound && AnimProgress > 0.4f)
            {
                playedSound = true;
                SoundEngine.PlaySound(GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(0.75f), Projectile.Center);
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
                return -40f * p;
            }
            if (progress < 0.2f)
            {
                return -40f * (1f - progress / 0.2f);
            }*/
            return 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.knockBack *= 4f;
            Projectile.damage = Projectile.damage * 2;
            
            Player player = Main.player[Projectile.owner];
            Projectile.damage = Projectile.damage + Projectile.damage / 4;
            
            player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 600);
            player.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 600);
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.DamnSun>(), 1200);
            
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SmallExplotion>()] < 1)
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.SmallExplotion>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);

            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 1f,
                Projectile.velocity.Y * 1f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * -1f,
                Projectile.velocity.Y * -1f, 60, default(Color), 2f);
            Main.dust[dustId2].noGravity = true;
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

            if (AnimProgress > 0.35f && AnimProgress < 0.75f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);

                var swish = Swish2Texture.Value;
                var swishOrigin = swish.Size() / 2f;
                var swishColor = new Color(100, 120, 80, 80) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor.UseA(0), r + MathHelper.PiOver2, swishOrigin, 1.5f, effects, 0);
            }

            if (AnimProgress > 0.10f && AnimProgress < 0.90f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);
                Player player = Main.player[Projectile.owner];
                effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                var swishOrigin = swish.Size() / 2;
                var swishColor = new Color(120, 200, 40, 0) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor, r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 2f, effects, 0);
            }
            return false;
        }
    }
}