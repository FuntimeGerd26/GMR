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
	public class AluminiumEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases damage, knockback, attack speed by 5%, weapons have a chance to shoot an Aluminium Shuriken, hide the accessory to distable the shurikens" +
				$"\nRanged weapons will shoot a projectile that will home into enemies\n'It's called aluminium you silly'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetKnockback(DamageClass.Generic) += 0.05f;
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
			if (!hideVisual)
				player.GPlayer().AlumArmor = Item;
			player.GPlayer().AluminiumCharm = Item;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AluminiumHelmet");
			recipe.AddIngredient(null, "AluminiumBreastplate");
			recipe.AddIngredient(null, "AluminiumLeggings");
			recipe.AddIngredient(null, "AluminiumSword");
			recipe.AddIngredient(null, "AluminiumGun");
			recipe.AddIngredient(null, "AluminiumBow");
			recipe.AddIngredient(null, "AluminiumStaff");
			recipe.AddIngredient(null, "AluminiumCharm");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}