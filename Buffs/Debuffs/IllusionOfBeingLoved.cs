using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class IllusionOfBeingLoved : ModBuff
    {
        public override string Texture => "GMR/Buffs/Based";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Illusion of being loved");
            Description.SetDefault("Decreased defense and you take 10% more damage");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GerdGlobalNPC>().IllusionOfBeingLoved = true;
            npc.defense = npc.defDefense - 10;
        }
    }
}