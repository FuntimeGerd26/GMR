using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.NPCs.Bosses.CyberNeon
{
    [AutoloadBossHead()]
    public class CyberNeon : ModNPC
    {
        float AI0;
        float AI1;
        float AI2;
        float AI3;
        float AI4;
        bool PlayerDead;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 1f,
                PortraitPositionYOverride = -1f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            Main.npcFrameCount[Type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 140;
            NPC.height = 112;
            NPC.lifeMax = 26800;
            NPC.defense = 40;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 340);
            NPC.damage = 34;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/CyberNeon");
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false; // Set to false because fuck contact damage
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            new FlavorTextBestiaryInfoElement("A machine created for defense of various civilian areas among a far away world's cities." +
            "\nYou successfully managed to interfere it's signals to call it to Terraria, congratulations on leaving a city without it's protector"),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            Dust dustId = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
            dustId.noGravity = true;
            Dust dustId3 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 60, -NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
            dustId3.noGravity = true;

            if (NPC.life <= 0)
            {
                Dust dustId2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
                dustId2.noGravity = true;
                Dust dustId4 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 60, -NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
                dustId4.noGravity = true;
            }
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(faceTarget: false);
                var target = Main.player[NPC.target];

                if (NPC.damage > 102)
                    NPC.damage = 102;

                if (target.dead || !NPC.HasValidTarget)
                {
                    NPC.netUpdate = true;

                    NPC.velocity.Y += 7f;
                    NPC.EncourageDespawn(300);
                    return;
                }
                else
                {
                    Vector2 bossToPlayer = target.Center - 350 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.05f;
                    NPC.netUpdate = true;
                }

                NPC.rotation = NPC.velocity.X * 0.0295f;

                if (AI0 <= 0 && ++AI1 >= 180)
                {
                    if (++AI2 == 120)
                    {
                        SpecialAttack();
                        AI2 = 0;
                    }
                    // Reset Attack & go back
                    if (++AI3 >= 241)
                    {
                        AI1 = 0;
                        AI2 = 0;
                        AI3 = 0;
                        AI0 = 1;
                    }
                }
                else if (AI0 == 1 && ++AI1 >= 60)
                {
                    if (++AI2 == 120)
                    {
                        HomingAttack();
                    }
                    // Reset Attack & go to next Attack
                    if (++AI3 >= 121)
                    {
                        AI1 = 0;
                        AI2 = 0;
                        AI3 = 0;
                        AI0 = 2;
                    }
                }
                else if (AI0 == 2 && ++AI1 >= 120)
                {
                    if (++AI4 == 40)
                        SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/GuttertankPrep").WithVolumeScale(1.5f), NPC.Center);
                    if (++AI2 == 60)
                    {
                        BasicAttack();
                        AI2 = 0;
                    }
                    // Reset Attack & go to next Attack
                    if (++AI3 >= 181)
                    {
                        AI1 = 0;
                        AI2 = 0;
                        AI3 = 0;
                        AI4 = 0;
                        AI0 = 0;
                    }
                }
            }
        }

        public void BasicAttack()
        {
            Player player = Main.player[NPC.target];
            Vector2 pos1 = NPC.Center - (48 * Vector2.UnitY).RotatedBy(NPC.rotation) - (70 * Vector2.UnitX).RotatedBy(NPC.rotation);
            Vector2 pos2 = NPC.Center - (48 * Vector2.UnitY).RotatedBy(NPC.rotation) + (70 * Vector2.UnitX).RotatedBy(NPC.rotation);
            Vector2 toPlayer1 = (Main.player[NPC.target].Center - pos1) * 0.02f;
            Vector2 toPlayer2 = (Main.player[NPC.target].Center - pos2) * 0.02f;
            toPlayer1.X = toPlayer1.X + (player.velocity.X * 0.1f);
            toPlayer2.X = toPlayer2.X + (player.velocity.X * 0.1f);

            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos1, toPlayer1,
                ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), (int)(NPC.damage * 0.75), 0f, Main.myPlayer);

            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, toPlayer2,
                ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), (int)(NPC.damage * 0.75), 0f, Main.myPlayer);

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(pos1, 10, 10, 60, toPlayer1.X, toPlayer1.Y, 60, default(Color), 1.5f);
                dustId.noGravity = true;

                Dust dustId3 = Dust.NewDustDirect(pos2, 10, 10, 60, toPlayer2.X, toPlayer2.Y, 60, default(Color), 1.5f);
                dustId3.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
            player.GetModPlayer<GerdPlayer>().ShakeScreen(3, 0.5f);
            NPC.netUpdate = true;
        }

        public void HomingAttack()
        {
            Player player = Main.player[NPC.target];
            Vector2 pos = NPC.Center - (30 * Vector2.UnitY).RotatedBy(NPC.rotation);
            Vector2 velocityTo = (Main.player[NPC.target].Center - pos) * 0.03f;

            for (int i = 0; i < 6; i++)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, -velocityTo.RotatedByRandom(270f), ModContent.ProjectileType<Projectiles.Bosses.AcheronOrb>(), NPC.damage, 1f, Main.myPlayer);
                player.GetModPlayer<GerdPlayer>().ShakeScreen(4, 0.6f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(pos, 0, 0, 60, velocityTo.X, velocityTo.Y, 60, default(Color), 2f);
                dustId.noGravity = true;

                Dust dustId3 = Dust.NewDustDirect(pos, 0, 0, 60, -velocityTo.X, velocityTo.Y, 60, default(Color), 2f);
                dustId3.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
            NPC.netUpdate = true;
        }

        public void SpecialAttack()
        {
            Player player = Main.player[NPC.target];
            Vector2 pos = NPC.Center - (30 * Vector2.UnitY).RotatedBy(NPC.rotation);
            Vector2 velocityTo = (Main.player[NPC.target].Center - pos) * 0.015f;
            velocityTo.X = velocityTo.X + (player.velocity.X * 0.4f);
            float numberProjectiles = 7;
            float rotation = MathHelper.ToRadians(30f);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocityTo.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, -perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 1f, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, new Vector2(perturbedSpeed.X, -perturbedSpeed.Y), ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 1f, Main.myPlayer);
                player.GetModPlayer<GerdPlayer>().ShakeScreen(3, 0.75f);
            }

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(pos, 0, 0, 60, velocityTo.X, velocityTo.Y, 60, default(Color), 2f);
                dustId.noGravity = true;

                Dust dustId3 = Dust.NewDustDirect(pos, 0, 0, 60, -velocityTo.X, velocityTo.Y, 60, default(Color), 2f);
                dustId3.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
            NPC.netUpdate = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

        }

        public override void FindFrame(int FrameHeight)
        {
            int startFrame = 0;
            int finalFrame = Main.npcFrameCount[NPC.type] - 1;

            int frameSpeed = 6; // Used to delay in frames an animation
            NPC.frameCounter += 1f; // How fast the frames are going


            if (NPC.frameCounter > frameSpeed) // When the count goes above the set speed
            {
                NPC.frameCounter = 0; // Reset
                NPC.frame.Y += FrameHeight; // Next frame

                if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past all frames
                {
                    NPC.frame.Y = startFrame * FrameHeight; // Reset to the first frame
                }
            }
        }
    }
}
