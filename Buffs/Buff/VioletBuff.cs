using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class VioletBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Violet's Blessing");
            Description.SetDefault("Increases magic damage and attacks speed by 10% and mana regeneration by 20%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Magic) += 0.10f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.10f;
            player.manaRegen += (int)(player.manaRegen / 5);
        }
    }
}