using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons
{
	public static class ItemDefaults
    {
        public static void FixSwing(this Item item, Player player)
        {
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
            {
                if (player.toolTime > 0 && player.itemTime == 0 || !player.controlUseItem)
                    return;
                player.itemAnimation = Math.Min(player.itemAnimation, player.toolTime);
            }
            player.itemAnimation = player.itemAnimationMax;
        }

        public static void DefaultToDopeSword<T>(this Item item, int swingTime) where T : ModProjectile
        {
            item.useTime = swingTime;
            item.useAnimation = swingTime;
            item.shoot = ModContent.ProjectileType<T>();
            item.shootSpeed = 1f;
            item.DamageType = DamageClass.Melee;
            item.useStyle = ItemUseStyleID.Shoot;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;
        }
    }
}