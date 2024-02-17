using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class BuiltDifferent : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alternative Engine");
            Description.SetDefault("Increases ranged damage by 15% and ranged crit chance by 5%\nIncreases ranged attack speed by 10%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 5f;
        }
    }
}