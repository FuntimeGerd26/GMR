using GMR;
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
using static Terraria.ModLoader.ModContent;
namespace GMR.Items.Misc
{
	public class Hatred : ModItem
	{
		public int BossType;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right-Click to change boss types: Trerios, Magma Eye, Jack, Acheron, loop back");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 54;
            Item.rare = 2;
            Item.value = Item.buyPrice(silver: 550);
            Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.useTime = 40;
			Item.useAnimation = 40;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (BossType < 3)
                    BossType++;
                else
                    BossType = 1;

                Rectangle displayPoint = new Rectangle(player.Hitbox.Center.X, player.Hitbox.Center.Y - player.height / 4, 2, 2);
                if (BossType == 1)
                {
                    CombatText.NewText(displayPoint, new Color(255, 205, 55), "Magma Eye");
                }
                else if (BossType == 2)
                {
                    CombatText.NewText(displayPoint, new Color(125, 125, 125), "Jack");
                }
                else
                {
                    CombatText.NewText(displayPoint, new Color(255, 105, 185), "Acheron");
                }
                return false;
            }
            else if (NPC.downedBoss2 && BossType == 1) // Magma Eye (After Brain of Cthulhu/Eater of World)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.MagmaSummon>();
            }
            else if (BossType == 1) // Magma Eye (Before- Wait what?)
                Main.NewText("'How do you fuck up this much?'", new Color(125, 25, 225));
            else if (NPC.downedBoss3 && BossType == 2) // Jack (After Skeletron)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.JackSummon>();
            }
            else if (BossType == 2) // Jack (Before Skeletron)
                Main.NewText("'You're not allowed'", new Color(125, 125, 125));
            else if (Main.hardMode && BossType == 3) // Acheron (Hardmode)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.AcheronSummon>();
            }
            else if (BossType == 3)
                Main.NewText("'The core's interference dosen't allow you to utilize this option'", new Color(255, 105, 185));
            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (BossType == 1) // Magma Eye
            {
                return new Color(255, 205, 55);
            }
            else if (NPC.downedBoss3 && BossType == 2) // Jack (After Skeletron)
            {
                return new Color(125, 125, 125);
            }
            else if (Main.hardMode && BossType == 3) // Acheron (After WoF)
            {
                return new Color(255, 105, 185);
            }
            else
                return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
        }
    }
}