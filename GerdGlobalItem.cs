using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using ReLogic.Content;
using GMR;

namespace GMR
{
    public class GerdGlobalItem : GlobalItem
    {
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.GPlayer().IllusionOfLove)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Buff.IllusionOfLove>(), 180);
                target.AddBuff(ModContent.BuffType<Buffs.Debuffs.IllusionOfBeingLoved>(), 180);
            }

            if (target.HasBuff(ModContent.BuffType<Buffs.Debuffs.IllusionOfBeingLoved>()))
                target.takenDamageMultiplier *= 1.1f;

            if (target.HasBuff(ModContent.BuffType<Buffs.Debuffs.Rupture>()))
                target.takenDamageMultiplier *= 2f;
        }
    }
}