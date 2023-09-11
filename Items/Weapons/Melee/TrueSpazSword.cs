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

namespace GMR.Items.Weapons.Melee
{
	public class TrueSpazSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Time's Sword");
			Tooltip.SetDefault($"Shoots blades that inflicts Cursed Inferno and Ichor to enemies\n'Among all, it just had the best performance'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.TrueSpazSword>(38);
			Item.useTime /= 2;
			Item.SetWeaponValues(70, 2f, 4);
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
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.CursedFlame, 18);
			recipe.AddIngredient(ItemID.DemoniteBar, 25);
			recipe.AddIngredient(ItemID.CobaltBar, 14);
			recipe.AddRecipeGroup("GMR:AnyGem", 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "SpazSword");
			recipe2.AddIngredient(null, "MumeikenKyomu");
			recipe2.AddIngredient(ItemID.SoulofNight, 12);
			recipe2.AddIngredient(ItemID.Ichor, 18);
			recipe2.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe2.AddIngredient(ItemID.CobaltBar, 14);
			recipe2.AddRecipeGroup("GMR:AnyGem", 2);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(null, "SpazSword");
			recipe3.AddIngredient(null, "MumeikenKyomu");
			recipe3.AddIngredient(ItemID.SoulofNight, 12);
			recipe3.AddIngredient(ItemID.CursedFlame, 18);
			recipe3.AddIngredient(ItemID.DemoniteBar, 25);
			recipe3.AddIngredient(ItemID.PalladiumBar, 14);
			recipe3.AddRecipeGroup("GMR:AnyGem", 2);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(null, "SpazSword");
			recipe4.AddIngredient(null, "MumeikenKyomu");
			recipe4.AddIngredient(ItemID.SoulofNight, 12);
			recipe4.AddIngredient(ItemID.Ichor, 18);
			recipe4.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe4.AddIngredient(ItemID.PalladiumBar, 14);
			recipe4.AddRecipeGroup("GMR:AnyGem", 2);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}