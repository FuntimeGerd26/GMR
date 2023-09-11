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
using GMR.NPCs.Bosses.Jack;

namespace GMR.NPCs.Bosses.Jack.Eternity
{
    [AutoloadBossHead()]
    public class JackE : ModNPC
    {
        public bool PlayAnimation;
        public bool PlayerDead;
        public bool HalfHealthSummon;
        public bool Alivent;
        public int ExplodeTimer;
        public int ShouldDie;
        public int AI4;
        public int AI5;
        public int AI6;

        private const float DEATHTIME = MathHelper.PiOver4 * 134;

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

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Hide = true, });
        }

        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 122;
            NPC.lifeMax = 8250;
            NPC.defense = 25;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.damage = 18;
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
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            new FlavorTextBestiaryInfoElement("An ancient machine that was never finished, it has a really bad temper for this reason. It launched an attack on it's creators for never bothring finishing it. Consuming souls causes it to become more powerful for following fights, making it a threat possibly as dangerous as the mechanical bosses."),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int dustType = 6;
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 10, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 10, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

                    dust.noLight = false;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
                }
            }
        }

        public override bool CheckDead()
        {
            if (NPC.ai[0] == -3f)
            {
                NPC.lifeMax = -33333;
                return true;
            }
            NPC.ai[0] = -3f;
            NPC.velocity = new Vector2(0f, 0f);
            NPC.dontTakeDamage = true;
            NPC.life = NPC.lifeMax;
            Alivent = true;
            return false;
        }

        public void Die()
        {
            NPC.ai[1] += 0.5f;

            if (NPC.ai[1] > DEATHTIME * 1.314f)
            {
                NPC.life = -33333;
                NPC.HitEffect();
                CheckDead();
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

            if (NPC.life <= 1)
            {
                Die();
            }

            if (Alivent)
            {
                if (++ShouldDie == 180)
                    NPC.life = -33333;

                if (++ExplodeTimer % 15 == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + Main.rand.Next(-NPC.width / 2, NPC.width / 2), NPC.Center.Y + Main.rand.Next(-NPC.height / 2, NPC.height / 2)),
                        Vector2.Zero, ModContent.ProjectileType<Projectiles.SmallExplotion>(), 0, 0f, Main.myPlayer, NPC.whoAmI);
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
            if (!Alivent && !NPC.AnyNPCs(ModContent.NPCType<JackEArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackEArmClaw>()) || NPC.life < (int)(NPC.lifeMax * 0.1))
            {
                NPC.dontTakeDamage = false;
                NPC.netUpdate = true;
            }
            else
            {
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
            }

            if (!Alivent && NPC.dontTakeDamage == false) // If it's vulnerable
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
            if (!Alivent && NPC.ai[2] == 0)
            {
                NPC.ai[2]++;
                int count = 1;
                int spawnX = (int)player.position.X + player.width / 2;
                int spawnY = (int)player.position.Y + player.height / 2;
                for (int i = 0; i < count; i++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackEArmGun>(), NPC.whoAmI, 0f, 0f, 5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackEArmClaw>(), NPC.whoAmI, 0f, 0f, 5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackEArmGun>(), NPC.whoAmI, 0f, 0f, -5f);
                    NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackEArmClaw>(), NPC.whoAmI, 0f, 0f, -5f);
                }
                NPC.netUpdate = true;
            }

            if (player.dead)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y += 7f;
                NPC.EncourageDespawn(300);
                if (!PlayerDead)
                {
                        Main.NewText("OBJECTIVE DOWNED", Color.Red);
                        PlayerDead = true;
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

            if (!Alivent && NPC.dontTakeDamage == false && ++NPC.ai[2] % 480 == 0 || NPC.life < NPC.lifeMax / 2 && ++NPC.ai[2] % 480 == 0)
            {
                SideAttack();
                NPC.netUpdate = true;
            }

            if (!Alivent && NPC.life < (int)(NPC.lifeMax * 0.75) && Main.expertMode && ++AI6 % 480 == 0)
            {
                RainAttack();
                NPC.netUpdate = true;
            }

            if (!Alivent && NPC.life < (int)(NPC.lifeMax * 0.35) && ++AI4 % 600 == 0)
            {
                WallAttack();
                NPC.netUpdate = true;
            }

            if (!Alivent && NPC.dontTakeDamage == false && NPC.life > (int)(NPC.lifeMax * 0.1) && ++AI5 % 240 == 0)
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
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.GildedMetalSword>(), 3));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.JuiceBox>(), 5, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.LeadBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.TungstenBar, 2, 5, 16));
            npcLoot.Add(ItemDropRule.Common(ItemID.SilverBar, 2, 5, 16));
        }

        public override void OnKill()
        {
            GerdWorld.downedJack = true;

            int dustType = 64;
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 10, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;

            var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/Eternity/JackE_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
