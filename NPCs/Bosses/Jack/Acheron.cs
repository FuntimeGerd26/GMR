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

namespace GMR.NPCs.Bosses.Jack
{
    [AutoloadBossHead()]
    public class Acheron : ModNPC
    {
        public bool PlayerDead;
        public int NoArms;
        public int AI4;

        public static int MinionType()
        {
            return ModContent.NPCType<AcheronArmGun>();
            return ModContent.NPCType<AcheronArmClaw>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acheron");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
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
            NPC.lifeMax = 15500;
            NPC.defense = 75;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0.2f;
            NPC.damage = 28;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(gold: 6);
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
            new FlavorTextBestiaryInfoElement("A finished model of the Jack Prototype, it had quite some time to master it's skills. However these skills weren't enough to stop the attack caused by the Jack Prototype on the factory it was made in." +
            "\nAfter the attack it has held quite a rivalry with Jack"),
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
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];

                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }

                if (NPC.damage > 56)
                    NPC.damage = 56;

                if (player.dead)
                {
                    NPC.netUpdate = true;

                    NPC.velocity.Y += 7f;
                    NPC.EncourageDespawn(300);
                    if (!PlayerDead)
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Special.ShapeShifter>()))
                        {
                            NPC.NewNPC(new EntitySource_Parent(NPC), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<NPCs.Special.ShapeShifter>());
                        }

                        Main.NewText("Good luck next time", Color.Red);
                        PlayerDead = true;
                    }
                    return;
                }
                else if (NPC.ai[0] == 0 || NPC.ai[0] == 1 || NPC.ai[0] == 4 || NPC.ai[0] == 6)
                    NPC.velocity *= 0f;
                else
                {
                    // Where's the boss going
                    Vector2 bossToPlayer = Main.player[NPC.target].Center - 250 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.005f;
                }

                if (!NPC.AnyNPCs(ModContent.NPCType<AcheronArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<AcheronArmClaw>()))
                {
                    NPC.defense = 5;
                    NoArms--;
                }

                if (NoArms < 0)
                {
                    NoArms = 600;
                    int spawnX = (int)player.position.X + player.width / 2;
                    int spawnY = (int)player.position.Y + player.height / 2;
                    for (int i = 0; i < 1; i++)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Acheron"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<AcheronArmGun>(), NPC.whoAmI, 0f, 0f, 5f);
                        NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Acheron"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<AcheronArmClaw>(), NPC.whoAmI, 0f, 0f, 5f);
                        NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Acheron"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<AcheronArmGun>(), NPC.whoAmI, 0f, 0f, -5f);
                        NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Acheron"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<AcheronArmClaw>(), NPC.whoAmI, 0f, 0f, -5f);
                        SoundEngine.PlaySound(SoundID.Research, NPC.Center);
                    }

                    NPC.defense = 75;
                    NPC.netUpdate = true;
                }


                switch ((int)NPC.ai[0])
                {
                    case 0: // Heal for skill issue
                        if (++NPC.ai[2] % 30 == 0 && (NPC.AnyNPCs(ModContent.NPCType<AcheronArmGun>()) || NPC.AnyNPCs(ModContent.NPCType<AcheronArmClaw>())))
                        {
                            int lifeHeal = NPC.lifeMax / 10;
                            if (NPC.life > (int)(NPC.lifeMax * 0.9))
                            {
                                lifeHeal = NPC.lifeMax - NPC.life;
                                NPC.life += lifeHeal;
                            }
                            else if (NPC.life < NPC.lifeMax)
                                NPC.life += lifeHeal;

                            Rectangle displayPoint = new Rectangle(NPC.Hitbox.Center.X, NPC.Hitbox.Center.Y - NPC.height / 4, 2, 2);
                            CombatText.NewText(displayPoint, Color.Lime, $"{lifeHeal}");
                        }
                        if (++NPC.ai[1] == 301)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    case 1: // Spining Projectiles
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[2] % 10 == 0)
                        {
                            NPC.netUpdate = true;

                            float angleRotate = 0;
                            angleRotate += 4f * (NPC.ai[1] / 8);
                            Vector2 velocity = new Vector2(0f, 4f);
                            for (int i = 0; i < 1; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(angleRotate)), ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 801)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    case 2: // Rune Beam
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[2] == 120)
                        {
                            NPC.netUpdate = true;

                            for (int i = 0; i < 1; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.AcheronShootingRune>(), NPC.damage * 2, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 401)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    case 3: // Spin Saws
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[2] % 200 == 0)
                        {
                            NPC.netUpdate = true;

                            int numberProjectiles = 4;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / numberProjectiles * i) * 8f,
                                    ModContent.ProjectileType<Projectiles.Bosses.JackBlastRotateFlip>(), NPC.damage, 0f, Main.myPlayer);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / numberProjectiles * i) * 8f,
                                    ModContent.ProjectileType<Projectiles.Bosses.JackBlastRotate>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item23, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 801) // If you're wondering why these have an extra 1, idk i think it makes stuff work one last time
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    case 4: // Reverse Spinning Projectiles
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[2] % 10 == 0)
                        {
                            NPC.netUpdate = true;

                            float angleRotate = 0;
                            angleRotate -= 4f * (NPC.ai[1] / 8);
                            Vector2 velocity = new Vector2(0f, 4f);
                            for (int i = 0; i < 1; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(angleRotate)), ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 801)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    case 5: // Jack Rain Attack is back
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[2] % 400 == 0)
                        {
                            NPC.netUpdate = true;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.JackRain>(), NPC.damage * 2, 0f, Main.myPlayer);
                            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                        }
                        if (++NPC.ai[1] > 801)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;


                    case 6: // Homing Orbs
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        if (++NPC.ai[1] < 600 && ++NPC.ai[2] % 5 == 0)
                        {
                            NPC.netUpdate = true;

                            float angleRotate = 0;
                            angleRotate -= 4f * (NPC.ai[1] / 8);
                            Vector2 velocity = new Vector2(0f, 8f);
                            for (int i = 0; i < 1; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 5f, ModContent.ProjectileType<Projectiles.Bosses.AcheronOrb>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 801)
                        {
                            AI4 = 0;
                            NPC.TargetClosest();
                            NPC.ai[0]++;
                            NPC.ai[1] = 0;
                            NPC.ai[2] = 0;
                            NPC.localAI[0] = 0;
                            NPC.netUpdate = true;
                        }
                        break;

                    default:
                        NPC.ai[0] = 0;
                        goto case 0;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.ScrapFragment>(), 1, 14, 30));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Consumable.DGPCrate>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.JackRifle>(), 50));

            int[] drops = { ModContent.ItemType<Items.Weapons.Melee.InfraRedSpear>(), ModContent.ItemType<Items.Weapons.Ranged.AcheronBow>(), ModContent.ItemType<Items.Weapons.Magic.InfraRedStaff>(), };

            npcLoot.Add(ItemDropRule.OneFromOptions(1, drops));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.JackMask>(), 10));
            if (Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.JackExpert>(), 1));
            }

            npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.LeadBar, 1, 7, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.TungstenBar, 3, 5, 16));
            npcLoot.Add(ItemDropRule.Common(ItemID.SilverBar, 3, 5, 16));
            npcLoot.Add(ItemDropRule.Common(ItemID.GoldBar, 4, 4, 18));
            npcLoot.Add(ItemDropRule.Common(ItemID.PlatinumBar, 4, 4, 18));
        }

        public override void OnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Special.ShapeShifter>()))
            {
                NPC.NewNPC(new EntitySource_Parent(NPC), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<NPCs.Special.ShapeShifter>());
            }

            GerdWorld.downedAcheron = true;

            int dustType = 60;
            for (int i = 0; i < 60; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 60, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
            }
        }

        private float runeRotate;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            var rune = GMR.Instance.Assets.Request<Texture2D>("Assets/Images/JackRitual", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/Acheron_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var shield = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/AcheronShields", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var shieldglow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/AcheronShields_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Color drawColor2 = new Color(255, 155, 155, 55);
            var origin2 = new Vector2(rune.Width / 2f, rune.Height / 2f);
            var originshields = new Vector2(shield.Width / 2f, shield.Height / 2f);
            runeRotate += 0.015f;
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f - 1f / trailLength * i;
                Main.EntitySpriteDraw(rune, NPC.oldPos[i] + offset, null, drawColor2 * progress, runeRotate, origin2, NPC.scale * 0.75f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(rune, NPC.position + offset, null, drawColor2, runeRotate, origin2, NPC.scale * 0.75f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(shield, NPC.position + offset, null, drawColor, runeRotate, originshields, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(shieldglow, NPC.position + offset, null, drawColor2, runeRotate, originshields, NPC.scale, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}

