using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class IllusionOfLove : ModBuff
    {
        public override string Texture => "GMR/Buffs/Based";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possesive");
            Description.SetDefault("Increases crit chance by 10% and decreases damage by 20%\nIncreases life regen mildy");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) -= 0.20f;
            player.GetCritChance(DamageClass.Generic) += 10f;
            player.lifeRegen += 7;
        }
    }
}