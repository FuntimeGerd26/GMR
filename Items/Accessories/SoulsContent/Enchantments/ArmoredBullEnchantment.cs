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
	public class ArmoredBullEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases endurance and melee attack speed by 2%\nIncreases damage and max health by 15%" +
				"\nZombie Breaker will now be inflict 'Venom', and the 'Poison Charge' mode's attacks will heal you by 2% of your max health" +
				"\nIncreases the damage of ALL projectile shooting weapons by 5%\n'Hit! Golden Fever'");

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
			player.GetAttackSpeed(DamageClass.Melee) += 0.02f;
			player.endurance += 0.02f;
			player.GetDamage(DamageClass.Generic) += 0.15f;
			player.statLifeMax2 += (int)(player.statLifeMax * 0.15f);
			player.GPlayer().BullSet = true;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ArmoredBullHelmet");
			recipe.AddIngredient(null, "ArmoredBullChestplate");
			recipe.AddIngredient(null, "ArmoredBullGreaves");
			recipe.AddIngredient(null, "BullChainsaw");
			recipe.AddIngredient(null, "GerdHeroSword");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}