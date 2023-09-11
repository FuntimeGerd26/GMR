using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class PartiallyCrystallized : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Partially Crystallized");
            Description.SetDefault("You feel awful pain");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GerdPlayer>().PartialCrystal = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().PartialCrystal = true;
        }
    }
}