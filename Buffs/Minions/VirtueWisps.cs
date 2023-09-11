using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Buffs.Minions
{
    public class VirtueWisps : BaseMinionBuff
    {
        protected override int MinionProj => ModContent.ProjectileType<Projectiles.Summon.VirtueWisps>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Virtues");
            Description.SetDefault("Wisps aid you in your fight");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}