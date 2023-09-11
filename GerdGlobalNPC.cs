using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using ReLogic.Content;

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
        public bool Devilish;
        public int DevilishDamage;
        public bool ChillBurn;
        public bool PlagueCrystal;
        public bool ChaosBurnt;

        public static int gerdBoss = -1;

        public override void ResetEffects(NPC npc)
        {
            Glimmering = false;
            Thoughtful = false;
            PartialCrystal = false;
            Devilish = false;
            DevilishDamage = 0;
            ChillBurn = false;
            PlagueCrystal = false;
            ChaosBurnt = false;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            Player player = Main.player[Main.myPlayer];
            if (npc.CanBeChasedBy())
            {
                if (npc.boss)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Consumable.Medkit>(), 1, 2, 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.BossUpgradeCrystal>(), 1, 1, 4));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.Halu>(), 10));
                }
                else if (npc.type != NPCID.EaterofWorldsHead || npc.type != NPCID.EaterofWorldsBody || npc.type != NPCID.EaterofWorldsTail)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.SpecialUpgradeCrystal>(), 10000));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 20, 5, 10));
                }

                switch (npc.type)
                {
                    #region Enemies
                    case 48: // Harpy
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.ThunderbladeCharm>(), 40));
                        break;
                        
                    #endregion
                        
                    // Lunar Nova Axe
                    #region Hallowed Enemies
                    case 75: // Pixie
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 86: // Unicorn
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 122: // Gastropod
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 138: // Illuminant Slime
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 137: // Illuminant Bat
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 84: // Enchanted Sword
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 120: // Chaos Elemental
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 80: // Light Mummy
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;
                    case 171: // Hallowed Pigron
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.LunarNovaAxe>(), 20));
                        break;

                    #endregion

                        
                    // Silent Gloves
                    #region Underworld Enemies
                    case 60: // Hellbat
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 250));
                        break;
                    case 59: // Lava Slime
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 250));
                        break;
                    case 24: // Imp
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 250));
                        break;
                    case 62: // Demon
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 250));
                        break;
                    case 66: // Voodoo Demon
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 200));
                        break;
                    case 39: // Bone Serpent
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 200));
                        break;
                    case 151: // Lava Bat
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 250));
                        break;
                    case 156: // Red Devil
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.SilentGloves>(), 200));
                        break;
                    #endregion

                    // Psyco Axe
                    #region Zombies, a fuck ton of em', You have been warned
                    case 3:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -26:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -27:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 430:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 132:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -28:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -29:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 186:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -30:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -31:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 187:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -32:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -33:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 433:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 188:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -34:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -35:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 434:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 189:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -36:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -37:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 200:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -44:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -45:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 438:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 590:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 591:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 319:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 320:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 321:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 331:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 332:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 223:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -54:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case -55:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 161:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 254:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 255:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 52:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 53:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 536:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 632:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    case 251:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PsycopathAxe>(), 100));
                        break;
                    #endregion

                    // Pre-hardmode bosses contain upgrade crystal drops, Hardmode ones have 20% chance to drop hardmode crystals, Different extra Item drops too
                    #region Bosses

                    case NPCID.KingSlime:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        break;

                    case NPCID.EyeofCthulhu:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.DesertAxe>(), 25));
                        break;

                    case NPCID.BrainofCthulhu:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.Blasphemy>(), 100));
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        LeadingConditionRule lastEater = new LeadingConditionRule(new Conditions.LegacyHack_IsABoss());
                        lastEater.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.BossUpgradeCrystal>()));
                        lastEater.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.Blasphemy>(), 100));
                        npcLoot.Add(lastEater);
                        break;

                    case NPCID.QueenBee:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        break;

                    case NPCID.SkeletronHead:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        break;

                    case NPCID.WallofFlesh:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.NanashiDagger>(), 10));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Accessories.HopefulFlower>(), 8));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.UpgradeCrystal>(), 1, 4, 6));
                        break;

                    case NPCID.TheDestroyer:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.SpaceDoggoStaff>(), 5));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;

                    case NPCID.SkeletronPrime:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.SpaceDoggoStaff>(), 5));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;


                    case NPCID.Retinazer:
                    case NPCID.Spazmatism:
                        LeadingConditionRule noTwin = new LeadingConditionRule(new Conditions.MissingTwin());
                        noTwin.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.SpaceDoggoStaff>(), 5));
                        noTwin.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        npcLoot.Add(noTwin);
                        break;

                    case NPCID.Plantera:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summoner.Whips.PlanteraWhip>(), 10));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;

                    case NPCID.Golem:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;

                    case NPCID.DukeFishron:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.PrincessTrident>(), 10));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.ElementalSpear>(), 5));
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;

                    case NPCID.CultistBoss:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;

                    case NPCID.MoonLordCore:
                        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.HardmodeUpgradeCrystal>(), 5));
                        break;
                    #endregion

                    default:
                        break;
                }
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();

            if (Main.getGoodWorld && modPlayer.Halu != null) // If on FTW increase spawns even more
            {
                spawnRate /= 10;
                maxSpawns *= 6;
            }
            else if (modPlayer.Halu != null)
            {
                spawnRate /= 5; //Sets spawn rate to x5 the normal
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
            if (ChaosBurnt)
            {
                npc.defense = originalDefense / 2;
                npc.velocity = npc.velocity * 0.75f;
            }

            if (Thoughtful)
            {
                if (npc.defense >= 30)
                    npc.defense += -30;
                else
                    npc.defense = 0;
            }

            if (ChillBurn)
            {
                if (npc.defense >= 10)
                    npc.defense += -10;
                else
                    npc.defense = 0;
            }
        }

        public override bool PreKill(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();

            if (modPlayer.DamnSun && npc.boss && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Explotion>()] < 10)
            {
                Projectile.NewProjectile(player.GetSource_Misc(""), npc.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.Explotion>(), npc.damage * 2, 2f, player.whoAmI, npc.lifeMax / 3);
            }
            else if (modPlayer.DamnSun && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.SmallExplotion>()] < 10)
            {
                Projectile.NewProjectile(player.GetSource_Misc(""), npc.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Projectiles.Explotion>(), npc.damage * 2, 2f, player.whoAmI, npc.lifeMax / 3);
            }
            return true;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            Player player = Main.player[Main.myPlayer];
            GerdPlayer modPlayer = player.GetModPlayer<GerdPlayer>();

            if (PartialCrystal)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 40;
                if (damage > 10 || damage < 10)
                    damage = 10;
            }

            if (Glimmering)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 20;
                if (damage > 7 || damage < 7)
                    damage = 7;
            }

            if (ChillBurn)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 20;
                if (damage > 8 || damage < 8)
                    damage = 8;
            }

            if (Glimmering)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 16;
                if (damage > 5 || damage < 5)
                    damage = 5;
            }

            if (PlagueCrystal)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 15;
                if (damage > 6 || damage < 6)
                    damage = 6;
            }

            if (Devilish)
            {
                if (npc.boss || npc.defense < 10)
                    DevilishDamage = 10;
                else
                    DevilishDamage = npc.defense * 2;

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 25;
                if (damage > DevilishDamage || damage < DevilishDamage)
                    damage = DevilishDamage;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Thoughtful)
            {
                drawColor = Color.Cyan;

                if (Main.rand.NextBool(7))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 68, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (PartialCrystal)
            {
                drawColor = new Color(255, 25, 50);

                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 60, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 3f;
                }
            }

            if (Devilish)
            {
                if (Main.rand.NextBool(7))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 60, npc.velocity.X * 0.7f, npc.velocity.Y * 0.7f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4f;
                }
            }

            if (ChillBurn)
            {
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 68, npc.velocity.X * 0.7f, npc.velocity.Y * 0.7f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (PlagueCrystal)
            {
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 163, npc.velocity.X * 0.7f, npc.velocity.Y * 0.7f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (ChaosBurnt)
            {
                drawColor = Color.Orange;
            }
        }
    }
}