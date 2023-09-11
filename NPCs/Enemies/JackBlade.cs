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


namespace GMR.NPCs.Enemies
{
    public class JackBlade : ModNPC
    {
        public override string Texture => "GMR/Items/Weapons/Melee/JackSword";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Blade");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = false
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDirtLayerHeight && NPC.downedBoss2)
            {
                return 0.00085f; //0.085% chance of spawning on the underground after beating any evil boss
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
            new FlavorTextBestiaryInfoElement("A blade which used to be only for self defense, after an attack on it's factory of origin many units escaped." +
            "\nHowever, their cheap sensors caused multiple issues with navigation since they can't recognize what's in front of them correctly, leading to a very few remaining intact."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.lifeMax = 400;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.5f;
            NPC.damage = 28;
            NPC.aiStyle = 23;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(silver: 50);
            NPC.npcSlots = 1f;
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

            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }


            if (player.dead)
            {
                NPC.velocity.Y += 0.5f;
                NPC.EncourageDespawn(300);
                NPC.aiStyle = -1;
                return;
            }
            else
            {
                NPC.aiStyle = 23;
            }

            if (++NPC.ai[2] % 120 == 0)
            {
                const int max2 = 6;
                for (int i = 0; i < max2; i++)
                {
                    if (Main.getGoodWorld)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max2 * (i + 0.5)) * 3f, ModContent.ProjectileType<Projectiles.Bosses.JackBlastRotate>(), NPC.damage, 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max2 * (i + 0.5)) * 3f, ModContent.ProjectileType<Projectiles.Bosses.JackBlastRotateFlip>(), NPC.damage, 0f, Main.myPlayer);
                    }
                }
            }

            int dustId = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
            int dustId3 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 60, NPC.velocity.X * 0.5f,
                NPC.velocity.Y * 0.2f, 60, default(Color), 2f);
            Main.dust[dustId3].noGravity = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 1, 2, 6));
            npcLoot.Add(ItemDropRule.Common(ItemID.LeadBar, 1, 2, 6));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.ScrapFragment>(), 1, 12, 30));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.AncientBullets>(), 30));
        }
    }
}