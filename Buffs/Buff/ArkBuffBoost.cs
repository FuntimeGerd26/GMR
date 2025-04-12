using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class ArkBuffBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.45f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.45f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.45f;
            player.GetAttackSpeed(DamageClass.Summon) -= 0.45f;
            player.GetDamage(DamageClass.Generic) += 0.45f;
            player.GetDamage(DamageClass.Summon) += 0.10f;
            player.endurance += 0.22f;
            player.lifeRegen += (int)(player.statLifeMax * 0.1f);
        }
    }
}