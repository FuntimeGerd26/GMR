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
using GMR;

namespace GMR.NPCs.Enemies
{
    public class InfraRedMortar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = false
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPC.AddElement(0);
            NPC.AddElement(2);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight) && Main.hardMode)
            {
                return 0.00068f; //0.068% chance of spawning on the underground or caverns in hardmode
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
            new FlavorTextBestiaryInfoElement("These flying cannons are deadly weapons designed for large scale wars." + 
            "\nWhile Acheron was in development, multiple units were created to keep the facility secure from any intruders attempting to destroy or steal the technologies found within them." +
            "\nAlong keeping secure the facilities, they also used to test the durability of armors by firing at them with high ammounts of energy projectiles."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 66;
            NPC.lifeMax = 500;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.5f;
            NPC.damage = 28;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(silver: 25);
            NPC.npcSlots = 1f;
            NPC.ElementMultipliers([1f, 0.5f, 0.8f, 1.5f]);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            int dustType = 60;
            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
            }
        }

        bool checkPos;
        float offsetX;
        Vector2 playerOldPos;
        public override void AI()
        {
            if (NPC.life <= 0)
            {
                int dustType = 60;
                for (int i = 0; i < 30; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 30, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
                SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
            }

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Vector2 toPlayer =  player.Center - NPC.Center;
            if (++NPC.ai[0] >= 300)
            {
                if (!checkPos)
                {
                    offsetX = Main.rand.NextFloat(-400f, 400f);
                    playerOldPos = player.Center;
                    checkPos = true;
                }

                Vector2 toAtkPosition = new Vector2(playerOldPos.X + offsetX, playerOldPos.Y - 150f);
                NPC.rotation = Vector2.Lerp(Vector2.UnitX.RotatedBy(NPC.rotation), Vector2.UnitX.RotatedBy(-0f), 0.25f).ToRotation();
                NPC.velocity = Vector2.Lerp(NPC.velocity, toAtkPosition - NPC.Center, 0.2f);
                NPC.defense = 15;
            }
            else
            {
                NPC.rotation = Vector2.Lerp(Vector2.UnitX.RotatedBy(NPC.rotation), Vector2.UnitX.RotatedBy(toPlayer.ToRotation() + MathHelper.ToRadians(90f)), 0.15f).ToRotation();
                NPC.velocity = ((player.Center - Vector2.UnitY * 100) - NPC.Center) * 0.03f;
                NPC.defense = 5;
                checkPos = false;
            }

            if (player.dead)
            {
                NPC.velocity.Y -= 0.25f;
                NPC.EncourageDespawn(120);
                return;
            }
            else
            {
                if (++NPC.ai[0] >= 420 && ++NPC.ai[2] % 2 == 0)
                {
                    Vector2 velocity = Vector2.UnitY * -8f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center - (Vector2.UnitY * NPC.height).RotatedBy(NPC.rotation),
                        velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-12f, 12f))), ModContent.ProjectileType<Projectiles.Bosses.InfraRedBlastFall>(), NPC.damage, 0f, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);

                    NPC.velocity += Vector2.UnitY * 0.4f;
                    if (++NPC.ai[0] >= 480)
                        NPC.ai[0] = 0;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.InfraRedBar>(), 1, 6, 20));
        }
    }
}