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


namespace GMR.Projectiles.Melee
{
    public class OvercooledSpearSpin : ModProjectile
    {
        public override string Texture => "GMR/Projectiles/Melee/OvercooledSpear";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(1);
            Projectile.AddElement(3);
        }

        public override void SetDefaults()
        {
            Projectile.width = 340;
            Projectile.height = 340;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 9;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 0.8f, 1f));

            Player player = Main.player[Projectile.owner];
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }
            if (++Projectile.ai[2] == 1)
            {
                Projectile.timeLeft = player.itemAnimationMax * 10;
            }

            Projectile.Center = player.MountedCenter;
            Projectile.velocity = Vector2.Zero;
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.rotation += Projectile.direction * 9f * 0.03f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.ChillBurn>(), 300);
            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 80, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1f);
            dustId.noGravity = true;
            Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 80, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1.5f);
            dustId2.noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 80, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1f);
            dustId.noGravity = true;
            Dust dustId2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 80, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 60, default(Color), 1.5f);
            dustId2.noGravity = true;
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
            Texture2D texture2D14 = ModContent.Request<Texture2D>($"GMR/Assets/Images/TwirlThing", AssetRequestMode.ImmediateLoad).Value;
            Vector2 origin3 = texture2D14.Size() / 2f;

            Color color26 = new Color(55, 200, 255, 100);

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture2D14, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                null, color26, Projectile.rotation, origin3, Projectile.scale * 0.75f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation,
            origin2, Projectile.scale * 1.35f, spriteEffects, 0);
            return false;
        }
    }
}