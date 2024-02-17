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

namespace GMR.NPCs.Bosses.Superboss
{
    [AutoloadBossHead()]
    public class TheAmalgamation : ModNPC
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
        }

        public override void SetDefaults()
        {
            NPC.width = 140;
            NPC.height = 112;
            NPC.lifeMax = 347500;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 520);
            NPC.damage = 120;
            NPC.aiStyle = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/TheAmalgamation");
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
            new FlavorTextBestiaryInfoElement("What have you created..."),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            Dust dustId = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 59 + Main.rand.Next(6), NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
            dustId.noGravity = true;
            Dust dustId3 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 59 + Main.rand.Next(6), -NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
            dustId3.noGravity = true;

            if (NPC.life <= 0)
            {
                Dust dustId2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 59 + Main.rand.Next(6), NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
                dustId2.noGravity = true;
                Dust dustId4 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 59 + Main.rand.Next(6), -NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 2.5f);
                dustId4.noGravity = true;
            }
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];

                if (NPC.damage > 240)
                    NPC.damage = 240;

                if (player.dead)
                {
                    NPC.netUpdate = true;

                    NPC.velocity.Y += 7f;
                    NPC.EncourageDespawn(240);
                    return;
                }
                else
                {
                    Vector2 bossToPlayer = Main.player[NPC.target].Center - 250 * Vector2.UnitY;
                    NPC.velocity = (bossToPlayer - NPC.Center) * 0.08f;
                    NPC.netUpdate = true;
                }

                NPC.rotation = NPC.velocity.X * 0.0314f;

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
                        AI0 = 0;
                    }
                }
            }
        }

        public void BasicAttack()
        {
            Player player = Main.player[NPC.target];
            Vector2 pos1 = NPC.Center - (51 * Vector2.UnitY).RotatedBy(NPC.rotation) - (70 * Vector2.UnitX).RotatedBy(NPC.rotation); // Issues with rotation
            Vector2 pos2 = NPC.Center - (51 * Vector2.UnitY).RotatedBy(NPC.rotation) + (70 * Vector2.UnitX).RotatedBy(NPC.rotation);
            Vector2 toPlayer1 = (Main.player[NPC.target].Center - pos1) * 0.04f;
            Vector2 toPlayer2 = (Main.player[NPC.target].Center - pos2) * 0.04f;
            toPlayer1.X = toPlayer1.X + (player.velocity.X * 0.2f);
            toPlayer2.X = toPlayer2.X + (player.velocity.X * 0.2f);

            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos1, toPlayer1,
                ModContent.ProjectileType<LineBlast>(), NPC.damage, 0f, Main.myPlayer);

            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos2, toPlayer2,
                ModContent.ProjectileType<LineBlast>(), NPC.damage, 0f, Main.myPlayer);

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
                Projectile.NewProjectile(NPC.GetSource_FromThis(), pos, -velocityTo.RotatedByRandom(180f), ModContent.ProjectileType<Projectiles.Bosses.AcheronOrb>(), NPC.damage / 2, 1f, Main.myPlayer);
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            var back = GMR.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Superboss/TheAmalgamationBack", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var originback = new Vector2(back.Width / 2f, back.Height / 2f);
            var shoulder = GMR.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Superboss/TheAmalgamationShoulder", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var originshoulder = new Vector2(shoulder.Width / 2f, shoulder.Height / 2f);
            var arm = GMR.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Superboss/TheAmalgamationArm", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var originarm = new Vector2(arm.Width / 2f, arm.Height / 2f);
            var wing = GMR.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Superboss/TheAmalgamationWing", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var originwing = new Vector2(wing.Width / 2f, wing.Height / 2f);

            Main.EntitySpriteDraw(back, NPC.position + offset, null, drawColor, NPC.velocity.X * 0.1f, originback, NPC.scale, SpriteEffects.None, 0);

            #region Arms
            Main.EntitySpriteDraw(shoulder, NPC.position + offset + (70 * Vector2.UnitX).RotatedBy(NPC.rotation) - (26 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 1.01f, originshoulder, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(arm, NPC.position + offset + (190 * Vector2.UnitX).RotatedBy(NPC.rotation) + (54 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 1.5f, originarm, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(wing, NPC.position + offset + (220 * Vector2.UnitX).RotatedBy(NPC.rotation) - (90 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 2f, originwing, NPC.scale, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(shoulder, NPC.position + offset - (70 * Vector2.UnitX).RotatedBy(NPC.rotation) - (26 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 1.01f, originshoulder, NPC.scale, SpriteEffects.FlipHorizontally, 0);
            Main.EntitySpriteDraw(arm, NPC.position + offset - (190 * Vector2.UnitX).RotatedBy(NPC.rotation) + (54 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 1.5f, originarm, NPC.scale, SpriteEffects.FlipHorizontally, 0);
            Main.EntitySpriteDraw(wing, NPC.position + offset - (220 * Vector2.UnitX).RotatedBy(NPC.rotation) - (90 * Vector2.UnitY).RotatedBy(NPC.rotation),
                null, drawColor, NPC.rotation * 2f, originwing, NPC.scale, SpriteEffects.FlipHorizontally, 0);
            #endregion

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
