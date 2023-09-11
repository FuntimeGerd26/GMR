using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class JackyMaskOn : ModBuff
    {
        public override string Texture => "GMR/Buffs/Debuffs/InfraRedCorrosion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shape Shifter Mask");
            Description.SetDefault("'Your armor now becomes vanity'");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}