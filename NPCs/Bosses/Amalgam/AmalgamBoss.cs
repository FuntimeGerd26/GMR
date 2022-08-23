using Microsoft.Xna.Framework;
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
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace GMR.NPCs.Bosses.Amalgam
{
    [AutoloadBossHead()]
    public class AmalgamBoss : ModNPC
    {
        private int frame = 0;
        private double counting;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amalgam"); //fake
            Main.npcFrameCount[Type] = 5;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 1f,
                PortraitPositionYOverride = 1.2f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 110;
            NPC.lifeMax = 2000;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0.2f;
            NPC.damage = 7;
            NPC.aiStyle = 3;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicID.Boss1;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime && !NPC.AnyNPCs(Type))
            {
                return 0.007f;
            }

            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
            new FlavorTextBestiaryInfoElement("An entity of unknown origin, it simply gives chase to any unlucky enough to find it."),
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                int dustType = 21;
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            Player player = Main.player[NPC.target];
            NPC.lifeMax = (int)(NPC.lifeMax * 1f);
            NPC.lifeMax = NPC.lifeMax / 2 * numPlayers;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            if (NPC.life <= 0)
            {
                int dustType = 21;
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
            }

            if (player.dead)
            {
                NPC.velocity.Y *= 7f;
                NPC.EncourageDespawn(300);
                NPC.localAI[0] = 500;
                    return;
            }
        }

        public override void FindFrame(int FrameHeight)
        {
            int startFrame = 0;
            int finalFrame = Main.npcFrameCount[NPC.type] - 1;

            int frameSpeed = 5;
            NPC.frameCounter += 1f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += FrameHeight;

                if (NPC.frame.Y > finalFrame * FrameHeight)
                {
                    NPC.frame.Y = startFrame * FrameHeight;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            if (Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.DevPlushie>(), 7));
            }
        }

        public override void OnKill()
        {
            int dustType = 21;
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 60, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
            }
        }
    }
}