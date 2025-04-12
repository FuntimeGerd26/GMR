using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class Rupture : ModBuff
    {
        public override string Texture => "GMR/Buffs/Based";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rupture");
            Description.SetDefault("Take extra damage from attacks (Enemies Only)");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
    }
}