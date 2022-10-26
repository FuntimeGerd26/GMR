using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class Thoughtful : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forgor");
            Description.SetDefault("Halves defense of enemies\nFor players it decreases 14 defense\n'You forgot what you were gonna do'");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GerdPlayer>().Thoughtful = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().Thoughtful = true;
        }
    }
}