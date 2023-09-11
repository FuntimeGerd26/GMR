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
	public class MagmaticEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Makes you explode into a rain of fireballs when hit\nIncreases armor penetration by 10%" +
				"\nIncreases melee and summon damage and attack speed by 5%\n'Rip and tear them apart'");

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
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Summon) += 0.05f;
			player.GetArmorPenetration(DamageClass.Generic) += 0.1f;
			player.GPlayer().MagmaSet = true;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "MagmaticVisor");
			recipe.AddIngredient(null, "MagmaticChestplate");
			recipe.AddIngredient(null, "MagmaticBoots");
			recipe.AddIngredient(null, "ExolBlade");
			recipe.AddIngredient(null, "Magmathrower");
			recipe.AddIngredient(null, "MagmaKnife");
			recipe.AddIngredient(null, "MagmaStaff");
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}