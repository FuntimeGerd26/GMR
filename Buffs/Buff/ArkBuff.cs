using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class ArkBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.25f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.25f;
            player.GetAttackSpeed(DamageClass.Summon) -= 0.25f;
            player.GetDamage(DamageClass.Generic) += 0.25f;
            player.GetDamage(DamageClass.Summon) += 0.10f;
            player.endurance += 0.02f;
            player.lifeRegen += 2;
        }
    }
}