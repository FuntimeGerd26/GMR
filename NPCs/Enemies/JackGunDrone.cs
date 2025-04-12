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
    public class JackGunDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infra-Red Gun Drone");
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPC.AddElement(0);
            NPC.AddElement(2);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDirtLayerHeight && NPC.downedBoss3)
            {
                return 0.00095f; //0.095% chance of spawning on the underground after beating Skeletron
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
            new FlavorTextBestiaryInfoElement("A small sentry drone, it had over a thousand sales withing a single day when it was on stock, they were cheap but the machinery to make them was destroyed in an attack."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 32;
            NPC.lifeMax = 350;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.05f;
            NPC.damage = 10;
            NPC.aiStyle = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(silver: 35);
            NPC.npcSlots = 1f;
            NPC.ElementMultipliers([1f, 0.5f, 0.8f, 1.5f]);
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

            NPC.ai[1] = 1;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Vector2 toPlayer = NPC.Center - player.Center;
            NPC.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(180f);
            NPC.velocity = NPC.DirectionTo(player.Center.X > NPC.Center.X ? player.Center - 60 * Vector2.UnitX : player.Center + 60 * Vector2.UnitX) * 2.5f;

            if (player.dead)
            {
                NPC.velocity.Y += 0.5f;
                NPC.EncourageDespawn(300);
                NPC.aiStyle = 0;
                return;
            }

            if (++NPC.ai[1] % 90 == 0 && Collision.CanHit(NPC.Center, 1, 1, player.Center, 1, 1))
            {
                Vector2 velocity = NPC.DirectionTo(player.Center) * 3f;
                for (int i = 0; i < 1; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity.RotatedByRandom(MathHelper.ToRadians(7)), ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.AncientInfraRedPlating>(), 1, 3, 12));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D npcTex = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value; // The NPC's texture
            Rectangle rectangle = NPC.frame; // The size of the texture/hitbox
            Vector2 origin2 = rectangle.Size() / 2f; // Where does it draw at
            SpriteEffects effects = NPC.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically; // This makes the NPC always have it's top side up

            if (!NPC.IsABestiaryIconDummy) // Check if it's not the bestiary entry what's being drawn
            {
                NPC.direction = 1;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Main.EntitySpriteDraw(npcTex, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), NPC.GetAlpha(drawColor), NPC.rotation, origin2, NPC.scale, effects, 0);

            return false; // Don't draw the original sprite
        }
    }
}