using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class Devilish : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Devilish");
            Description.SetDefault("Deals damage equal to half the enemy's defense\nPlayers get damage equal to 5% of their health");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GerdPlayer>().Devilish = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().Devilish = true;
        }
    }
}