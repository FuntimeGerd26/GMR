using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Buff
{
    public class ReloadBuff : ModBuff
    {
        public override string Texture => "GMR/Buffs/Debuffs/InfraRedCorrosion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reload Charge");
            Description.SetDefault("Damage mildly increased!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.20f;
        }
    }
}