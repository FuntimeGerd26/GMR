using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class AncientPickaxe : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/AncientPickaxe";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Pickaxe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

        private const int maxTime = 45;

        public override void SetDefaults()
        {
            Projectile.width = 104;
            Projectile.height = 104;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 0;
            Projectile.timeLeft = maxTime;
        }

        public override void AI()
        {
            //dust!
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0f,
                0f, 100, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0f,
                0f, 100, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;

            Player player = Main.player[Projectile.owner];
            /*if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
            {
                Projectile.Kill();
                return;
            }*/

            if (player.dead || !player.active)
            {
                Projectile.Kill();
                return;
            }

            //Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter);
            //Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            if (++Projectile.localAI[0] > 10)
            {
                Projectile.localAI[0] = 0;
            }

            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[0] = Main.rand.Next(10);
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }

            Projectile.localAI[1]++;
            float straightModifier = -0.5f * (float)Math.Cos(Math.PI * 2 / maxTime * Projectile.localAI[1]);
            float sideModifier = 0.5f * (float)Math.Sin(Math.PI * 2 / maxTime * Projectile.localAI[1]) * player.direction;

            Vector2 baseVel = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 straightVel = baseVel * straightModifier;
            Vector2 sideVel = baseVel.RotatedBy(Math.PI / 2) * sideModifier;

            Projectile.Center = player.Center + baseVel / 2f;
            Projectile.velocity = straightVel + sideVel;
            Projectile.rotation += (float)Math.PI / 6.85f * -player.direction;
        }

        public override void Kill(int timeLeft) //self reuse so you dont need to hold up always while autofiring
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer && player.controlUseTile && player.altFunctionUse == 2
                && player.HeldItem.type == ModContent.ItemType<Items.Weapons.Melee.AncientPickaxe>()
                && player.ownedProjectileCounts[Projectile.type] == 1)
            {
                Vector2 spawnPos = player.MountedCenter;
                Vector2 speed = Main.MouseWorld - spawnPos;
                if (speed.Length() < 180)
                    speed = Vector2.Normalize(speed) * 180;
                int damage = player.GetWeaponDamage(player.HeldItem);
                Projectile.CritChance = player.GetWeaponCrit(player.HeldItem);
                float knockBack = player.GetWeaponKnockback(player.HeldItem, player.HeldItem.knockBack);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Vector2.Normalize(speed), Projectile.type, damage, knockBack, Projectile.owner, speed.X, speed.Y);
                player.ChangeDir(Math.Sign(speed.X));
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 55, 55, 55) * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = Projectile.GetAlpha(color26);

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Color color27 = color26 * 0.5f;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Vector2 value4 = Projectile.position;//Projectile.oldPos[i];
                float num165 = Projectile.oldRot[i];
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle),
                Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}