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

namespace GMR.Items.Weapons.Melee.Swords
{
	public class MagmaticSword : ModItem
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.AddElement(0);
        }

        public override void SetDefaults()
        {
            Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.MagmaticSword>(30);
            Item.useTime /= 2;
            Item.SetWeaponValues(25, 6f, 0);
            Item.width = 60;
            Item.height = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 125);
            Item.rare = 3;
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
    }
}