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
using GMR;

namespace GMR.Items.Accessories
{
	public class IllusionOfLove : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 16)); // How long the frames last, Animation Frames
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Animates in world
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 42;
			Item.value = Item.sellPrice(silver: 160);
			Item.rare = 4;
			Item.expert = true;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
			player.GetAttackSpeed(DamageClass.Magic) += 0.2f;
			player.manaCost = player.manaCost * 0.9f;
			player.GPlayer().IllusionOfLove = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 10);
		}
	}
}