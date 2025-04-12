using System.Collections.Generic;
using System.ComponentModel;
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
        [Label("[i:GMR/DevPlushie] Blacklist Items")]
        [Tooltip("In case you require more frames or it breaks weapons")]
        public List<ItemDefinition> MultiplicateBlacklist = new();
    }
}
