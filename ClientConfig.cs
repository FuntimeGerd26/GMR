using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace GMR
{
    public class ClientConfig : ModConfig
    {
        public static ClientConfig Instance { get; private set; }
        public override void OnLoaded()
        {
            Instance = this;
        }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Misc")]
        [BackgroundColor(90, 90, 180)]
        [Label("Screen-Shake")]
        [DefaultValue(true)]
        public bool EnabledScreenShake;

        [Header("Accessories")]
        [BackgroundColor(180, 90, 120)]
        [Label("$[i:GMR/AlloybloodEnchantment] Alloyblood Dagger")]
        [Tooltip("May create lag with all the items it creates")]
        [DefaultValue(true)]
        public bool AlloybloodDagger;

        [BackgroundColor(180, 90, 120)]
        [Label("$[i:GMR/DevPlushie] Multiplicative Projectiles")]
        [Tooltip("In case you require more frames or it breaks weapons")]
        [DefaultValue(true)]
        public bool MultiplicateProj;

        [BackgroundColor(180, 90, 120)]
        [Label("$[i:GMR/NajaCharm] Volcano Charm Fireballs")]
        [Tooltip("Generally an annoying effect, i'll think of one later")]
        [DefaultValue(true)]
        public bool NajaFireball;
    }
}
