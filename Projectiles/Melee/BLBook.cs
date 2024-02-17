using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
    public class BLBook : ModProjectile
    {
        public override string Texture => "GMR/Items/Accessories/BLBook";

        public int rotationDirection;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BL Book");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 4;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                rotationDirection = Math.Sign(Projectile.ai[1]);
                SoundEngine.PlaySound(SoundID.Item7, Projectile.Center);
            }

            if (Projectile.localAI[0] == 1) //extend out, locked to move around player
            {
                Projectile.ai[0] += Projectile.velocity.Length();
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];

                if (Projectile.Distance(Main.player[Projectile.owner].Center) > Math.Abs(Projectile.ai[1]))
                {
                    Projectile.localAI[0]++;
                    Projectile.localAI[1] = Math.Sign(Projectile.ai[1]);
                    Projectile.ai[0] = Math.Abs(Projectile.ai[1]) - Projectile.velocity.Length();
                    Projectile.ai[1] = 0;
                    Projectile.netUpdate = true;
                }
            }
            else if (Projectile.localAI[0] == 2) //orbit player, please dont ask how this code works i dont know either
            {
                //Projectile.ai[0] += Projectile.velocity.Length();
                Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
                Projectile.Center += Projectile.velocity.RotatedBy(Math.PI / 2 * Projectile.localAI[1]);
                Projectile.velocity = Projectile.DirectionFrom(Main.player[Projectile.owner].Center) * Projectile.velocity.Length();
                Projectile.velocity += 0.025f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);
            }

            Projectile.direction = Projectile.spriteDirection = rotationDirection;
            Projectile.rotation = 0f;

            Player player = Main.player[Projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<Buffs.Minions.BLBook>()))
            {
                Projectile.Kill();
                    return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = Projectile.GetAlpha(lightColor);

            SpriteEffects effects = SpriteEffects.None;

            for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
            {
                Color color27 = color26;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
                if (max0 < 0)
                    continue;
                Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
                Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, Projectile.rotation, origin2, Projectile.scale, effects, 0);
            }

            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(color26), Projectile.rotation, origin2, Projectile.scale, effects, 0);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}