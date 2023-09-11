using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class CuttingEdge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dual Cutting Edge");
            Description.SetDefault("Increases melee damage by 14% and increases crit chance by 7%\nDecreases melee knockback by 6%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetKnockback(DamageClass.Melee) += -0.06f;
            player.GetDamage(DamageClass.Melee) += 0.14f;
            player.GetCritChance(DamageClass.Melee) += 7f;
        }
    }
}