using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class WildHunt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.1f;
            player.statDefense -= 40;
            player.GetDamage(DamageClass.Generic) += 0.60f;
            player.GetKnockback(DamageClass.Generic) += 1f;
            player.statLifeMax2 += player.statLifeMax / 4;
        }
    }
}