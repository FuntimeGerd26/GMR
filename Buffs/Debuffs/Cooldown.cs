using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class Cooldown : ModBuff
    {
        public override string Texture => "GMR/Buffs/Based";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cooldown");
            Description.SetDefault("You cannot use abilities that require a key to be pressed");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
    }
}