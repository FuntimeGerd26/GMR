using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Summon
{
    public class VirtueWisps : MinionBase
    {
        public override string Texture => "Terraria/Images/Projectile_596";

        private float _pulseTimer;
        public int ShootTimer;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.DesertDjinnCurse];
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
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

        private int BossesDowned;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.5f, 1.25f));

            if (++Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            if (!GerdHelper.UpdateProjActive<Buffs.Minions.VirtueWisps>(Projectile))
            {
                return;
            }

            if (NPC.downedMoonlord) BossesDowned = 6;
            else if (Main.hardMode) BossesDowned = 3;
            else BossesDowned = 1;

            var target = Projectile.FindTargetWithinRange(1000f);
            if (target != null)
            {
                Vector2 Aim = Projectile.DirectionTo(target.Center) * 8f;
                int damage = (int)(target.lifeMax * 0.005f);
                if (damage < 10)
                    damage = 10;
                if (++ShootTimer % 60 == 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Aim, ModContent.ProjectileType<Projectiles.Summon.TearProj>(), damage * BossesDowned, Projectile.knockBack, Main.myPlayer);
                }
            }

            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = 1;
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
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
            }

            Projectile.direction = Projectile.spriteDirection;
            Projectile.rotation = 0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle frame = new Rectangle(0, y3, texture.Width, num156);
            Vector2 origin = frame.Size() / 2f;
            var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;
            var color = new Color(25, 125, 255);
            int trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f / trailLength * i;
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + offset, new Microsoft.Xna.Framework.Rectangle?(frame), new Color(25, 125, 255, 55) * (1f - progress), 0, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.position + offset, new Microsoft.Xna.Framework.Rectangle?(frame), color, 0, origin, Projectile.scale, SpriteEffects.None, 0);
            float wave = GerdHelper.Wave(_pulseTimer * 4f, 0f, 4f);
            color *= 0.1f + wave * 0.1f;
            color.A = 0;
            for (int i = 0; i < 4; i++)
            {
                Main.EntitySpriteDraw(texture, Projectile.position + offset + new Vector2(wave, 0f).RotatedBy(MathHelper.PiOver2 * i + Projectile.rotation), new Microsoft.Xna.Framework.Rectangle?(frame), color, 0, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
