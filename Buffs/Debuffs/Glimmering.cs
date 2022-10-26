using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class Glimmering : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Sickness");
            Description.SetDefault("You feel sick by the glow...");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GerdPlayer>().Glimmering = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().Glimmering = true;
        }
    }
}