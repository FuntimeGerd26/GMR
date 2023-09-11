using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace GMR.NPCs.Bosses.Jack
{
    public class AcheronArmGun : ModNPC
    {
        public int Attack;
        public int ParentIndex
        {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        // Helper method to determine the body type
        public static int BodyType()
        {
           return ModContent.NPCType<Acheron>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acheron Cannon");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 72;
            NPC.lifeMax = 1000;
            NPC.defense = 3;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath37;
            NPC.knockBackResist = 0.5f;
            NPC.damage = 18;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.npcSlots = 1f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return false; // Set to false because fuck contact damage
        }


        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Acheron>()))
            {
                NPC.life += -200;

                if (NPC.life <= 0)
                {
                    int dustType = 60;
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                        Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 30, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                        dust.noLight = false;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                    }
                    SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
                }
                NPC.netUpdate = true;
                return;
            }

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }

            if (NPC.damage > 36)
                NPC.damage = 36;

            Vector2 toPlayer = NPC.Center - player.Center;
            NPC.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(90f);

            if (player.dead)
            {
                NPC.netUpdate = true;
                NPC.Center = NPC.position;
                NPC.velocity.Y += 0.5f;
                NPC.EncourageDespawn(300);
                return;
            }
            else if (NPC.ai[2] > 0)
            {
                NPC.netUpdate = true;
                NPC.spriteDirection = 1;

                if (NPC.ai[0] < 0)
                {
                    Vector2 bossToPlayer = Main.player[NPC.target].Center + 350 * Vector2.UnitX + 300 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                }
                else
                {
                    Vector2 bossToPlayer = Main.player[NPC.target].Center + 350 * Vector2.UnitX - 300 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                }
            }
            else if (NPC.ai[2] < 0)
            {
                NPC.netUpdate = true;
                NPC.spriteDirection = -1;

                if (NPC.ai[0] < 0)
                {
                    Vector2 bossToPlayer = Main.player[NPC.target].Center - 350 * Vector2.UnitX + 300 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                }
                else
                {
                    Vector2 bossToPlayer = Main.player[NPC.target].Center - 350 * Vector2.UnitX - 300 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.075f;
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
                if (++NPC.ai[1] > 301) // After 5 seconds plus one tick, reset
                {
                    NPC.ai[3] = 0;
                    NPC.ai[1] = 0;
                    NPC.ai[0]++;
                    NPC.ai[0] *= -1;
                    NPC.netUpdate = true;
                }
                else if (++NPC.ai[1] > 300) // After 5 seconds
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 4f, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                    SoundEngine.PlaySound(SoundID.Research, NPC.Center);
                    NPC.netUpdate = true;
                }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/AcheronArmGun_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
            if (NPC.spriteDirection == -1)
                spriteEffects = SpriteEffects.None;

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, Color.White, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            return false;
        }
    }
}