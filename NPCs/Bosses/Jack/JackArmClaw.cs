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

namespace GMR.NPCs.Bosses.Jack
{
    public class JackArmClaw : ModNPC
    {
        // This is a neat trick that uses the fact that NPCs have all NPC.ai[] values set to 0f on spawn (if not otherwise changed).
        // We set ParentIndex to a number in the body after spawning it. If we set ParentIndex to 3, NPC.ai[0] will be 4. If NPC.ai[0] is 0, ParentIndex will be -1.
        // Now combine both facts, and the conclusion is that if this NPC spawns by other means (not from the body), ParentIndex will be -1, allowing us to distinguish
        // between a proper spawn and an invalid/"cheated" spawn
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
            DisplayName.SetDefault("Jack Claw");
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
            NPC.width = 32;
            NPC.height = 86;
            NPC.lifeMax = 9000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 10;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.npcSlots = 1f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            Player player = Main.player[NPC.target];
            NPC.lifeMax = (int)(NPC.lifeMax * 1f);
            NPC.lifeMax += numPlayers * 100;
        }

        public override void AI()
        {
            NPC.dontTakeDamage = true;

            if (!NPC.AnyNPCs(ModContent.NPCType<Jack>()))
            {
                NPC.life += -250;
                float ai1 = Main.rand.Next(20, 30);
                Vector2 speed = -Vector2.UnitY.RotatedByRandom(Math.PI / 2) * Main.rand.NextFloat(2f, 6f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, speed, ModContent.ProjectileType<Projectiles.Bosses.AttackPreview>(), 0, 1f, Main.myPlayer, ai1);
                NPC.alpha = 0;

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

            NPC.rotation = MathHelper.ToRadians(-90f);

            if (player.dead)
            {
                NPC.velocity.Y += 0.5f;
                NPC.EncourageDespawn(300);
                return;
            }

            if (++NPC.localAI[0] > 300)
            {
                NPC.localAI[0] = 0;
            }
            else if (++NPC.localAI[0] > 236)
            {

            }
            else if (++NPC.localAI[0] > 235)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(25f, 0f), ModContent.ProjectileType<Projectiles.Bosses.JackArmClaw>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.alpha = 225;
            }
            else if (++NPC.localAI[0] > 125)
            {

            }
            else if (++NPC.localAI[0] > 120)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(25f, 0f), ModContent.ProjectileType<Projectiles.Bosses.AttackPreview>(), 0, 1f, Main.myPlayer, NPC.whoAmI);
            }
            else if (++NPC.localAI[0] > 0)
            {
                NPC.Center = Main.player[NPC.target].Center - 600 * Vector2.UnitX;
                NPC.alpha = 0;
            }
        }
    }
}