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
	public class JackEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Enchantment");
			Tooltip.SetDefault($"A style of swords can now shoot an extra projectile that goes through walls and deals 50% more damage\nIncreases crit chance by 4%, increases jump speed by 14%" + 
				"\nIncreases max minions and sentries by 2\nIncreases movement speed by 7%\n'100110'");

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
			player.jumpSpeedBoost += 0.14f;
			player.maxMinions += 2;
			player.maxTurrets += 2;
			player.moveSpeed += 0.07f;
			player.GPlayer().InfraRedSet = Item;
			player.GetCritChance(DamageClass.Generic) += 4f;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedVisor");
			recipe.AddIngredient(null, "InfraRedPlating");
			recipe.AddIngredient(null, "InfraRedGreaves");
			recipe.AddIngredient(null, "InfraRedSpear");
			recipe.AddIngredient(null, "InfraRedSword");
			recipe.AddIngredient(null, "JackRifle");
			recipe.AddIngredient(null, "JackRailgun");
			recipe.AddIngredient(null, "JackCore");
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}