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

namespace GMR.Projectiles.Melee.CoolSwords
{
    public class PrismaticHammer : CoolSwordBase
    {
        public override string Texture => "GMR/Items/Weapons/Melee/PrismaticHammer";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.ownerHitCheck = true;
            hitboxOutwards = 130;
            rotationOffset = -MathHelper.PiOver4 * 3f;
            Projectile.extraUpdates = 19;
            Projectile.light = 1f;
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
                SoundEngine.PlaySound(SoundID.Item1.WithPitchOffset(-0.75f), Projectile.Center);
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
            Color disco = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.7f,
                Projectile.velocity.Y * -0.5f, 30, disco, 1.5f);
            Main.dust[dustId].noGravity = true;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.Center.X + Main.rand.Next(-target.width / 2, target.width / 2), target.Center.Y + Main.rand.Next(-target.height / 2, target.height / 2)),
                Vector2.Zero, ModContent.ProjectileType<PrismaticSpark>(), 0, 0f, Main.myPlayer);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var center = Main.player[Projectile.owner].Center;
            var handPosition = Main.GetPlayerArmPosition(Projectile) + AngleVector * visualOutwards;
            var drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 25) * Projectile.Opacity;
            var drawCoords = handPosition - Main.screenPosition;
            float size = texture.Size().Length();
            var effects = SpriteEffects.None;
            bool flip = Main.player[Projectile.owner].direction == 1 ? combo > 0 : combo == 0;
            if (flip)
            {
                Main.instance.LoadItem(ModContent.ItemType<Items.Weapons.Melee.PrismaticHammer>());
                texture = TextureAssets.Item[ModContent.ItemType<Items.Weapons.Melee.PrismaticHammer>()].Value;
            }
            var origin = new Vector2(0f, texture.Height);

            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, Color.White, Projectile.rotation, origin, Projectile.scale, effects, 0);
            Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale, effects, 0);

            if (AnimProgress > 0.35f && AnimProgress < 0.75f)
            {
                float intensity = (float)Math.Sin((AnimProgress - 0.35f) / 0.4f * MathHelper.Pi);
                Main.EntitySpriteDraw(texture, handPosition - Main.screenPosition, null, drawColor.UseA(0) * intensity * 0.5f, Projectile.rotation, origin, Projectile.scale, effects, 0);
                Player player = Main.player[Projectile.owner];
                effects = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                var swish = Swish2Texture.Value;
                var swishOrigin = swish.Size() / 2f;
                var swishColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 25) * intensity * intensity * Projectile.Opacity * 0.5f;
                float r = BaseAngleVector.ToRotation() + ((AnimProgress - 0.45f) / 0.2f * 2f - 1f) * -swingDirection * 0.6f;
                var swishLocation = Main.player[Projectile.owner].Center - Main.screenPosition + r.ToRotationVector2() * (size - 20f) * scale;
                Main.EntitySpriteDraw(swish, swishLocation, null, swishColor.UseA(0), r + MathHelper.PiOver2, swishOrigin, Projectile.scale * 1.5f, effects, 0);
            }
            return false;
        }

        public class PrismaticSpark : ModProjectile, IDrawable
        {
            public override string Texture => "GMR/Empty";

            public override void SetDefaults()
            {
                Projectile.width = 5;
                Projectile.height = 5;
                Projectile.aiStyle = -1;
                Projectile.timeLeft = 600;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.extraUpdates = 9;
            }

            public override void AI()
            {
                Projectile.alpha += 2;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
                Projectile.rotation += MathHelper.ToRadians(0.2f);
            }

            public override bool PreDraw(ref Color lightColor)
            {
                return false;
            }

            DrawLayer IDrawable.DrawLayer => DrawLayer.AfterProjectiles;
            public void Draw(Color lightColor)
            {
                Player player = Main.player[Projectile.owner];
                Texture2D flash = Request<Texture2D>("GMR/Assets/Images/Flash01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                Vector2 flashOrigin = flash.Size() / 2f;
                Color disco = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) * 1.3f;

                Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, disco * Projectile.Opacity,
                    Projectile.rotation, flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(flash, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, disco * Projectile.Opacity,
                    Projectile.rotation + MathHelper.ToRadians(90f), flashOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            }
        }
    }
}