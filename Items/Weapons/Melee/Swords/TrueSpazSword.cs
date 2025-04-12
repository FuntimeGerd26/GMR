using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using GMR;
using GMR.Items.CustomStuff;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class TrueSpazSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Time's Sword");
			Tooltip.SetDefault($"Shoots blades that inflicts Cursed Inferno and Ichor to enemies\n'Among all, it just had the best performance'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.TrueSpazSword>(38);
			Item.useTime /= 2;
			Item.SetWeaponValues(38, 8f, 4);
			Item.width = 70;
			Item.height = 90;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.sellPrice(silver: 225);
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
			recipe.AddIngredient(null, "SpazSword");
			recipe.AddIngredient(null, "MumeikenKyomu");
			recipe.AddIngredient(null, "InfraRedBar", 30);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 18);
			recipe.AddIngredient(ItemID.DemoniteBar, 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "SpazSword");
			recipe2.AddIngredient(null, "MumeikenKyomu");
			recipe2.AddIngredient(null, "InfraRedBar", 30);
			recipe2.AddIngredient(ItemID.SoulofNight, 12);
			recipe2.AddIngredient(ItemID.Ichor, 18);
			recipe2.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}