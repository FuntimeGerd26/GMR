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
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.NPCs.Bosses.Jack.Eternity
{
    public class EternityTower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Energy Source");
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            NPC.AddElement(0);
            NPC.AddElement(2);
        }

        public override void SetDefaults()
        {
            NPC.width = 46;
            NPC.height = 100;
            NPC.lifeMax = 2000;
            NPC.defense = 0;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath37;
            NPC.knockBackResist = 0f;
            NPC.damage = 30;
            NPC.aiStyle = -1;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPC.value = Item.buyPrice(gold: 0);
            NPC.npcSlots = 1f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            //cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources (NOTE: Unused)
            return false; // Set to false because fuck contact damage
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center - 40 * Vector2.UnitY, new Vector3(0.8f, 0.8f, 0.15f));

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<JackE>()))
                {
                    NPC.life += -100;

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
                    NPC.netUpdate = true;
                    return;
                }

                if (NPC.damage > 90)
                    NPC.damage = 90;

                Player player = Main.player[NPC.target];
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }

                NPC.rotation = 0f;

                if (player.dead)
                {
                    NPC.life += -200;
                    NPC.EncourageDespawn(300);
                    NPC.netUpdate = true;
                    return;
                }
                
                if (++NPC.ai[1] % 240 == 0) // After 4 seconds
                {
                    Vector2 velocity = NPC.DirectionTo(player.Center) * 6f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - 40 * Vector2.UnitY, velocity, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), NPC.damage, 1f, Main.myPlayer, NPC.whoAmI);

                    SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                    NPC.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            var glow = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/Eternity/EternityTower_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var offset = NPC.Size / 2f - screenPos;
            SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
            if (NPC.spriteDirection == -1)
                spriteEffects = SpriteEffects.None;

            Color color = new Color(195, 195, 95, 5);

            Main.EntitySpriteDraw(texture, NPC.position + offset, null, drawColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            Main.EntitySpriteDraw(glow, NPC.position + offset, null, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0);

            Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
            var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
            Vector2 flareOrigin = flare.Size() / 2f;
            Main.EntitySpriteDraw(flare, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY) - (40 * Vector2.UnitY),
                null, color, NPC.rotation, flareOrigin, new Vector2(NPC.scale * 2f, NPC.scale * 0.4f), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(flare, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) - (40 * Vector2.UnitY),
                null, color, NPC.rotation + MathHelper.ToRadians(90f), flareOrigin, new Vector2(NPC.scale * 2.5f, NPC.scale * 0.25f), SpriteEffects.None, 0);
            return false;
        }
    }
}