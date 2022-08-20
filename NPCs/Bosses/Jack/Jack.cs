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
            DisplayName.SetDefault("J-06-26, Jack");
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneSkyHeight && !Main.dayTime)
            {
                return 0.012f;
            }

            return 0f;
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
            NPC.lifeMax = NPC.lifeMax / 2 * numPlayers;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            Rectangle displayPoint = new Rectangle(NPC.Hitbox.Center.X, NPC.Hitbox.Center.Y - NPC.height / 4, 2, 2);
            if (NPC.ai[1] == -1)
            {
                CombatText.NewText(displayPoint, Color.Red, "DEPLOYING WEAPONS");
            }

            if ((int)NPC.ai[1] == 0 || NPC.ai[1] == -1)
            {
                NPC.ai[1]++;
                int count = 1;
                int spawnX = (int)NPC.position.X + NPC.width / 2;
                int spawnY = (int)NPC.position.Y + NPC.height / 2;
                for (int i = 0; i < count; i++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackArmGun>(), NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackArmGunFlip>(), NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackArmClaw>(), NPC.whoAmI, 0f, NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackArmClawFlip>(), NPC.whoAmI, 0f, NPC.whoAmI);
                }
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
            }

            if (NPC.ai[1] == 1)
            {
                Vector2 toPlayer = player.Center - NPC.Center;

                float offsetX = 20f;

                Vector2 abovePlayer = player.Top + new Vector2(NPC.direction * offsetX, -NPC.height);

                Vector2 toAbovePlayer = abovePlayer - NPC.Center;
                Vector2 toAbovePlayerNormalized = toAbovePlayer.SafeNormalize(Vector2.UnitY);

                float changeDirOffset = offsetX * 0.7f;

                if (NPC.direction == -1 && NPC.Center.X - changeDirOffset < abovePlayer.X ||
                    NPC.direction == 1 && NPC.Center.X + changeDirOffset > abovePlayer.X)
                {
                    NPC.direction *= -1;
                }

                float speed = 25f;
                float inertia = 20f;

                // If the boss is somehow below the player, move faster to catch up
                if (NPC.Top.Y > player.Bottom.Y)
                {
                    speed = 35f;
                }

                Vector2 moveTo = toAbovePlayerNormalized * speed;
                NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;

                //NPC.position = (Main.player[NPC.target].Center + 150 * Vector2.UnitY) - NPC.Center;
                NPC.rotation = NPC.velocity.ToRotation() * -0.0173f;
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<JackArmGunFlip>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClawFlip>()) && NPC.life <= NPC.lifeMax / 10)
            {
                int heal = (int)(NPC.lifeMax / 90 * Main.rand.NextFloat(1f, 1.5f));
                NPC.life += heal;
                if (NPC.life > NPC.lifeMax)
                    NPC.life = NPC.lifeMax;
                CombatText.NewText(NPC.Hitbox, CombatText.HealLife, heal);
                NPC.ai[1] = 0;
                if (NPC.ai[1] == 0)
                {
                    NPC.ai[2] = 0;
                    CombatText.NewText(displayPoint, Color.Red, "DEPLOYING WEAPONS... AGAIN");
                }
                NPC.dontTakeDamage = true;
                return;
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
                    Main.NewText("Well sh-", Color.Red);
                    Vector2 perturbedSpeed = velocity2.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.AttackPreview>(), 0, 1f, Main.myPlayer, NPC.whoAmI);
                }
            }

            if (player.dead)
            {
                NPC.velocity.Y *= 7f;
                NPC.EncourageDespawn(300);
                NPC.localAI[0] = 500;
                if (NPC.ai[2] == 0)
                {
                    NPC.ai[2]++;
                    Main.NewText("See ya on the flipside", Color.Red);
                }
                    return;
            }
            /*if (NPC.ai[1] == 2)
            {
                if (++NPC.localAI[0] > 500) //No lock
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
            }*/
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
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackCannon>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackRifle>(), 26));
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