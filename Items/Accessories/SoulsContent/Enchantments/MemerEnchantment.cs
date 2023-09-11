using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using GMR;
using GMR.Items.Accessories;

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
	public class MemerEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases magic damage by 25%\nStar Veil, Shiny Stone, Honeycomb, Mana Flower and Celestial Cuffs effects" +
				"\nIncreases falling speed and melee size\nIncreases defense by 10\n'You can make rules, but who's gonna follow them?'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 8;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Magic) += 0.25f;
			player.honeyCombItem = Item;
			if (player.statLife <= player.statLifeMax2 / 2)
				player.AddBuff(BuffID.IceBarrier, 5, true);
			player.manaFlower = true;
			player.manaMagnet = true;
			player.maxFallSpeed += 1.5f;
			player.meleeScaleGlove = true;
			player.shinyStone = true;
			player.starCloakItem = Item;
			player.starCloakItem_starVeilOverrideItem = Item;
			player.statDefense += 10;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "SpazHatMask");
			recipe.AddIngredient(null, "SpazDress");
			recipe.AddIngredient(null, "SpazThighs");
			recipe.AddIngredient(null, "LostTime");
			recipe.AddIngredient(null, "SpazChargeBow");
			recipe.AddIngredient(null, "CoreEjectShotgun");
			recipe.AddIngredient(null, "ScarletLauncher");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}