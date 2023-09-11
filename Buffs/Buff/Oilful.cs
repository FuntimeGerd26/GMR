using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class Oilful : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oil Smell");
            Description.SetDefault("'Don't you love the smell of fresh oil?'\nIncreases crit chance by 5%\nIncreases melee and ranged damage by 6%\nIncreases magic and summon damage by 4%");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 5f;
            player.GetDamage(DamageClass.Melee) += 0.06f;
            player.GetDamage(DamageClass.Ranged) += 0.06f;
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
        }
    }
}