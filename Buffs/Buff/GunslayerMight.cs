using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class GunslayerMight : ModBuff
    {
        public override string Texture => "GMR/Buffs/Buff/Spiteful";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gunslayer Might");
            Description.SetDefault("Attack speed greatly increased!\nDamage lightly decreased!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Ranged) += 0.5f;
            player.GetDamage(DamageClass.Ranged) += -0.25f;
        }
    }
}