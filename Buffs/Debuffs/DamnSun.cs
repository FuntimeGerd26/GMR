using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Debuffs
{
    public class DamnSun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Burnt");
            Description.SetDefault("'Your skin burns even in the shadows'\nIncreases summon knockback and movement speed of the player by 5%\nKilling enemies makes them explode on death");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetKnockback(DamageClass.Summon) += 0.05f;
            player.moveSpeed += 0.05f;
            player.runAcceleration += 0.05f;
            player.maxRunSpeed += 0.05f;
        }
    }
}