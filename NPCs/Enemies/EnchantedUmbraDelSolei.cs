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
    public class EnchantedUmbraDelSolei : ModNPC
    {
        public override string Texture => "GMR/Items/Weapons/Melee/Swords/GerdSword";

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 25;
            NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitScale = 0.8f,
                PortraitPositionYOverride = 1.2f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPC.AddElement(2);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDirtLayerHeight && Main.hardMode)
            {
                return 0.000025f; //0.085% chance of spawning on the underground in hardmode
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
            new FlavorTextBestiaryInfoElement("An unique sword that has taken sentience and wanders across the world. It's a rare sight finding one in the wild instead of creating one"),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 112;
            NPC.height = 112;
            NPC.lifeMax = 800;
            NPC.defense = 15;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.2f;
            NPC.damage = 30;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(silver: 350);
            NPC.npcSlots = 1f;
        }

        int state;
        float velMult;
        Vector2 playerOldPos;
        bool dashing;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }


            if (player.dead)
            {
                NPC.velocity.Y += 0.3f;
                NPC.EncourageDespawn(300);
                return;
            }
            else
            {
                if (state == 2)
                {
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.ToRadians(45f);
                    if (++NPC.ai[0] == 1)
                        NPC.velocity = playerOldPos - NPC.Center;
                    else
                        NPC.velocity = NPC.velocity;
                    NPC.velocity.Normalize();
                    if (++NPC.ai[0] >= 60)
                        velMult *= 0.97f;
                    NPC.velocity *= velMult;
                    dashing = true;

                    if (++NPC.ai[0] == 120)
                    {
                        state = 0;
                        velMult = 5f;
                        playerOldPos = player.Center;
                        NPC.ai[0] = 0;
                    }
                }
                else if (state == 1)
                {
                    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.ToRadians(45f);
                    if (++NPC.ai[0] == 1)
                        NPC.velocity = playerOldPos - NPC.Center;
                    else
                        NPC.velocity = NPC.velocity;
                    NPC.velocity.Normalize();
                    if (++NPC.ai[0] >= 60)
                        velMult *= 0.97f;
                    NPC.velocity *= velMult;
                    dashing = true;

                    if (++NPC.ai[0] == 120)
                    {
                        state = 2;
                        velMult = 20f;
                        playerOldPos = player.Center;
                        NPC.ai[0] = 0;
                    }
                }
                else
                {
                    velMult += 0.25f;
                    NPC.rotation += MathHelper.ToRadians(velMult);
                    NPC.velocity *= 0.8f;
                    dashing = false;

                    if (++NPC.ai[0] == 120)
                    {
                        state = 1;
                        velMult = 20f;
                        playerOldPos = player.Center;
                        NPC.ai[0] = 0;
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            state = 0;
            velMult = 0f;
            NPC.ai[0] = 0;

            for (int i = 0; i < 10; i++)
            {
                Dust dustId = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 111, NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 1.5f);
                dustId.noGravity = true;
                Dust dustId2 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 111, -NPC.velocity.X, -NPC.velocity.Y, 60, default(Color), 1.5f);
                dustId2.noGravity = true;

                Dust dustId3 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 164, NPC.velocity.X, NPC.velocity.Y, 60, default(Color), 1.5f);
                dustId3.noGravity = true;
                Dust dustId4 = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 164, -NPC.velocity.X, -NPC.velocity.Y, 60, default(Color), 1.5f);
                dustId4.noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            int trailLength = NPCID.Sets.TrailCacheLength[NPC.type];

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < trailLength; i++)
            {
                float progress = 1f - 1f / trailLength * i;
                Main.EntitySpriteDraw(texture, NPC.oldPos[i] + offset, null, new Color(Main.DiscoR, 125, 255) * progress, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); //reset to normal
            Main.EntitySpriteDraw(texture, NPC.position + offset, null, Color.White, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}