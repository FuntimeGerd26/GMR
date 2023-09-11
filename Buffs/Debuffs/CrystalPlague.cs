using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class CrystalPlague : ModBuff
    {
        public override string Texture => "GMR/Buffs/Buff/PlagueRegen";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Plague");
            Description.SetDefault("Slight damage over time");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().PlagueCrystal = true;
        }
    }
}