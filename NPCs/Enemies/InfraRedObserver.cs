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
    public class InfraRedObserver : ModNPC
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
                return 0.0006f; //0.06% chance of spawning on the underground or caverns in hardmode
            }
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
            new FlavorTextBestiaryInfoElement("A security drone that used to be roaming around various facilities." + 
            "\nThis machine now roams freely various encloses systems in order to find more of itself." +
            "\nOnce it finds more of itself it then exchanges data to create maps of various areas."),
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 36;
            NPC.lifeMax = 400;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath44;
            NPC.knockBackResist = 0.9f;
            NPC.damage = 30;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(silver: 20);
            NPC.npcSlots = 1f;
            NPC.ElementMultipliers([1f, 0.5f, 0.8f, 1.5f]);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (NPC.ai[0] >= 300)
                return false;
            else
                return true;
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
        Vector2 playerOldPos;
        float offsetX;
        float offsetY;
        float rotProj;
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

            if (player.dead)
            {
                NPC.velocity.Y -= 0.25f;
                NPC.EncourageDespawn(180);
                return;
            }
            else if (++NPC.ai[0] >= 420 && ++NPC.ai[2] % 3 == 0 && Collision.CanHit(NPC.Center, 1, 1, player.Center, 1, 1))
            {
                rotProj += 0.3f;
                Vector2 velocity = NPC.DirectionTo(playerOldPos) * 0.1f;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center,
                    velocity.RotatedBy(MathHelper.ToRadians(22.5f * rotProj - 52.5f)), ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 0f, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item33, NPC.Center);

                if (++NPC.ai[0] >= 528)
                {
                    NPC.ai[0] = 0;
                    rotProj = 0f;
                }
            }
            else if (++NPC.ai[0] >= 300)
            {
                if (!checkPos)
                {
                    offsetX = Main.rand.NextFloat(-20f, 20f) * 20f;
                    offsetY = Main.rand.NextFloat(-15f, 15f) * 20f;
                    playerOldPos = player.Center;
                    checkPos = true;
                }

                Vector2 toAtkPosition = new Vector2(playerOldPos.X + offsetX, playerOldPos.Y + offsetY);
                NPC.rotation = Vector2.Lerp(Vector2.UnitX.RotatedBy(NPC.rotation), Vector2.UnitX.RotatedBy(-0f), 0.15f).ToRotation();
                NPC.velocity = Vector2.Lerp(NPC.velocity, toAtkPosition - NPC.Center, 0.2f);
            }
            else
            {
                NPC.rotation = NPC.velocity.X * 0.0134f;
                NPC.velocity = ((player.Center - Vector2.UnitY * 100) - NPC.Center) * 0.01f;
                checkPos = false;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.InfraRedBar>(), 1, 6, 20));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var glow = GMR.Instance.Assets.Request<Texture2D>("NPCs/Enemies/InfraRedObserver_Eye", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var originGlow = new Vector2(glow.Width / 2f, glow.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, Color.White, NPC.rotation, originGlow, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}