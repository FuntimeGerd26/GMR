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
    public class InfraRedSpearSpin : ModProjectile
    {
        public override string Texture => "GMR/Items/Weapons/Melee/InfraRedSpear";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infra-Red Spear");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Projectile.AddElement(0);
            Projectile.AddElement(2);
        }

        public override void SetDefaults()
        {
            Projectile.width = 288;
            Projectile.height = 288;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.light = 0.25f;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 9;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Player player = Main.player[Projectile.owner];
            if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.owner == Main.myPlayer && player.controlUseItem)
            {
                Projectile.timeLeft = 2;
            }
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.MountedCenter;

            Projectile.direction = player.direction;
            Projectile.spriteDirection = Projectile.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.reuseDelay = 10;
            Projectile.rotation += Projectile.spriteDirection * 8f * 0.03f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            player.Heal(player.statLifeMax / 100);
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 900);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.InfraRedExplotion>()] < 1)
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.InfraRedExplotion>(),
                    (int)(Projectile.damage * 0.75), Projectile.knockBack, Main.myPlayer);
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
                Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override void Kill(int timeLeft) //self reuse so you dont need to hold up always while autofiring
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer && player.controlUseTile && (!player.controlUp || player.controlUp) && player.HeldItem.type == ModContent.ItemType<Items.Weapons.Melee.InfraRedSpear>()
                && player.ownedProjectileCounts[Projectile.type] == 1)
            {
                Vector2 spawnPos = player.MountedCenter;
                Vector2 speed = Main.MouseWorld - spawnPos;
                if (speed.Length() < 360)
                    speed = Vector2.Normalize(speed) * 360;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                player.ChangeDir(Math.Sign(speed.X));
            }
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

            Color color26 = new Color(255, 55, 55);

            SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.None;

            Main.EntitySpriteDraw(texture2D14, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                null, color26, Projectile.rotation, origin3, Projectile.scale * 0.58f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), lightColor, Projectile.rotation,
            origin2, Projectile.scale * 1.75f, spriteEffects, 0);
            return false;
        }
    }
}