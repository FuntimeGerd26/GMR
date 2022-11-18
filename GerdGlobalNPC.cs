using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR
{
    public class GerdGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public static int boss = -1;

        public int originalDefense;
        public bool FirstTick;

        public bool Glimmering;
        public bool Thoughtful;
        public bool PartialCrystal;

        public override void ResetEffects(NPC npc)
        {
            Glimmering = false;
            Thoughtful = false;
            PartialCrystal = false;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

            if(npc.boss)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.BossUpgradeCrystal>()));
                if (Main.hardMode)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>()));
                }
            }
            else
            {
               npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 3));
            }

            switch (npc.type)
            {
                case 48: //Harpy
                    npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.ThunderbladeCharm>(), ItemID.Feather, ItemID.Feather, ItemID.Feather, ItemID.Feather));
                    break;

                //Bosses for Halu

                case NPCID.KingSlime:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.EyeofCthulhu:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.BrainofCthulhu:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    LeadingConditionRule lastEater = new LeadingConditionRule(new Conditions.LegacyHack_IsABoss());
                    lastEater.OnSuccess(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    npcLoot.Add(lastEater);
                    break;

                case NPCID.QueenBee:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.SkeletronHead:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.WallofFlesh:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.TheDestroyer:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.SkeletronPrime:
                    if (Main.dayTime)
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    else
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;


                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    LeadingConditionRule noTwin = new LeadingConditionRule(new Conditions.MissingTwin());
                    noTwin.OnSuccess(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    npcLoot.Add(noTwin);
                    break;

                case NPCID.Plantera:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));

                    break;

                case NPCID.Golem:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.DukeFishron:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.CultistBoss:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                case NPCID.MoonLordCore:
                    npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    break;

                default:
                    break;
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();

            if (modPlayer.Halu != null)
            {
                spawnRate *= 5; //Sets spawn rate to x5 the normal
                maxSpawns *= 3;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.boss || npc.type == NPCID.EaterofWorldsHead)
                boss = npc.whoAmI;
            bool retval = base.PreAI(npc);

            if (!FirstTick)
            {
                originalDefense = npc.defense;
                FirstTick = true;
            }

            return retval;
        }

        public override void PostAI(NPC npc)
        {
            if (Thoughtful)
            {
                npc.defense = originalDefense / 2;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            Player player = Main.player[Main.myPlayer];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();

            if (Glimmering)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 20;
                if (damage > 5 || damage < 5)
                    damage = 5;
            }

            if (PartialCrystal)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 30;
                if (Main.hardMode && npc.boss)
                {
                    if (damage > 200 || damage < 200)
                        damage = 200;
                }
                else if (npc.boss)
                {
                    if (damage > 40 || damage < 40)
                        damage = 40;
                }
                else
                {
                    if (damage > npc.lifeMax / 40 || damage < npc.lifeMax / 40)
                        damage = npc.lifeMax / 40;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Thoughtful)
            {
                if (Main.rand.NextBool(7))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 68, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, Color.White, 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (PartialCrystal)
            {
                if (Main.rand.NextBool(7))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 60, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 3f;
                }
            }
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (Thoughtful)
            {
                drawColor = Color.Cyan;
                return drawColor;
            }

            if (PartialCrystal)
            {
                drawColor = Color.Red;
                return drawColor;
            }

            return null;
        }
    }
}