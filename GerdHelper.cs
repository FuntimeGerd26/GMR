using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR
{
    public static class GerdHelper
    {
        public static GerdPlayer GPlayer(this Player player)
        {
            return player.GetModPlayer<GerdPlayer>();
        }

        public static int FindClosestHostileNPC(Vector2 location, float detectionRange, bool lineCheck = false)
        {
            NPC closestNpc = null;
            foreach (NPC n in Main.npc)
            {
                if (n.CanBeChasedBy() && n.Distance(location) < detectionRange && (!lineCheck || Collision.CanHitLine(location, 0, 0, n.Center, 0, 0)))
                {
                    detectionRange = n.Distance(location);
                    closestNpc = n;
                }
            }
            return closestNpc == null ? -1 : closestNpc.whoAmI;
        }
    }
}