using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Coffin : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.1f;
            player.statDefense -= 10;
            player.GetDamage(DamageClass.Generic) += 0.20f;
            player.GetKnockback(DamageClass.Generic) += 0.5f;
            player.statLifeMax2 += player.statLifeMax / 4;
        }
    }
}