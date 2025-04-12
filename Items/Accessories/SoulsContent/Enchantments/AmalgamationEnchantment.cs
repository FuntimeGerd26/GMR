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
	public class AmalgamationEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases health, health regen and damage reduction by 10%\nIncreases magic and ranged knockback by 5%\nIncreases ranged and magic damage by 18%" +
                "\nIncreases luck cap and bullet damage by 20%\nEffect of Palladin's Shield\n'A true amalgamation'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 7;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetKnockback(DamageClass.Magic) += 0.05f;
			player.GetKnockback(DamageClass.Ranged) += 0.05f;
			player.GetDamage(DamageClass.Magic) += 0.18f;
			player.GetDamage(DamageClass.Ranged) += 0.18f;
			player.statLifeMax2 += (int)(player.statLifeMax / 10);
			player.lifeRegen += (int)(player.lifeRegen / 10);
			player.endurance += 0.10f;
			player.luckMaximumCap += player.luckMaximumCap / 5;
			player.bulletDamage += 0.20f;
			player.hasPaladinShield = true;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmalgamationCrown");
			recipe.AddIngredient(null, "AmalgamationShirt");
			recipe.AddIngredient(null, "AmalgamationLeggings");
			recipe.AddIngredient(null, "Epiphany");
			recipe.AddIngredient(null, "UltraBlueChainsaw");
			recipe.AddIngredient(null, "UltraBlueScythe");
			recipe.AddIngredient(null, "UltraBlueEnergyGun");
			recipe.AddIngredient(null, "XShotBow");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}