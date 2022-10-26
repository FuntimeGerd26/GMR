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
    public class JackArmGunFlip : ModNPC
    {
        public int ParentIndex
        {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        // Helper method to determine the body type
        public static int BodyType()
        {
            return ModContent.NPCType<Jack>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jack Cannon");
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
            NPC.lifeMax = 6000;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 4;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.npcSlots = 1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackRifle>(), 30));
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            Player player = Main.player[NPC.target];
            NPC.lifeMax = (int)(NPC.lifeMax * 1f) + 5;
            NPC.lifeMax = NPC.lifeMax / 2 * numPlayers;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<Jack>()))
            {
                NPC.life += -250;

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
                return;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()))
            {
                NPC.life += -250;
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
                return;
            }

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            NPC.rotation = MathHelper.ToRadians(45f);

            if (Main.dayTime || player.dead)
            {
                NPC.velocity.Y += 0.5f;
                NPC.EncourageDespawn(300);
                return;
            }
            else
            {
                NPC.Center = Main.player[NPC.target].Center + 360 * Vector2.UnitX - 320 * Vector2.UnitY;
            }
            if (++NPC.ai[1] > 180) //Each 3 seconds run
            {
                if (++NPC.ai[2] > 120) //after 2 seconds from start
                {
                    if (++NPC.ai[0] > 31) //resets all
                    {
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                    }
                    else if (++NPC.ai[0] > 30) //2.5 seconds after start, shoot
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(-6f, 4f), Main.rand.Next(new int[] { ModContent.ProjectileType<Projectiles.Bosses.AlloyCrate>(), ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>()}), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                    }
                }
                else if (++NPC.ai[2] > 60) //Dust after 1 second from start
                {
                    int dustType = 60;
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                        Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 30, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                        dust.noLight = false;
                        dust.noGravity = true;
                        dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                    }
                }
            }
        }
    }
}