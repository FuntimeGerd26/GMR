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
	public class LuxLunae : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Shoots a beam that when hitting an enemy, grants the player the 'Cutting Edge' and 'Empowered' buffs\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.LuxLunaeProj>(32);
			Item.useTime /= 2;
			Item.SetWeaponValues(68, 2f, 8);
			Item.width = 56;
			Item.height = 56;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 5;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.buyPrice(silver: 315);
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 35);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.SoulofLight, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}