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

namespace GMR.NPCs.Bosses.MagmaEye
{
    [AutoloadBossHead()]
    public class MagmaEye : ModNPC
    {
        public float AI4;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 9;
            NPCID.Sets.TrailingMode[NPC.type] = 7;

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
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 80;
            NPC.lifeMax = 4150;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath10;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 4);
            NPC.damage = 30;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/ExolThemeOld");
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
            new FlavorTextBestiaryInfoElement("An entity born from perfectly positioned rocks and lava, it was worshipped as a statue to an unknown god supposedly in charge of the infinite heat of the underworld."),
            });
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }

        public override void AI()
        {
            if (NPC.ai[0] == -1f)
            {
                NPC.velocity.X *= 1f;
                NPC.velocity.Y += 1f;
                if (NPC.timeLeft > 120)
                {
                    NPC.timeLeft = 120;
                }
                return;
            }

            if (NPC.damage > 56)
                NPC.damage = 56;

            var center = NPC.Center;
            float lifePercentage = NPC.life / (float)NPC.lifeMax;
            if (lifePercentage > 0.8f && !Main.masterMode) // Fakeout Phase
            {
                NPC.TargetClosest(faceTarget: false);
                if (!NPC.HasValidTarget)
                {
                    AI4 = -1f;
                    return;
                }

                var target = Main.player[NPC.target];
                Vector2 gotoPosition;
                if (NPC.ai[2] > 400f)
                {
                    gotoPosition = new Vector2(target.position.X + target.width / 2f, target.position.Y + target.height / 2f);
                    if (NPC.ai[2] > 600f)
                    {
                        NPC.ai[2] = 0f;
                    }
                }
                else
                {
                    gotoPosition = new Vector2(target.position.X + target.width / 2f, target.position.Y - NPC.height * 3f);
                }
                if ((center - gotoPosition).Length() > 30f)
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(gotoPosition - center) * 12f, 0.0137f);
                }

                NPC.rotation = NPC.velocity.X * 0.085f;

                if (lifePercentage < 0.95f)
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 150f * (lifePercentage * lifePercentage))
                    {
                        if ((int)NPC.ai[1] % 20 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item45, center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), center, Vector2.Normalize(center - target.Center).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 11f,
                                    ModContent.ProjectileType<BackFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[1] > 180f)
                        {
                            NPC.ai[1] = 0f;
                        }
                    }
                }
            }
            else // Full Boss
            {
                NPC.TargetClosest(faceTarget: false);
                if (!NPC.HasValidTarget)
                {
                    AI4 = -1f;
                    return;
                }
                var target = Main.player[NPC.target];
                NPC.rotation = AI4 == 6 ? NPC.rotation + 0.45f : NPC.velocity.X * 0.0314f;

                var gotoPosition = new Vector2(0f, NPC.height * 2.5f);
                gotoPosition = target.position + new Vector2(target.width / 2f, 0f) + gotoPosition.RotatedBy(NPC.ai[2]);
                NPC.ai[2] += 0.08f;
                NPC.ai[2] %= MathHelper.TwoPi;
                if ((center - gotoPosition).Length() > 30f)
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(gotoPosition - center) * 12f, 0.0137f);
                }

                if (AI4 == 0) // Basic AI (Homing Fireballs)
                {
                    NPC.rotation = NPC.velocity.X * 0.0314f;
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 150f * (lifePercentage * lifePercentage))
                    {
                        if ((int)NPC.ai[1] % 40 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item45, center);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), center, Vector2.Normalize(center - target.Center).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 11f,
                                    ModContent.ProjectileType<BackFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                        if (NPC.ai[1] > 180f)
                        {
                            NPC.ai[1] = 0f;
                            if (Main.rand.NextBool())
                            {
                                AI4++;
                                NPC.ai[1] = 0f;
                            }
                        }
                    }
                }
                else if ((int)AI4 == 1) //Horizontal Wall
                {
                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                            float x = 1000f;
                            float y = 1500f;
                            int amount = 20;
                            var posX = target.position.X + target.width / 2f;
                            var posY = target.position.Y + target.height / 2f - y / 2f;
                            float yAdd = y / (amount / 2);
                            int type = Main.masterMode ? ModContent.ProjectileType<MagmaBigSword>() : ModContent.ProjectileType<MagmaSword>();
                            for (int i = 0; i < amount; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + x, posY + yAdd * i), new Vector2(-18f, 0f), type, NPC.damage, 1f, Main.myPlayer);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX - x, posY + yAdd * i), new Vector2(18f, 0f), type, NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 300f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                    }
                }
                else if ((int)AI4 == 2) // Sword "Ring" (Any attack with ring in the name just means it's many evenly spread projectiles around the boss)
                {
                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                            int max = 16;
                            for (int i = 0; i < max; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 12f,
                                ModContent.ProjectileType<MagmaBigSword>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 180f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                    }
                }
                else if ((int)AI4 == 3) // Homing Fireballs Ring
                {
                    NPC.velocity *= 0.0137f;
                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                            SoundEngine.PlaySound(SoundID.Item45, center);
                            int max = 8;
                            for (int i = 0; i < max; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 18f,
                                ModContent.ProjectileType<BackFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 240f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                    }
                }
                else if ((int)AI4 == 4) // Vertical Walls
                {
                    NPC.rotation = NPC.velocity.X * 0.0314f;
                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                            float x = 1500f;
                            float y = 1000f;
                            int amount = 20;
                            var posX = target.position.X + target.width / 2f - x / 2f;
                            var posY = target.position.Y + target.height / 2f;
                            float xAdd = x / (amount / 2);
                            int type = Main.masterMode ? ModContent.ProjectileType<MagmaSword>() : ModContent.ProjectileType<MagmaBigSword>();
                            for (int i = 0; i < amount; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + xAdd * i, posY + y), new Vector2(0f, -18f), type, NPC.damage, 1f, Main.myPlayer);
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + xAdd * i, posY - y), new Vector2(0f, 18f), type, NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 180f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                    }
                }
                else if ((int)AI4 == 5) // Above Player, Rain projectiles
                {
                    NPC.Center = target.Center - 300 * Vector2.UnitY;

                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                        }
                    }

                    if (++NPC.ai[3] % 30 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item45, center);

                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(15);
                            Vector2 velocity = Vector2.Normalize(center - target.Center).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 11f;
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), center, perturbedSpeed, ModContent.ProjectileType<FallFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 300f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                        NPC.ai[3] = 0f;
                    }
                }
                else if ((int)AI4 == 6) // Chase Player, Shoot homing fireballs in expert
                {
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(target.Center - center) * 6f, 0.075f);
                    NPC.dontTakeDamage = true;

                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                        }
                    }

                    if (++NPC.ai[3] % 40 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item45, center);

                            float numberProjectiles = 5;
                            float rotation = MathHelper.ToRadians(15);
                            Vector2 velocity = new Vector2(NPC.velocity.X * 2f, -9f);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), center, perturbedSpeed, ModContent.ProjectileType<FallFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }

                            if (Main.expertMode)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), center, Vector2.Normalize(center - target.Center).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 11f,
                                    ModContent.ProjectileType<BackFireball>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 240f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                        NPC.ai[3] = 0f;
                    }
                }
                else if ((int)AI4 == 7) // Basic AI (Homing Fireballs)
                {
                    NPC.dontTakeDamage = false;

                    if (NPC.ai[1] == 0f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                        }
                    }

                    if (++NPC.ai[3] % 40 == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item45, center);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), center, Vector2.Normalize(center - target.Center).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 11f,
                                ModContent.ProjectileType<BackFireball>(), NPC.damage, 1f, Main.myPlayer);
                        }
                    }

                    if (++NPC.ai[1] > 200f)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                        NPC.ai[3] = 0f;
                    }
                }
                else if ((int)AI4 == 8) // Swords Ring
                {
                    NPC.Center = center;
                    NPC.velocity *= 0.0137f;

                    if (NPC.ai[1] == 15f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                        }
                    }

                    if (++NPC.ai[3] == 30f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Item45, center);

                            int max = 8;
                            int max2 = 16;
                            for (int i = 0; i < max; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 14f,
                                ModContent.ProjectileType<MagmaGreatSword>(), NPC.damage, 1f, Main.myPlayer);
                            }
                            for (int y = 0; y < max2; y++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max2 * (y + 0.5)) * 18f,
                                ModContent.ProjectileType<MagmaSword>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 240f || !Main.masterMode)
                    {
                        AI4++;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                    }
                }
                else // Big Spinning Sword Ring
                {
                    NPC.rotation = NPC.velocity.X * 0.0314f;
                    NPC.velocity *= 0.0137f;
                    if (NPC.ai[1] == 5f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            SoundEngine.PlaySound(SoundID.Roar, center);
                            int max = 8;
                            int max2 = 16;
                            for (int i = 0; i < max; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 12f,
                                ModContent.ProjectileType<MagmaBigSword>(), NPC.damage, 1f, Main.myPlayer);
                            }
                            for (int y = 0; y < max2; y++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max2 * (y + 0.5)) * 18f,
                                ModContent.ProjectileType<MagmaSword>(), NPC.damage, 1f, Main.myPlayer);
                            }
                        }
                    }
                    NPC.ai[1]++;
                    if (NPC.ai[1] > 240f || Main.masterMode)
                    {
                        AI4 = 0f;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                    }
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Consumable.DGPCrate>(), 20));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.ExolBlade>(), 10));
            int[] drops = { ModContent.ItemType<Items.Weapons.Melee.BrokenMagmaticSword>(), ModContent.ItemType<Items.Weapons.Ranged.Magmathrower>(), ModContent.ItemType<Items.Weapons.Ranged.MagmaKnife>(),
                ModContent.ItemType<Items.Weapons.Magic.MagmaStaff>() };

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, drops));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.Hellstone, 1, 20, 38));

            if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && !GerdWorld.downedMagmaEye)
            {
                npcLoot.Add(ItemDropRule.Common(magicStorage.Find<ModItem>("ShadowDiamond").Type));
            }

            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Misc.Consumable.MagmaEyeTreasureBag>()));
            npcLoot.Add(notExpertRule);
        }

        public override void OnKill()
        {
            GerdWorld.downedMagmaEye = true;

            int dustType = 60;
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 64, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

                dust.noLight = false;
                dust.noGravity = true;
                dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
            }
        }

        private float _glowmaskIntensity = 0.25f;
        private readonly byte _glowmaskAlpha = 128;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];
            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            texture = GMR.Instance.Assets.Request<Texture2D>("NPCs/Bosses/MagmaEye/MagmaEyeGlowmask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            drawColor = new Color(255, 255, 255, _glowmaskAlpha);
            float lifePercentage = NPC.life / (float)NPC.lifeMax;
            if (lifePercentage < 0.95f && NPC.ai[1] > 150f * (lifePercentage * lifePercentage) && (int)NPC.ai[1] % 10 == 0)
            {
                _glowmaskIntensity = 0.6f;
            }
            _glowmaskIntensity = MathHelper.Lerp(_glowmaskIntensity, 0.4f, 0.05f);
            drawColor *= _glowmaskIntensity;
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f - 1f / trailLength * i;
                Main.EntitySpriteDraw(texture, NPC.oldPos[i] + offset, null, drawColor * progress, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}