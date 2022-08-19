using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.NPCs.Bosses.Jack
{
    [AutoloadBossHead()]
    public class Jack : ModNPC
    {
        private int frame = 0;
        private double counting;

        public static int MinionType()
        {
            return ModContent.NPCType<JackArmGun>();
            return ModContent.NPCType<JackArmGunFlip>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("[c/c25b70:J-06-26, Jack]");
            Main.npcFrameCount[Type] = 6;
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
            NPC.width = 110;
            NPC.height = 122;
            NPC.lifeMax = 12000;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 0;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicID.Boss3;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
            new FlavorTextBestiaryInfoElement("A machine made with the forges on the underworld, it is weak compared to other creations that have came from them, the ones under all."),
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                int dustType = 60;
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
                float numberProjectiles = 8;
                float rotation = MathHelper.ToRadians(90);
                Vector2 velocity2;
                velocity2 = new Vector2(0f, -20f);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity2.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.AttackPreview>(), 0, 1f, Main.myPlayer, NPC.whoAmI);
                }
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            Player player = Main.player[NPC.target];
            NPC.lifeMax = (int)(NPC.lifeMax * 1f) + 5;
            NPC.lifeMax += numPlayers * 1000;
        }

        public override void AI()
        {
            Rectangle displayPoint = new Rectangle(NPC.Hitbox.Center.X, NPC.Hitbox.Center.Y - NPC.height / 4, 2, 2);
            if (NPC.ai[1] == 0)
            {
                CombatText.NewText(displayPoint, Color.Red, "DEPLOYING WEAPONS");
            }

            if ((int)NPC.ai[1] == 0)
            {
                NPC.ai[1]++;
                int count = 1;
                int spawnX = (int)NPC.position.X + NPC.width / 2;
                int spawnY = (int)NPC.position.Y + NPC.height / 2;
                int type = ModContent.NPCType<JackArmGun>();
                int type1 = ModContent.NPCType<JackArmGunFlip>();
                int type2 = ModContent.NPCType<JackArmClaw>();
                int type3 = ModContent.NPCType<JackArmClawFlip>();
                for (int i = 0; i < count; i++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, type, NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, type1, NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, type2, NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, type3, NPC.whoAmI, 0f, NPC.whoAmI);
                }
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (!NPC.AnyNPCs(ModContent.NPCType<JackArmGunFlip>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClawFlip>()))
            {
                int heal = (int)(NPC.lifeMax / 90 * Main.rand.NextFloat(1f, 1.5f));
                NPC.life += heal;
                if (NPC.life > NPC.lifeMax)
                    NPC.life = NPC.lifeMax;
                CombatText.NewText(NPC.Hitbox, CombatText.HealLife, heal);
                NPC.ai[1] = 0;
                if (NPC.ai[1] == 0)
                {
                    CombatText.NewText(displayPoint, Color.Red, "DEPLOYING WEAPONS... AGAIN");
                }

                if (NPC.life <= 0)
                {
                    int dustType = 60;
                    for (int i = 0; i < 40; i++)
                    {
                        Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                        Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                        dust.noLight = false;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                    }
                    float numberProjectiles = 8;
                    float rotation = MathHelper.ToRadians(90);
                    Vector2 velocity2;
                    velocity2 = new Vector2(0f, -20f);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = velocity2.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.AttackPreview>(), 0, 1f, Main.myPlayer, NPC.whoAmI);
                    }
                }
                return;
            }

            if (player.dead)
            {
                NPC.velocity.Y += 0.25f;
                NPC.EncourageDespawn(300);
                NPC.localAI[0] = 500;
                return;
            }
            else if (++NPC.localAI[0] > 500) //No lock
            {
                NPC.Center = NPC.Center;
            }
            else if (++NPC.localAI[0] > 480) //Reset
            {
                NPC.localAI[0] = 0;
            }
            else if (++NPC.localAI[0] > 360) //Left
            {
                NPC.Center = Main.player[NPC.target].Center - 150 * Vector2.UnitX;

                int dustId = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId].noGravity = true;
                int dustId3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId3].noGravity = true;
            }
            else if (++NPC.localAI[0] > 240) //Up
            {
                NPC.Center = Main.player[NPC.target].Center - 150 * Vector2.UnitY;

                int dustId = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId].noGravity = true;
                int dustId3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId3].noGravity = true;
            }
            else if (++NPC.localAI[0] > 120) //Right
            {
                NPC.Center = Main.player[NPC.target].Center + 150 * Vector2.UnitX;

                int dustId = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId].noGravity = true;
                int dustId3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId3].noGravity = true;
            }
            else //Down
            {
                NPC.Center = Main.player[NPC.target].Center + 150 * Vector2.UnitY;

                int dustId = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId].noGravity = true;
                int dustId3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                    NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
                Main.dust[dustId3].noGravity = true;
            }
        }

        public override void FindFrame(int FrameHeight)
        {
            int startFrame = 0;
            int finalFrame = Main.npcFrameCount[NPC.type] - 1;

            int frameSpeed = 5;
            NPC.frameCounter += 1.5f;
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
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackCannon>(), 20));
        }

        public override void OnKill()
        {
            int dustType = 60;
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