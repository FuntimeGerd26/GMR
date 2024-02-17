using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Utilities;
using GMR;

namespace GMR
{
    public static class GerdHelper
    {
        public static int iterations;
        public static bool AnyIterations => iterations > 0;

        public static Regex SubstitutionRegex { get; private set; }

        public static GerdPlayer GPlayer(this Player player)
        {
            return player.GetModPlayer<GerdPlayer>();
        }

        public static void AddElement(this Item item, int elementID)
        {
            GMR.TryElementCall("assignElement", item, elementID);
        }

        public static void AddElement(this Projectile projectile, int elementID)
        {
            GMR.TryElementCall("assignElement", projectile, elementID);
        }

        public static void AddElement(this NPC npc, int elementID)
        {
            GMR.TryElementCall("assignElement", npc, elementID);
        }

        public static void ElementMultipliers(this NPC npc, float[] multipliers)
        {
            GMR.TryElementCall("assignElement", npc, multipliers);
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

        public static bool UpdateProjActive(Projectile projectile, int buff)
        {
            if (!Main.player[projectile.owner].active || Main.player[projectile.owner].dead)
            {
                Main.player[projectile.owner].ClearBuff(buff);
                return false;
            }
            if (Main.player[projectile.owner].HasBuff(buff))
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
        public static bool UpdateProjActive<T>(Projectile projectile) where T : ModBuff
        {
            return UpdateProjActive(projectile, ModContent.BuffType<T>());
        }

        public static bool UpdateProjActive(Projectile projectile, ref bool active)
        {
            if (Main.player[projectile.owner].dead)
                active = false;
            if (active)
                projectile.timeLeft = 2;
            return active;
        }

        public static void GetMinionLeadership(this Projectile projectile, out int leader, out int minionPos, out int count)
        {
            leader = -1;
            minionPos = 0;
            count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
                {
                    if (leader == -1)
                    {
                        leader = i;
                    }
                    if (i == projectile.whoAmI)
                    {
                        minionPos = count;
                    }
                    count++;
                }
            }
        }

        public static int GetMinionTarget(this Projectile projectile, Vector2 position, out float distance, float maxDistance = 2000f, float? ignoreTilesDistance = 0f)
        {
            if (Main.player[projectile.owner].HasMinionAttackTargetNPC)
            {
                distance = Vector2.Distance(Main.npc[Main.player[projectile.owner].MinionAttackTargetNPC].Center, projectile.Center);
                if (distance < maxDistance)
                {
                    return Main.player[projectile.owner].MinionAttackTargetNPC;
                }
            }
            int target = -1;
            distance = maxDistance;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].CanBeChasedBy(projectile))
                {
                    float d = Vector2.Distance(position, Main.npc[i].Center).UnNaN();
                    if (d < distance)
                    {
                        if (!ignoreTilesDistance.HasValue || d < ignoreTilesDistance || Collision.CanHit(position - projectile.Size / 2f, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                        {
                            distance = d;
                            target = i;
                        }
                    }
                }
            }
            return target;
        }
        public static int GetMinionTarget(this Projectile projectile, out float distance, float maxDistance = 2000f, float? ignoreTilesDistance = 0f)
        {
            return GetMinionTarget(projectile, projectile.Center, out distance, maxDistance, ignoreTilesDistance);
        }


        public static int Abs(this int value)
        {
            return value < 0 ? -value : value;
        }
        public static float Abs(this float value)
        {
            return value < 0f ? -value : value;
        }

        public static float UnNaN(this float value)
        {
            return float.IsNaN(value) ? 0f : value;
        }
        public static Vector2 UnNaN(this Vector2 value)
        {
            return new Vector2(UnNaN(value.X), UnNaN(value.Y));
        }

        public static Color GetColor(Vector2 v, Color color)
        {
            return Lighting.GetColor((int)(v.X / 16), (int)(v.Y / 16f), color);
        }

        public static Color GetColor(Vector2 v)
        {
            return Lighting.GetColor((int)(v.X / 16), (int)(v.Y / 16f));
        }

