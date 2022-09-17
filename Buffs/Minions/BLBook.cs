using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Minions
{
    public class BLBook : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BL Books");
            Description.SetDefault("Orbital books which repel enemies");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GPlayer().BLBook = true;
        }
    }
}