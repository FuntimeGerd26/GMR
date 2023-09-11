using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class InfraRedCorrosion : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infra-Red Corrosion");
            Description.SetDefault("'You are exposed to high amounts of Infra-Red energy'\nDosen't allow you to reuse Railguns again");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }
    }
}