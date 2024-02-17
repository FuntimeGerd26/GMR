using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class ManaBarrier : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Barrier");
            Description.SetDefault("You are protected by your own magic!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += (int)(player.statManaMax2 / 8);
        }
    }
}