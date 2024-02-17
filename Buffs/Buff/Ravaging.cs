using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Ravaging : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ravaging");
            Description.SetDefault("Attack speed greatly increased!\nDamage greatly decreased!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic) += 0.5f;
            player.GetDamage(DamageClass.Generic) += -0.65f;
        }
    }
}