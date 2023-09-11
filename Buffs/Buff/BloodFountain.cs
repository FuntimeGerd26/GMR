using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class BloodFountain : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Overflow");
            Description.SetDefault("Increases attack speed by 20% and decreases damage by 15%\nIncreases life regen");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) -= 0.15f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f;
            player.lifeRegen += (int)(player.lifeRegen / 5);
        }
    }
}