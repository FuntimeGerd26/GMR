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
	public class AmethystGolemEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases attack speed and armor penetration by 5%\nIncreases damage reduction by 3%" +
				$"\nWhen damaged you release crystal shards\n'You cannot just sit inside enemies'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 5;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetArmorPenetration(DamageClass.Generic) += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			player.endurance += 0.03f;
			player.GPlayer().AmethystSet = true;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystGolemHelmet");
			recipe.AddIngredient(null, "AmethystGolemChestplate");
			recipe.AddIngredient(null, "AmethystGolemBoots");
			recipe.AddIngredient(null, "AmethystGreatSlasher");
			recipe.AddIngredient(null, "AmethystBlaster");
			recipe.AddIngredient(null, "PrismaticSwordDance");
			recipe.AddIngredient(null, "PrismaticRifle");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}