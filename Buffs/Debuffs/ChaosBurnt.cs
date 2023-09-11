using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class ChaosBurnt : ModBuff
    {
        public override string Texture => "GMR/Buffs/Based";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ChaosBurnt.cs");
            Description.SetDefault("ChaosBurnt_Debuff_ToolTip_Burn\nChaosBurnt_Debuff_ToolTip_Slow\nChaosBurnt_Debuff_ToolTip_DefenseDown");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().ChaosBurnt = true;
        }
    }
}