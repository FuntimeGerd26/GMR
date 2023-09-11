using System;
using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DesertAxe : ModItem
	{
        private static readonly Color[] itemNameCycleColors = {
            new Color(255, 125, 0),
            new Color(0, 0, 0),
        };

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault($"'S@nd'\nHitting enemies will cause explosions\n[c/DD1166:--Special Melee Weapon--]");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Count of items to research
		}

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.DesertAxe>(25);
            Item.useTime /= 2;
            Item.SetWeaponValues(28, 3f, 4);
            Item.width = 82;
            Item.height = 82;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.axe = 20;
            Item.tileBoost = 2;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = 2;
            Item.autoReuse = true;
            Item.reuseDelay = 2;
        }

		public override bool MeleePrefix()
        {
            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int numColors = itemNameCycleColors.Length;

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.Mod == "Terraria" && line2.Name == "ItemName")
                {
                    float fade = (Main.GameUpdateCount % 60) / 60f;
                    int index = (int)((Main.GameUpdateCount / 60) % numColors);
                    int nextIndex = (index + 1) % numColors;

                    line2.OverrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[nextIndex], fade);
                }
            }
        }
    }
}