using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using ReLogic.Content;

using GMR;
using GMR.Items.Vanity;
using GMR.Items.Accessories;
using GMR.Items.Weapons;
using GMR.Items.Misc;
using GMR.Items.Misc.Consumable;
using GMR.Items.Misc.Materials;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Ranged;
using GMR.Items.Weapons.Magic;
using GMR.NPCs.Bosses.Jack;
using GMR.NPCs.Bosses.MagmaEye;
using GMR.NPCs.Special;

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

        internal static SoundStyle GetSounds(string name, int num, float volume = 1f, float pitch = 0f, float variance = 0f)
        {
            return new SoundStyle(SoundsPath + name, 0, num) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
        }
        internal static SoundStyle GetSound(string name, float volume = 1f, float pitch = 0f, float variance = 0f)
        {
            return new SoundStyle(SoundsPath + name) { Volume = volume, Pitch = pitch, PitchVariance = variance, };
        }
    }

    // PS: Waiting till this whole thing breaks, nvm it already did
    /*internal class ModSupport<TMod> : ModSystem where TMod : ModSupport<TMod>
    {
        public static Mod Instance { get; private set; }
        public static string ModName => typeof(TMod).Name;

        public static bool IsLoadingEnabled() { return ModLoader.HasMod(ModName); }

        public static int GetItem(string name, int defaultItem = 0) { return TryFind<ModItem>(name, out var value) ? value.Type : defaultItem; }

        public static bool TryFind<T>(string name, out T value) where T : IModType
        {
            if (Instance == null)
            {
                value = default(T);
                return false;
            }
            return Instance.TryFind(name, out value);
        }

        public static object Call(params object[] args) { return Instance?.Call(args); }

        public override bool IsLoadingEnabled(Mod mod) { return IsLoadingEnabled(); }

        public sealed override void Load()
        {
            Instance = null;
            if (ModLoader.TryGetMod(ModName, out var mod))
            {
                Instance = mod;
                SafeLoad(Instance);
            }
        }

        public virtual void SafeLoad(Mod mod) { }

        public sealed override void Unload()
        {
            SafeUnload();
            Instance = null;
        }

        public virtual void SafeUnload() { }
    }


    internal class BossChecklist : ModSupport<BossChecklist>
    {
        internal enum LogEntryType
        {
            Boss,
            MiniBoss,
        }

        private void LogBossEntry(LogEntryType type, string bossName, List<int> npcIDs, float progression, Func<bool> downed, Func<bool> available, List<int> extraDrops, List<int> spawnItems)
        {
            try
            {
                Instance.Call(
                    $"Add{type}",
                    Mod,
                    $"$Mods.GMR.NPCName.{bossName}",
                    npcIDs,
                    progression,
                    downed,
                    available,
                    extraDrops,
                    spawnItems,
                    $"$Mods.GMR.BossChecklist.{bossName}",
                    null,
                    new Action<SpriteBatch, Rectangle, Color>((spriteBatch, rect, color) =>
                    {
                        var tex = Mod.Assets.Request<Texture2D>("NPCs/Bosses/" + bossName + "_Still", AssetRequestMode.ImmediateLoad).Value;
                        var sourceRect = tex.Bounds;
                        float scale = Math.Min(1f, (float)rect.Width / sourceRect.Width);
                        spriteBatch.Draw(tex, rect.Center.ToVector2(), sourceRect, color, 0f, sourceRect.Size() / 2, scale, SpriteEffects.None, 0);
                    })
                );
            }
            catch (Exception ex)
            {
                Mod.Logger.Error($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        private void LogBossEntries()
        {
            LogBossEntry(
                LogEntryType.Boss,
                    "Jack",
                    new List<int>() { ModContent.NPCType<Jack>() },
                    6.5f, // Post Skeletron, before Wall of Flesh
                    () => GerdWorld.downedJack,
                    null,
                    null,
                    new List<int>() { ModContent.ItemType<JackDroneOld>(), });

            LogBossEntry(
                LogEntryType.Boss,
                "Acheron",
                new List<int>() { ModContent.NPCType<Acheron>() },
                7.5f, // Right before Queen Slime
                () => GerdWorld.downedAcheron,
                null,
                null,
                null);

            LogBossEntry(
                LogEntryType.Boss,
                "MagmaEye",
                new List<int>() { ModContent.NPCType<MagmaEye>() },
                5.5f, // Somewhere before Skeletron
                () => GerdWorld.downedMagmaEye,
                null,
                null,
                null);

        }

        public override void PostSetupContent()
        {
            if (Instance == null)
                return;

            LogBossEntries();
        }
    }*/


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