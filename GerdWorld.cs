using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Creative;
using GMR;

namespace GMR
{
    public class GerdWorld : ModSystem
    {
        public static bool downedAcheron;
        public static bool downedJack;
        public static bool downedMagmaEye;
        public static bool downedTrerios;

        public override void OnWorldUnload()
        {
            downedAcheron = false;
            downedJack = false;
            downedMagmaEye = false;
            downedTrerios = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["downedAcheron"] = downedAcheron;
            tag["downedJack"] = downedJack;
            tag["downedMagmaEye"] = downedMagmaEye;
            tag["downedTrerios"] = downedTrerios;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("downedAcheron"))
                downedAcheron = tag.GetBool("downedAcheron");
            if (tag.ContainsKey("downedJack"))
                downedJack = tag.GetBool("downedJack");
            if (tag.ContainsKey("downedMagmaEye"))
                downedMagmaEye = tag.GetBool("downedMagmaEye");
            if (tag.ContainsKey("downedTrerios"))
                downedTrerios = tag.GetBool("downedTrerios");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedAcheron;
            flags[1] = downedJack;
            flags[2] = downedMagmaEye;
            flags[3] = downedTrerios;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            downedAcheron = flags[0];
            downedJack = flags[1];
            downedMagmaEye = flags[2];
            downedTrerios = flags[3];
        }
    }
}