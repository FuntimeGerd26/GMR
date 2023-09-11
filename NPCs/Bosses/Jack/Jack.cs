using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.NPCs.Bosses.Jack
{
    [AutoloadBossHead()]
    public class Jack : ModNPC
    {
        public bool PlayAnimation;
        public bool PlayerDead;
        public bool HalfHealthSummon;
        public bool Alive;
        public int AI4;
        public int AI5;
        public int AI6;

        public static int MinionType()
        {
            return ModContent.NPCType<JackArmGun>();
            return ModContent.NPCType<JackArmClaw>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jack");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 1f,
                PortraitPositionYOverride = 1.2f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[Type] = 13;
        }

        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 122;
            NPC.lifeMax = 7500;
            NPC.defense = 20;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 15;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 7);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/Jack");
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false; // Set to false because fuck contact damage
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            new FlavorTextBestiaryInfoElement("An ancient machine that was never finished, it has a really bad temper for this reason. It launched an attack on it's creators for never bothring finishing it. Consuming souls causes it to become more powerful for following fights, making it a threat possibly as dangerous as the mechanical bosses."),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int dustType = 60;
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
            }
        }

        public override void AI()
        {
            if (NPC.damage > 30)
                NPC.damage = 30;
            
            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }

            if (NPC.life <= NPC.lifeMax / 2 && !HalfHealthSummon && Main.getGoodWorld) // When at 50% HP, summon this new NPC once
            {
                int spawnX = (int)NPC.position.X + NPC.width / 2;
                int spawnY = (int)NPC.position.Y + NPC.height / 2 - 300;
                for (int i = 0; i < 1; i++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackDrone>(), NPC.whoAmI, 0f, 0f);
                }
                HalfHealthSummon = true; // Set to true for it to not summon more
                NPC.netUpdate = true;
            }

            // If no Arms or under 10% health make the NPC vulnerable
            if (!NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()) || NPC.life < (int)(NPC.lifeMax * 0.1))
            {
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            else
            {
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
            }

            if (NPC.dontTakeDamage == false) // If it's vulnerable
            {
                // After 5 seconds summon more arms
                if (++NPC.ai[3] == 900)
                {
                    NPC.ai[2] = 0; // Check the code below for how arms are summoned
                    NPC.netUpdate = true;
                }
            }
            else // If not Vulnerable (AKA dosen't have arms or is under 10% health)
            {
                NPC.ai[3] = 0; // Set to 0 for it to not summon arms imediatelly again
                NPC.ai[2] = 10; // Set higher than 0 for it to not summon more arms than it should
                NPC.netUpdate = true;
            }

            // This summons new Arms, and a Sword if on FTW seed
            if (NPC.ai[2] == 0)
            {
                NPC.ai[2]++;
                int count = 1;
                int spawnX = (int)player.position.X + player.width / 2;
                int spawnY = (int)player.position.Y + player.height / 2;
                for (int i = 0; i < count; i++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackArmGun>(), NPC.whoAmI, 0f, 0f, 5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackArmClaw>(), NPC.whoAmI, 0f, 0f, 5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackArmGun>(), NPC.whoAmI, 0f, 0f, -5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackArmClaw>(), NPC.whoAmI, 0f, 0f, -5f);
                }

                    int max = Main.masterMode ? 60 : 90;
                    for (int i = 0; i < max; i++)
                    {
                        Vector2 dir = NPC.DirectionTo(player.Center).RotatedBy(2 * (float)Math.PI / max * i) * 8f;
                        if (Main.masterMode)
                            dir *= Main.getGoodWorld ? 1.5f : 2f;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, dir * NPC.width / 120f, ModContent.ProjectileType<Projectiles.Bosses.JackBlastSpin>(),
                            NPC.damage * 4, 14f, Main.myPlayer, 1f, dir.ToRotation());
                        SoundEngine.PlaySound(SoundID.Item74, NPC.Center);
                    }
                NPC.netUpdate = true;
            }

            if (!Main.dayTime || player.dead)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y += 7f;
                NPC.EncourageDespawn(300);
                if (!PlayerDead)
                {
                    if (!Main.dayTime || Main.rand.NextBool(4))
                    {
                        Main.NewText("Good night", Color.Red);
                        PlayerDead = true;
                    }
                    else
                    {
                        Main.NewText("OBJECTIVE DOWNED", Color.Red);
                        PlayerDead = true;
                    }
                }
                return;
            }
            else
            {
                // Where's the boss?
                Vector2 bossToPlayer = Main.player[NPC.target].Center - 150 * Vector2.UnitY;
                NPC.velocity = (bossToPlayer - NPC.Center) * 0.055f;
                NPC.netUpdate = true;
            }

            if (NPC.dontTakeDamage == false && ++NPC.ai[2] % 480 == 0 || NPC.life < NPC.lifeMax / 2 && ++NPC.ai[2] % 480 == 0)
            {
                SideAttack();
                NPC.netUpdate = true;
            }

            if (NPC.life < (int)(NPC.lifeMax * 0.75) && Main.expertMode && ++AI6 % 480 == 0)
            {
                RainAttack();
                NPC.netUpdate = true;
            }

            if (NPC.life < (int)(NPC.lifeMax * 0.35) && ++AI4 % 600 == 0)
            {
                WallAttack();
                NPC.netUpdate = true;
            }

            if (NPC.dontTakeDamage == false && NPC.life > (int)(NPC.lifeMax * 0.1) && ++AI5 % 240 == 0)
            {
                float numberProjectiles = 8;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / numberProjectiles * (i + 0.5)) * 4f,
                    ModContent.ProjectileType<Projectiles.Bosses.JackShuriken>(), NPC.damage, 1f, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                    NPC.netUpdate = true;
                }
            }
        }

        private void RainAttack()
        {
            Player player = Main.player[NPC.target];

            if (Main.masterMode)
            {
                player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.50f);
                int max = 8;
                for (int i = 0; i < max; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 4f,
                    ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 1f, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                    NPC.netUpdate = true;
                }
            }

            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.JackMarker>(), NPC.damage, 0f, Main.myPlayer);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, new Vector2(0f, 0.0001f), ModContent.ProjectileType<Projectiles.Bosses.JackRain>(), NPC.damage, 0f, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item61, NPC.Center);
            NPC.netUpdate = true;
        }

        private void SideAttack()
        {
            Player player = Main.player[NPC.target];

            float numberProjectiles = 4;
            float rotation = MathHelper.ToRadians(90f);
            Vector2 velocity = new Vector2(0f, 6f);
            Vector2 velocity2 = new Vector2(0f, -6f);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                Vector2 perturbedSpeed2 = velocity2.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, perturbedSpeed2, ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.JackMarker>(), NPC.damage, 0f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                NPC.netUpdate = true;
            }
        }

        private void WallAttack()
        {
            Player player = Main.player[NPC.target];
            player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.50f);

            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
            float x = 1000f;
            float y = 1800f;
            int amount = 24;
            var posX = player.position.X + player.width / 2f;
            var posY = player.position.Y + player.height / 2f - y / 2f;
            float yAdd = y / (amount / 2);
            int type = ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>();
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + x, posY + yAdd * i), new Vector2(-3f, 0f), type, NPC.damage, 1f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX - x, posY + yAdd * i), new Vector2(3f, 0f), type, NPC.damage, 1f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
                NPC.netUpdate = true;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            if(Main.expertMode)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.AncientCore>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Consumable.DGPCrate>(), 4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackRailcannon>(), 100));

            int[] drops = { ModContent.ItemType<Items.Weapons.Melee.JackSword>(), ModContent.ItemType<Items.Weapons.Ranged.AncientRifle>(), ModContent.ItemType<Items.Weapons.Melee.AncientPickaxe>(), };

            npcLoot.Add(ItemDropRule.OneFromOptions(1, drops));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.JuiceBox>(), 5, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.LeadBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.TungstenBar, 2, 5, 16));
            npcLoot.Add(ItemDropRule.Common(ItemID.SilverBar, 2, 5, 16));
        }

        public override void OnKill()
        {
            GerdWorld.downedJack = true;

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

        public override void FindFrame(int FrameHeight)
        {
            int startFrame = 0;
            int finalFrame = Main.npcFrameCount[NPC.type] - 1;

            int frameSpeed = 10; // Used to delay in frames an animation
            NPC.frameCounter += 1f; // How fast the frames are going

            if (Main.rand.NextBool(50) && NPC.frame.Y == 7 * FrameHeight) // Enable the animation randomly as long as the 8th frame is reached
                PlayAnimation = true;

            // Randomly if the current frame is the 8th or higher and the counter per tick is over the speed, next frame
            if (PlayAnimation && (NPC.frame.Y >= 7 * FrameHeight) && NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0; // Reset counter
                NPC.frame.Y += FrameHeight; // Next frame
                frameSpeed = 15;

                if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past the final frame
                {
                    NPC.frame.Y = startFrame + 7 * FrameHeight; // Reset to the 8th frame
                    PlayAnimation = false; // Distable animation
                }
            }
            else if (NPC.frame.Y < 7 * FrameHeight && NPC.frameCounter > frameSpeed) // As long as there's no random animation playing
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += FrameHeight;

                if (NPC.frame.Y == 7 * FrameHeight) // If the current frame is the 8th frame
                {
                    NPC.frame.Y = startFrame + 7 * FrameHeight; // Reset to the 8th frame
                }

                /*if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past all frames
                {
                    NPC.frame.Y = startFrame * FrameHeight; // Reset to the first frame
                }*/
            }
        }
    }
}
