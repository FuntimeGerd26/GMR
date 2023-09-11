using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Empowered : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Empowered");
            Description.SetDefault("Increases damage by 5%\nIncreases crit chance by 3%\nIncreases knockback");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 3f;
            player.GetKnockback(DamageClass.Generic) += 0.5f;
        }
    }
}