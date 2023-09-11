using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class PainfullyHealed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Painfully Healed");
            Description.SetDefault("'You are fine, but still feel the pain'\nYou cannot regenerate health or use Bloody Medkit\nYou take 25% less damage");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<GerdPlayer>().PainHeal = true;
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
        }
    }
}