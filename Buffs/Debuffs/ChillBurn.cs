using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class ChillBurn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chilling Flames");
            Description.SetDefault("Decreases defense by 10\nUpgrade to Frost Burn");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().ChillBurn = true;
            npc.defense = npc.defDefense - 10;
        }
    }
}