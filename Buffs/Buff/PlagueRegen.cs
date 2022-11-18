using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class PlagueRegen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Masked Plague Regeneration");
            Description.SetDefault("Your mana regeneration is increased and magic damage is increased by 5%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaRegen += 10;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }
    }
}