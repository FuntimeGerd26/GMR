using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class AtlantisTridentBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trident Bite");
            Description.SetDefault("Increases melee attack speed by 7%\nIncreased knockback by 5%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
            player.GetKnockback(DamageClass.Melee) += 0.05f;
        }
    }
}