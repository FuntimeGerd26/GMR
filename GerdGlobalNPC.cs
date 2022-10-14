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
                        npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<Items.Accessories.Halu>(), ModContent.ItemType<Items.Accessories.Halu>()));

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
                spawnRate *= 5; //Simple enough to understand, Just read the override line
                maxSpawns *= 3;
            }
        }
    }
}