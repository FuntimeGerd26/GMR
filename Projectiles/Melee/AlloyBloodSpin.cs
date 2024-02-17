using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
    public class AlloybloodSpin : ModProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/AlloybloodSword";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alloyblood Slash");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
            Projectile.AddElement(3);
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 76;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        float atkSpeedBuffs;
        float spinVal;
        float spinValAdd;
        Vector2 shoulderPosition;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.velocity = Vector2.Zero;

            atkSpeedBuffs = (player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic)) * 0.5f;
            if (atkSpeedBuffs < 0.5f)
                atkSpeedBuffs = 0.5f;

            if (spinValAdd < 16f)
                spinValAdd += 0.05f;
            else
                spinValAdd = 16f;
            spinVal += (2f + spinValAdd) * (atkSpeedBuffs);

            shoulderPosition = player.ShoulderPosition();
            Projectile.Center = shoulderPosition - (56 * Vector2.UnitY * player.gravDir).RotatedBy(MathHelper.ToRadians(player.direction * spinVal));

            Vector2 toPlayer = Projectile.Center - player.MountedCenter;
            Projectile.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(45f);

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(player.direction == -1 ? 25f : 70f) - MathHelper.PiOver2);

            if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                if (spinVal > 0f)
                   spinVal += -0.5f;
                Projectile.alpha += 16;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
                return;
            }

            if (Projectile.owner == Main.myPlayer && player.controlUseItem)
            {
                Projectile.timeLeft = 120;
            }

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;

            Projectile.idStaticNPCHitCooldown = 60 - (int)(30 * atkSpeedBuffs);
            if (Projectile.idStaticNPCHitCooldown < 1)
                Projectile.idStaticNPCHitCooldown = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 240);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.InfraRedExplotion>()] < 5)
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.InfraRedExplotion>(),
                    Projectile.damage / 4, Projectile.knockBack, Main.myPlayer);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 speed = Main.MouseWorld - shoulderPosition;
            if (speed.Length() < 400)
                speed = Vector2.Normalize(speed) * 400;
            if (spinValAdd >= 5f)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Normalize(speed) * 20f, ModContent.ProjectileType<AlloybloodSpinThrow>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f);
            Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);
            float opacity = Projectile.Opacity;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos2 = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color2 = new Color(255, 0, 0, 25) * opacity * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                int max0 = (int)k - 1;
                if (max0 < 0)
                    continue;
                float num165 = MathHelper.Lerp(Projectile.oldRot[(int)k], Projectile.oldRot[max0], 1 - k % 1);
                Main.EntitySpriteDraw(texture, drawPos2, null, color2 * (opacity / 2f), num165, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, drawPos, null, color * opacity, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }

    public class AlloybloodSpinThrow : ModProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/AlloybloodSword";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }

        float atkSpeedBuffs;
        int enemiesHit;
        float spinVal;
        float spinUpMult;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            atkSpeedBuffs = (player.GetAttackSpeed(DamageClass.Melee) + player.GetAttackSpeed(DamageClass.Generic)) * 0.5f;
            if (atkSpeedBuffs < 0.5f)
                atkSpeedBuffs = 0.5f;

            spinVal += 12f * atkSpeedBuffs;
            Projectile.rotation = MathHelper.ToRadians(spinVal);

            if (++Projectile.ai[0] <= 80)
            {
                Projectile.velocity *= 0.98f;
            }
            else if (enemiesHit >= 3 || ++Projectile.ai[0] >= 80)
            {
                Projectile.velocity = (player.Center - Projectile.Center);
                Projectile.velocity.Normalize();
                Projectile.velocity *= 8f + spinUpMult;
                spinUpMult += 0.025f;
                Projectile.alpha += 3;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;

            Projectile.idStaticNPCHitCooldown = 30 - (int)(15 * atkSpeedBuffs);
            if (Projectile.idStaticNPCHitCooldown < 5)
                Projectile.idStaticNPCHitCooldown = 5;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            enemiesHit++;
            player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 120);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ArkBomb>()] < 1)
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.ArkBomb>(),
                    Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            float opacity = Projectile.Opacity;
            var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color26 = new Color(255, 0, 0, 25) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
                    Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, spriteEffects, 0);

                Color color27 = color26;
                color27.A = 0;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
                    color27 * 0.9f * opacity, num165, origin2, Projectile.scale, spriteEffects, 0);
            }
            return false;
        }
    }
}
