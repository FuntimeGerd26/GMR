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

namespace GMR.Items.Accessories.SoulsContent.Enchantments
{
	public class MaskedPlagueEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Using Magic or Summon weapons will shoot a homing projectile that inflicts 'Crystal Plague', hide the accessory to distable this effect" + 
				"\nIncreases magic damage by 6% and summon damage by 10%\nIncreases max minions by 2\n'No it dosen't bring the plague'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 200);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!hideVisual)
				player.GPlayer().MaskedPlagueCloak = Item;
			player.GetDamage(DamageClass.Magic) += 0.06f;
			player.GetDamage(DamageClass.Summon) += 0.10f;
			player.maxMinions += 2;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MaskedPlagueHeadgear");
			recipe.AddIngredient(null, "MaskedPlagueBreastplate");
			recipe.AddIngredient(null, "MaskedPlagueSword");
			recipe.AddIngredient(null, "MaskedPlagueFlail");
			recipe.AddIngredient(null, "MaskedPlagueStaff");
			recipe.AddIngredient(null, "MaskedPlagueCloak");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}