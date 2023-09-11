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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 140;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                Projectile.rotation = 0f + MathHelper.ToRadians(-45f);
                Projectile.alpha += 8;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
                return;
            }
            if (Projectile.owner == Main.myPlayer && player.controlUseItem)
            {
                Projectile.timeLeft = 1200;
            }
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 target = Main.MouseWorld;
            Projectile.velocity = (target - Projectile.Center) * 0.025f;

            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;
            Projectile.rotation += 0.20f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.AddBuff(ModContent.BuffType<Buffs.Buff.BloodFountain>(), 120);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ArkBomb>()] < 1)
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.ArkBomb>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
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
            float opacity = Projectile.Opacity / 2;
            var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

            SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color26 = Color.Red * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.Red * opacity, Projectile.rotation, origin2, Projectile.scale * 1.5f, spriteEffects, 0);

                Color color27 = color26;
                color27.A = 0;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 1.5f, spriteEffects, 0);
            }

            var swish = GMR.Instance.Assets.Request<Texture2D>("Projectiles/Melee/SwordSlash2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var swishFrame = swish.Frame(verticalFrames: 1);
            var swishColor = new Color(255, 75, 75, 0) * opacity;
            var swishOrigin = swishFrame.Size() / 2;
            float swishScale = Projectile.scale * 1f;
            var swishPosition = Projectile.position + drawOffset;

            Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
            var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
            var flareOrigin = flare.Size() / 2f;
            float flareOffset = (swish.Width / 2f - 4f);
            var flareDirectionNormal = Vector2.Normalize(Projectile.velocity) * flareOffset;
            var flareDirectionNormal2 = Vector2.Normalize(-Projectile.velocity) * flareOffset;
            float flareDirectionDistance = 200f;
            for (int i = 0; i < 2; i++)
            {
                float swishRotation = Projectile.rotation + MathHelper.Pi * i;
                Main.EntitySpriteDraw(
                    swish,
                    swishPosition,
                    swishFrame,
                    swishColor,
                    swishRotation,
                    swishOrigin,
                    swishScale, SpriteEffects.None, 0);

                for (int j = 0; j < 3; j++)
                {
                    var flarePosition = (swishRotation + 0.6f * (j - 1)).ToRotationVector2() * flareOffset;
                    float flareIntensity = Math.Max(flareDirectionDistance - Vector2.Distance(flareDirectionNormal, flarePosition), 0f) / flareDirectionDistance;
                    Main.EntitySpriteDraw(
                        flare,
                        swishPosition + flarePosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        0f,
                        flareOrigin,
                        new Vector2(swishScale * 0.7f, swishScale * 2f) * flareIntensity, SpriteEffects.None, 0);

                    Main.EntitySpriteDraw(
                        flare,
                        swishPosition + flarePosition,
                        null,
                        swishColor * flareIntensity * 3f * 0.4f,
                        MathHelper.PiOver2,
                        flareOrigin,
                        new Vector2(swishScale * 0.8f, swishScale * 2.5f) * flareIntensity, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}
