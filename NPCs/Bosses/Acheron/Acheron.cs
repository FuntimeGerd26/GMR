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

#region Mod Items

using GMR.Items.Misc;
using GMR.Items.Misc.Materials;
using GMR.Items.Misc.Consumable;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Melee.Swords;
using GMR.Items.Weapons.Melee.Spears;
using GMR.Items.Weapons.Melee.Others;
using GMR.Items.Weapons.Ranged;
using GMR.Items.Weapons.Ranged.Bows;
using GMR.Items.Weapons.Ranged.Guns;
using GMR.Items.Weapons.Ranged.Others;
using GMR.Items.Weapons.Ranged.Railcannons;
using GMR.Items.Weapons.Magic;
using GMR.Items.Weapons.Magic.Books;
using GMR.Items.Weapons.Magic.Staffs;
using GMR.Items.Weapons.Magic.Others;

#endregion

namespace GMR.NPCs.Bosses.Acheron
{
    [AutoloadBossHead()]
    public class Acheron : ModNPC
    {
        public bool PlayerDead;
        public int NoArms;
        public int AI4;
        public int AI5;

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
            NPC.AddElement(0);
            NPC.AddElement(2);
        }

        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 122;
            NPC.lifeMax = 12000;
            NPC.defense = 5;
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

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.5f * (3 - balance) + (numPlayers * 0.1f)));
            NPC.damage = (int)(NPC.damage * (0.5f * (3 - balance)));
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, new Vector3(0.8f, 0.15f, 0.5f));

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];

                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }

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
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.025f;
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

                    NPC.defense = 30;
                    NPC.netUpdate = true;
                }


                switch ((int)NPC.ai[0])
                {
                    case -1: // Just a loading thing
                        if (++NPC.ai[1] == 180)
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
                    case 0: // Heal for skill issue
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
                        }
                        /*if (++NPC.ai[2] % 5 == 0)
                        {
                            float xFloat = Main.rand.NextFloat(-1500f, 1500f);
                            int numberProjectiles = 4;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - 1000 * Vector2.UnitY + (xFloat * Vector2.UnitX), new Vector2(0f, 3f * (i + 1)),
                                    ModContent.ProjectileType<Projectiles.Bosses.AncientBulletHostile>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            }
                            NPC.netUpdate = true;
                        }*/
                        if (++NPC.ai[2] % 30 == 0 && (NPC.AnyNPCs(ModContent.NPCType<AcheronArmGun>()) || NPC.AnyNPCs(ModContent.NPCType<AcheronArmClaw>())))
                        {
                            int lifeHeal = NPC.lifeMax / 50;
                            {
                                if (NPC.life > (int)(NPC.lifeMax * 0.98f))
                                {
                                    lifeHeal = NPC.lifeMax - NPC.life;
                                    NPC.life += lifeHeal;
                                }
                                else if (NPC.life < NPC.lifeMax)
                                    NPC.life += lifeHeal;

                                Rectangle displayPoint = new Rectangle(NPC.Hitbox.Center.X, NPC.Hitbox.Center.Y - NPC.height / 4, 2, 2);
                                CombatText.NewText(displayPoint, Color.Lime, $"{lifeHeal}");

                                NPC.netUpdate = true;
                            }
                        }
                        if (++NPC.ai[1] > 301)
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
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
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
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
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
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
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
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
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

                    case 5: // Jack Rain & Wall Attack
                        if (++NPC.ai[1] == 1)
                        {
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);

                            player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.50f);
                            float x = 1000f;
                            float y = 2000f;
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

                            //Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.JackRain>(), NPC.damage, 0f, Main.myPlayer);
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
                            SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream").WithVolumeScale(2f), NPC.Center);
                        }
                        if (++NPC.ai[1] < 600 && ++NPC.ai[2] % 10 == 0)
                        {
                            NPC.netUpdate = true;

                            float angleRotate = 0;
                            angleRotate -= 4f * (NPC.ai[1] / 8);
                            Vector2 velocity = new Vector2(0f, 8f);
                            for (int i = 0; i < 1; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - 8 * Vector2.UnitY,
                                    NPC.DirectionTo(player.Center) * 8f, ModContent.ProjectileType<Projectiles.Bosses.AcheronOrb>(), NPC.damage, 0f, Main.myPlayer);
                                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            }
                        }
                        if (++NPC.ai[1] > 800)
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
                        NPC.ai[0] = -1;
                        goto case -1;
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

            int[] drops = { ModContent.ItemType<InfraRedSpear>(), ModContent.ItemType<AcheronBow>(), ModContent.ItemType<InfraRedStaff>(), };

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, drops));

            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<JackRifle>(), 50));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InfraRedBar>(), 1, 18, 45));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InfraRedCrystalShard>(), 2, 8, 20));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DGPCrate>(), 10));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.JackMask>(), 10));

            if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && !GerdWorld.downedAcheron)
            {
                npcLoot.Add(ItemDropRule.Common(magicStorage.Find<ModItem>("ShadowDiamond").Type));
            }

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AcheronTreasureBag>()));
            npcLoot.Add(notExpertRule);
        }

        public override void OnKill()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.Special.ShapeShifter>()))
            {
                NPC.NewNPC(new EntitySource_Parent(NPC), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<NPCs.Special.ShapeShifter>());
            }

            GerdWorld.downedAcheron = true;
            NPC.netUpdate = true;

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
            var star = GMR.Instance.Assets.Request<Texture2D>("Assets/Images/Star08", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Acheron/Acheron_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var shield = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Acheron/AcheronShields", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var shieldglow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Acheron/AcheronShields_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Color drawColor2 = new Color(194, 91, 112, 5);
            Color drawColor3 = new Color(255, 55, 85, 5);
            var origin2 = new Vector2(rune.Width / 2f, rune.Height / 2f);
            var originstar = new Vector2(star.Width / 2f, star.Height / 2f);
            var originshields = new Vector2(shield.Width / 2f, shield.Height / 2f);
            runeRotate += 0.015f;
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f - 1f / trailLength * i;
                Main.EntitySpriteDraw(rune, NPC.oldPos[i] + offset, null, drawColor3 * progress, runeRotate - MathHelper.ToRadians(1f), origin2, NPC.scale * 0.75f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(rune, NPC.position + offset, null, drawColor3, runeRotate, origin2, NPC.scale * 0.75f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(shield, NPC.position + offset, null, drawColor, runeRotate, originshields, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(shieldglow, NPC.position + offset, null, drawColor2, runeRotate, originshields, NPC.scale, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, drawColor2, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(star, NPC.position + offset - 8 * Vector2.UnitY, null, drawColor3 * 0.5f, runeRotate, originstar, NPC.scale * 0.125f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(star, NPC.position + offset + 26 * Vector2.UnitY, null, drawColor3 * 0.6f, runeRotate, originstar, NPC.scale * 0.15f, SpriteEffects.None, 0);
            return false;
        }
    }
}

