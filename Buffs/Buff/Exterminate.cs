using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Exterminate : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Exterminate");
            Description.SetDefault("Damage greatly increased!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.40f;
        }
    }
}