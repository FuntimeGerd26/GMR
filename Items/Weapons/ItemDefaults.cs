using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons
{
	public static class ItemDefaults
	{
        public static void DefaultToDopeSword<T>(this Item item, int swingTime) where T : ModProjectile
        {
            item.useTime = swingTime;
            item.useAnimation = swingTime;
            item.shoot = ModContent.ProjectileType<T>();
            item.shootSpeed = 1f;
            item.DamageType = DamageClass.Melee;
            item.useStyle = ItemUseStyleID.Shoot;
            item.autoReuse = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;
        }
    }
}