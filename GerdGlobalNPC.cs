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
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case 48: //Harpy
                    npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.ThunderbladeCharm>(), ItemID.Feather, ItemID.Feather, ItemID.Feather, ItemID.Feather));
                    break;

                //Bosses for Halu, Issue: Only KS and Golem drop it in Eternity mode????

                case NPCID.KingSlime:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.EyeofCthulhu:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.BrainofCthulhu:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    if (Main.rand.NextBool(10))
                    {
                        LeadingConditionRule lastEater = new LeadingConditionRule(new Conditions.LegacyHack_IsABoss());
                        lastEater.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                        npcLoot.Add(lastEater);
                    }
                    break;

                case NPCID.QueenBee:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.SkeletronHead:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.WallofFlesh:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.TheDestroyer:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.SkeletronPrime:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    if (Main.rand.NextBool(10))
                    {
                        LeadingConditionRule noTwin = new LeadingConditionRule(new Conditions.MissingTwin());
                        noTwin.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                        npcLoot.Add(noTwin);
                    }
                    break;

                case NPCID.Plantera:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.Golem:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.DukeFishron:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.CultistBoss:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
                    break;

                case NPCID.MoonLordCore:
                    if (Main.rand.NextBool(10))
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));
                    }
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
                spawnRate *= 5; //Simple enough to understand, Just read the override line
                maxSpawns *= 3;
            }
        }
    }
}