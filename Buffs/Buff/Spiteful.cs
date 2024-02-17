using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Spiteful : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiteful");
            Description.SetDefault("Crit chance greatly increased!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 25f;
        }
    }
}