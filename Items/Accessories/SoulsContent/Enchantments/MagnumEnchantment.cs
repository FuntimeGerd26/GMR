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
	public class MagnumEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases the speed of ranged velocity\nRanged weapons have a chance to shoot a rocket" + 
				"\nIncreases ranged damage by 10\nIncreases max health by 10%\nIncreases movement speed by 5%\n'Time for the highlight'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 4;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Ranged).Flat += 10f;
			player.GetKnockback(DamageClass.Ranged) += 0.05f;
			player.statLifeMax2 += player.statLifeMax / 10;
			player.moveSpeed += 0.05f;
			player.GPlayer().MagnumSet = true;
			player.GPlayer().ChargedArm = Item;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MagnumFoxMask");
			recipe.AddIngredient(null, "MagnumFoxChestplate");
			recipe.AddIngredient(null, "MagnumFoxBoots");
			recipe.AddIngredient(null, "MagnumShooter");
			recipe.AddIngredient(null, "ChargedArm");
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}