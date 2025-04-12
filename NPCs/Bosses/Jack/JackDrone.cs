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
    public class JackDrone : ModNPC
    {
        public bool SummonedBlades;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Drone");
            // Since this is more of a miniboss it does not have a check for if it's a boss
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 1f,
                PortraitPositionYOverride = 1.5f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPC.AddElement(0);
            NPC.AddElement(2);
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.lifeMax = 1500;
            NPC.defense = 8;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 30;
            NPC.aiStyle = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.npcSlots = 1f;
            NPC.ElementMultipliers([1f, 0.5f, 0.8f, 1.5f]);
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
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            new FlavorTextBestiaryInfoElement("A device used to create multiple weapons and tools, these were old prototypes to other cheaper and much easier to make versions, however the ones of such quality were destroyed in an attack"),
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

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.5f * (3 - balance) + (numPlayers * 0.1f)));
            NPC.damage = (int)(NPC.damage * (0.5f * (3 - balance)));
        }

        float rotate;
        public override void AI()
        {
            Player player = Main.player[NPC.target];

            rotate += 0.5f;
            Vector2 Pos = Vector2.Lerp(NPC.Center, Main.player[NPC.target].Center - (250 * Vector2.UnitY).RotatedBy(MathHelper.ToRadians(rotate)), 0.15f);
            NPC.Center = Pos;

            Vector2 toPlayer = NPC.Center - player.Center;
            NPC.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(90f);


            if (++NPC.ai[1] % 120 == 0)
            {
                NPC.netUpdate = true;
                SpawnDust();
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 6f,
                    ModContent.ProjectileType<Projectiles.Bosses.AlloyCrate>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Research, NPC.Center);
            }

            if (++NPC.ai[2] % 600 == 0)
            {
                NPC.netUpdate = true;
                SpawnDust();
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / 4 * (i + 0.5)) * 2f, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 0f, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(45f) + 2 * Math.PI / 4 * (i + 0.5)) * 1f,
                        ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 0f, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                }
            }
        }

        private void SpawnDust()
        {
            int dustType = 60;
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
            }
        }
    }
}