        public static Color GetRainbowColor(Projectile projectile, float index)
        {
            float laserLuminance = 0.5f;
            float laserAlphaMultiplier = 0f;
            float lastPrismHue = projectile.GetLastPrismHue(index % 6f, ref laserLuminance, ref laserAlphaMultiplier);
            float lastPrismHue2 = projectile.GetLastPrismHue((index + 1f) % 6f, ref laserLuminance, ref laserAlphaMultiplier);
            return Main.hslToRgb(MathHelper.Lerp(lastPrismHue, lastPrismHue2, index.Abs() % 1f), 1f, laserLuminance);
        }
        public static Color GetRainbowColor(int player, float position)
        {
            return GetRainbowColor(new Projectile() { owner = player }, position);
        }
        public static Color GetRainbowColor(Player player, float position)
        {
            return GetRainbowColor(player.whoAmI, position);
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

        public static Vector2[] CircularVector(int amt, float angleAddition = 0f)
        {
            return Array.ConvertAll(Circular(amt, angleAddition), (f) => f.ToRotationVector2());
        }
        public static float[] Circular(int amt, float angleAddition = 0f)
        {
            var v = new float[amt];
            float f = MathHelper.TwoPi / amt;
            for (int i = 0; i < amt; i++)
            {
                v[i] = (f * i + angleAddition) % MathHelper.TwoPi;
            }
            return v;
        }

        public static float Wave(float time, float minimum, float maximum)
        {
            return minimum + ((float)Math.Sin(time) + 1f) / 2f * (maximum - minimum);
        }

        public static Color UseA(this Color color, int alpha) => new Color(color.R, color.G, color.B, alpha);
        public static Color UseA(this Color color, float alpha) => new Color(color.R, color.G, color.B, (int)(alpha * 255));


        public static float CappedMeleeScale(this Player player)
        {
            var item = player.HeldItem;
            return Math.Clamp(player.GetAdjustedItemScale(item), 0.5f * item.scale, 2f * item.scale);
        }

        public static void CappedMeleeScale(Projectile proj)
        {
            float scale = Main.player[proj.owner].CappedMeleeScale();
            if (scale != 1f)
            {
                proj.scale *= scale;
                proj.width = (int)(proj.width * proj.scale);
                proj.height = (int)(proj.height * proj.scale);
            }
        }

        public static string CapSpaces(string text)
        {
            return Regex.Replace(text, "([A-Z])", " $1").Trim();
        }

        public static string FormatWith(this string text, object obj)
        {
            string input = text;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            return SubstitutionRegex.Replace(input, delegate (Match match)
            {
                if (match.Groups[1].Length != 0)
                {
                    return "";
                }

                string name = match.Groups[2].ToString();
                PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
                return (propertyDescriptor != null) ? (propertyDescriptor.GetValue(obj) ?? "")!.ToString() : "";
            });
        }

        public class Loader : IOnModLoad
        {
            void ILoadable.Load(Mod mod)
            {
                iterations = 0;
            }

            void IOnModLoad.OnModLoad(GMR gmod)
            {
                SubstitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);
            }

            void ILoadable.Unload()
            {
                SubstitutionRegex = null;
            }
        }



        // Any code below this line is from Pellucid Mod, i did not make/find any of this
        public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshit)
        {
            snapshit.Begin(spriteBatch);
        }

        public static SpriteBatchSnapshot CaptureSnapshot(this SpriteBatch spriteBatch)
        {
            return SpriteBatchSnapshot.Capture(spriteBatch);
        }

        public static bool HasBegun(this SpriteBatch spriteBatch)
        {
            return (bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
        }

        public static void Reload(this SpriteBatch spriteBatch, BlendState state = default, SpriteSortMode mode = SpriteSortMode.Deferred)
        {
            var snapshit = spriteBatch.CaptureSnapshot();
            snapshit.BlendState = state ?? snapshit.BlendState;
            snapshit.SortMode = mode;
            if (spriteBatch.HasBegun())
                spriteBatch.End();
            spriteBatch.Begin(snapshit);
        }

        public static Vector2 ShoulderPosition(this Player player)
        {
            return player.RotatedRelativePoint(player.MountedCenter) + new Vector2(-4 * player.direction, -2);
        }

        /// <summary>
        /// Usefull for held projectiles. Should run only player's client side.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="verticalOffset">Offset for gun muzzles, etc.</param>
        /// <returns>The direction to mouse from the player shoulder. </returns>
        public static Vector2 ShoulderDirectionToMouse(this Player player, float verticalOffset = 0)
        {
            Vector2 shoulderPosition = player.ShoulderPosition();
            Vector2 direction = shoulderPosition.DirectionTo(Main.MouseWorld);

            shoulderPosition += direction.RotatedBy(-MathHelper.PiOver2 * player.direction) * verticalOffset;

            return shoulderPosition.DirectionTo(Main.MouseWorld);
        }


        public static Vector2 ShoulderDirectionToMouse(this Player player, ref Vector2 shoulderPosition, float verticalOffset = 0)
        {
            Vector2 direction = shoulderPosition.DirectionTo(Main.MouseWorld);
            shoulderPosition += direction.RotatedBy(-MathHelper.PiOver2 * player.direction) * verticalOffset;

            return shoulderPosition.DirectionTo(Main.MouseWorld);
        }
    }
}