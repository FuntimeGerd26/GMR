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

        public static Color GetColor(Vector2 v, Color color)
        {
            return Lighting.GetColor((int)(v.X / 16), (int)(v.Y / 16f), color);
        }

        public static Color GetColor(Vector2 v)
        {
            return Lighting.GetColor((int)(v.X / 16), (int)(v.Y / 16f));
        }

        public static void DrawFramedChain(Texture2D chain, Rectangle frame, Vector2 currentPosition, Vector2 endPosition, Vector2 screenPos, Func<Vector2, Color> getLighting = null)
        {
            if (getLighting == null)
            {
                getLighting = GetColor;
            }
            int height = frame.Height - 2;
            Vector2 velocity = endPosition - currentPosition;
            int length = (int)(velocity.Length() / height);
            velocity.Normalize();
            velocity *= height;
            float rotation = velocity.ToRotation() + MathHelper.PiOver2;
            var origin = new Vector2(frame.Width / 2f, frame.Height / 2f);
            for (int i = 0; i < length + 1; i++)
            {
                var position = currentPosition + velocity * i;
                Main.EntitySpriteDraw(chain, position - screenPos, frame, getLighting(position), rotation, origin, 1f, SpriteEffects.None, 0);
            }
        }

        public static void DrawChain(Texture2D chain, Vector2 currentPosition, Vector2 endPosition, Vector2 screenPos, Func<Vector2, Color> getLighting = null)
        {
            DrawFramedChain(chain, chain.Bounds, currentPosition, endPosition, screenPos, getLighting);
        }
    }
}