using GMR.Items.Accessories;
using GMR.Items.Misc;
using GMR.Items.Misc.Consumable;
using GMR.Items.Misc.Materials;
using GMR.Items.Tiles;
using GMR.Items.Vanity;
using GMR.Items.Weapons.Magic.Staffs;
using GMR.Items.Weapons.Melee.Spears;
using GMR.Items.Weapons.Melee.Swords;
using GMR.Items.Weapons.Ranged.Bows;
using GMR.Items.Weapons.Ranged.Guns;
using GMR.Items.Weapons.Ranged.Others;
using GMR.Items.Weapons.Ranged.Railcannons;
using GMR.NPCs.Bosses.Acheron;
using GMR.NPCs.Bosses.Jack;
using GMR.NPCs.Bosses.MagmaEye;
using GMR.NPCs.Special;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR
{
    public class GMR : Mod
    {
        public const string SoundsPath = "GMR/Sounds/";
        internal static ModKeybind UniqueKeybind;

        public const string ModName = nameof(GMR);

        public static GMR Instance => ModContent.GetInstance<GMR>();

        public GMR()
        {
        }

        public static GMR GetInstance()
        {
            return ModContent.GetInstance<GMR>();
        }

        public override void Load()
        {
            UniqueKeybind = KeybindLoader.RegisterKeybind(this, "Special Key", "G");
        }

        public static bool Eternity()
        {
            if (ModLoader.TryGetMod("FargowiltasSouls", out Mod souls))
            {
                return (bool)souls.Call("EternityMode");
            }
            return false;
        }

        public override void PostSetupContent()
        {
            // Mod calls
            if (ModLoader.TryGetMod("Census", out Mod foundMod))
            {
                foundMod.Call("TownNPCCondition", ModContent.NPCType<ShapeShifter>(), "Have 300 HP and defeat the Eye of Cthulhu.");
                foundMod.Call("TownNPCCondition", ModContent.NPCType<Memer>(), "Defeat Brain of Cthulhu or Eater of Worlds.");
            }

            // For when I do add an individual summon item for each boss instead of all relying on Hatred
            /*if (ModLoader.TryGetMod("Fargowiltas", out Mod foundFargo))
            {
                foundFargo.Call("AddSummon", 6.5f, ModContent.ItemType<JackDroneOld>(), () => GerdWorld.downedJack, 50000);
            }*/
        }

        public static void TryElementCall(params object[] args)
        {
            if (ModLoader.TryGetMod("BattleNetworkElements", out Mod BNElem))
            {
                var em = ModLoader.GetMod("BattleNetworkElements");
                em.Call(args);
            }
        }

        public override void AddRecipes()
        {
            Mod GMR = ModLoader.GetMod("GMR");
            RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Gem type", ItemID.Diamond, ItemID.Ruby, ItemID.Emerald, ItemID.Sapphire, ItemID.Topaz, ItemID.Amethyst);
            RecipeGroup.RegisterGroup("GMR:AnyGem", group);

            /*recipe.AddRecipeGroup("GMR:AnyX");
			group = new RecipeGroup(() => Lang.misc[37] + " EXAMPLE RECIPE GROUP", Gerdsmod.Find<ModItem>("ITEM1").Type, Gerdsmod.Find<ModItem>("ITEM2").Type);
			RecipeGroup.RegisterGroup("GerdBoss:AnyX", group);*/
        }

        internal static SoundStyle GetSounds(string name, int num, float volume = 1f, float pitch = 0f, float variance = 0f, int instances = 4)
        {
            return new SoundStyle(SoundsPath + name, 0, num) { Volume = volume, Pitch = pitch, PitchVariance = variance, MaxInstances = instances, };
        }
        internal static SoundStyle GetSound(string name, float volume = 1f, float pitch = 0f, float variance = 0f, int instances = 4)
        {
            return new SoundStyle(SoundsPath + name) { Volume = volume, Pitch = pitch, PitchVariance = variance, MaxInstances = instances, };
        }
    }


    public class GMRModIntegrationsSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            DoBossChecklistIntegration();
        }
        private void DoBossChecklistIntegration()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            string internalName = "MagmaEye";
            float weight = 3.25f;
            Func<bool> downed = () => GerdWorld.downedMagmaEye;
            int bossType = ModContent.NPCType<MagmaEye>();
            int spawnItem = ModContent.ItemType<Hatred>();
            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<DGPCrate>(),
                ModContent.ItemType<ExolBlade>(),
                ModContent.ItemType<MagmaticSword>(),
                ModContent.ItemType<Magmathrower>(),
                ModContent.ItemType<MagmaKnife>(),
                ModContent.ItemType<MagmaStaff>(),
                ModContent.ItemType<MagmaticShard>(),
            };
            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("GMR/NPCs/Bosses/MagmaEye_Still").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles,
                    ["customPortrait"] = customPortrait
                }
            );


            internalName = "Jack";
            weight = 5.75f;
            downed = () => GerdWorld.downedJack;
            bossType = ModContent.NPCType<Jack>();
            spawnItem = ModContent.ItemType<Hatred>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<AncientInfraRedPlating>(),
                ModContent.ItemType<InfraRedCrystalShard>(),
                ModContent.ItemType<DGPCrate>(),
                ModContent.ItemType<JackRailcannon>(),
                ModContent.ItemType<IllusionOfLove>(),
                ModContent.ItemType<JackSword>(),
                ModContent.ItemType<AncientHarpoon>(),
                ModContent.ItemType<AncientRifle>(),
                ModContent.ItemType<JuiceBox>(),
                ModContent.ItemType<EternityJackGlider>(),
            };
            customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("GMR/NPCs/Bosses/Jack_Still").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles,
                    ["customPortrait"] = customPortrait
                }
            );

            internalName = "Acheron";
            weight = 7.6f;
            downed = () => GerdWorld.downedAcheron;
            bossType = ModContent.NPCType<Acheron>();
            spawnItem = ModContent.ItemType<Hatred>();
            collectibles = new List<int>()
            {
                ModContent.ItemType<InfraRedBar>(),
                ModContent.ItemType<InfraRedCrystalShard>(),
                ModContent.ItemType<DGPCrate>(),
                ModContent.ItemType<IllusionOfLove>(),
                ModContent.ItemType<InfraRedSpear>(),
                ModContent.ItemType<AcheronBow>(),
                ModContent.ItemType<InfraRedStaff>(),
                ModContent.ItemType<JackRifle>(),
                ModContent.ItemType<JackMask>(),
            };
            customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("GMR/NPCs/Bosses/Acheron_Still").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };
            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles,
                    ["customPortrait"] = customPortrait
                }
            );
        }
    }

    internal class LocalizationRewriter : ModSystem
    {
        public override void PostSetupContent()
        {
#if DEBUG
            MethodInfo refreshInfo = typeof(LocalizationLoader).GetMethod("UpdateLocalizationFilesForMod", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(GameCulture) });
            refreshInfo.Invoke(null, new object[] { GMR.Instance, null, Language.ActiveCulture });
#endif
        }
    }

    internal static class LocalizationRoundabout
    {
        public static void SetDefault(this LocalizedText text, string value)
        {
#if DEBUG
            PropertyInfo valueProp = typeof(LocalizedText).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

            LanguageManager.Instance.GetOrRegister(text.Key, () => value);
            valueProp.SetValue(text, value);
#endif
        }
    }
}