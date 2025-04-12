using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee.CoolSwords
{
    public abstract class CoolSwordBase : ModProjectile
    {
        public static Asset<Texture2D> SwishTexture => ModContent.Request<Texture2D>(typeof(CoolSwordBase).Namespace.Replace('.', '/') + "/Swish", AssetRequestMode.ImmediateLoad);
        public static Asset<Texture2D> Swish2Texture => ModContent.Request<Texture2D>(typeof(CoolSwordBase).Namespace.Replace('.', '/') + "/Swish2", AssetRequestMode.ImmediateLoad);

        public static SoundStyle HeavySwing => GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f);

        private bool _init;
        public int swingDirection;
        public int hitboxOutwards;
        public int visualOutwards;
        public float rotationOffset;
        public bool forced50;
        public float scale;

        public bool playedSound;

        public bool damaging;
        public int damageTime;

        public int combo;

        private float armRotation;
        private Vector2 angleVector;
        public Vector2 AngleVector { get => angleVector; set => angleVector = Vector2.Normalize(value); }
        public Vector2 BaseAngleVector => Vector2.Normalize(Projectile.velocity);
        public virtual float AnimProgress => 1f - (Main.player[Projectile.owner].itemAnimation * (Projectile.extraUpdates + 1) + Projectile.numUpdates + 1) / (float)(Main.player[Projectile.owner].itemAnimationMax * (Projectile.extraUpdates + 2));
        public float lastAnimProgress;

        public virtual bool SwingSwitchDir => AnimProgress > 0.6f && AnimProgress < 0.7f;

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 50;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ignoreWater = true;
        }

        public override bool? CanDamage()
        {
            return (AnimProgress > 0.4f && AnimProgress < 0.6f) ? null : false;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (player.itemAnimation > 1 && player.ownedProjectileCounts[Type] <= 1)
            {
                Projectile.timeLeft = 2;
            }

            var gerd = player.GetModPlayer<GerdPlayer>();

            player.heldProj = Projectile.whoAmI;
            if (!_init)
            {
                Projectile.scale = 1f;
                Initialize(player, player.GPlayer());
                scale = Projectile.scale;
                Projectile.netUpdate = true;
                _init = true;
            }

            if (SwingSwitchDir)
            {
                UpdateDirection(player);
            }

            if (!player.frozen && !player.stoned)
            {
                var arm = Main.GetPlayerArmPosition(Projectile);
                float progress = AnimProgress;
                lastAnimProgress = progress;
                if (!forced50 && progress >= 0.5f)
                {
                    progress = 0.5f;
                    forced50 = true;
                }
                InterpolateSword(progress, out var angleVector, out float swingProgress, out float scale, out float outer);
                AngleVector = angleVector;
                Projectile.position = arm + AngleVector * hitboxOutwards;
                Projectile.position.X -= Projectile.width / 2f;
                Projectile.position.Y -= Projectile.height / 2f;
                Projectile.rotation = (arm - Projectile.Center).ToRotation() + rotationOffset;
                UpdateSwing(progress, swingProgress);
                if (Main.netMode != NetmodeID.Server)
                {
                    SetArmRotation(player, progress, swingProgress);
                }
                Projectile.scale = scale;
                visualOutwards = (int)outer;
            }
        }

        public virtual void UpdateSwing(float progress, float interpolatedSwingProgress)
        {
            Player player = Main.player[Projectile.owner];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();
            if (progress == 0.5f && Main.myPlayer == Projectile.owner)
            {
                if (modPlayer.InfraRedSet != null)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, AngleVector * Projectile.velocity.Length() * 9f, ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 4f, Projectile.owner);
            }
        }

        public virtual float SwingProgress(float progress)
        {
            return progress;
        }
        public static float GenericSwing2(float progress, float pow = 2f)
        {
            if (progress > 0.5f)
            {
                return 0.5f - GenericSwing2(0.5f - (progress - 0.5f), pow) + 0.5f;
            }
            return ((float)Math.Sin(Math.Pow(progress, pow) * MathHelper.TwoPi - MathHelper.PiOver2) + 1f) / 2f;
        }
        public static float GenericSwing1(float progress, float pow = 2f, float startSwishing = 0.15f)
        {
            float oldProg = progress;
            float max = 1f - startSwishing;
            if (progress < startSwishing)
            {
                progress *= (float)Math.Pow(progress / startSwishing, pow);
            }
            else if (progress > max)
            {
                progress -= max;
                progress = startSwishing - progress;
                progress *= (float)Math.Pow(progress / startSwishing, pow);
                progress = startSwishing - progress;
                progress += max;
            }
            return MathHelper.Clamp((float)Math.Sin(progress * MathHelper.Pi - MathHelper.PiOver2) / 2f + 0.5f, 0f, oldProg);
        }
        public virtual Vector2 GetOffsetVector(float progress)
        {
            return BaseAngleVector.RotatedBy((progress * MathHelper.Pi - MathHelper.PiOver2) * -swingDirection);
        }
        public virtual float GetScale(float progress)
        {
            return scale;
        }
        public virtual float GetVisualOuter(float progress, float swingProgress)
        {
            return visualOutwards;
        }
        public void InterpolateSword(float progress, out Vector2 offsetVector, out float swingProgress, out float scale, out float outer)
        {
            swingProgress = SwingProgress(progress);
            offsetVector = GetOffsetVector(swingProgress);
            scale = GetScale(swingProgress);
            outer = (int)GetVisualOuter(progress, swingProgress);
        }

        public void UpdateDirection(Player player)
        {
            if (angleVector.X < 0f)
            {
                player.direction = -1;
                Projectile.direction = -1;
            }
            else if (angleVector.X > 0f)
            {
                player.direction = 1;
                Projectile.direction = 1;
            }
        }

        protected virtual void Initialize(Player player, GerdPlayer gerd)
        {
            AngleVector = Projectile.velocity;
            combo = gerd.itemCombo;
            if (player.whoAmI == Projectile.owner)
                GerdHelper.CappedMeleeScale(Projectile);
            swingDirection = 1;
            UpdateDirection(player);
            swingDirection *= Projectile.direction;
        }

        protected virtual void SetArmRotation(Player player, float progress, float swingProgress)
        {
            var diff = Main.player[Projectile.owner].MountedCenter - Projectile.Center;
            if (Math.Sign(diff.X) == -player.direction || progress < 0.1f)
            {
                var v = diff;
                v.X = Math.Abs(diff.X);
                armRotation = v.ToRotation();
            }

            if (armRotation > 1.1f)
            {
                player.bodyFrame.Y = 56;
            }
            else if (armRotation > 0.5f)
            {
                player.bodyFrame.Y = 56 * 2;
            }
            else if (armRotation < -0.5f)
            {
                player.bodyFrame.Y = 56 * 4;
            }
            else
            {
                player.bodyFrame.Y = 56 * 3;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(swingDirection == -1);
            writer.Write(combo);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swingDirection = reader.ReadBoolean() ? -1 : 1;
            combo = reader.ReadInt32();
        }
    }
}