using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class MedkitRegen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medkit Regeneration");
            Description.SetDefault("Your health regeneration is increased");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 5;
            player.lifeRegenTime = 10;
        }
    }
}