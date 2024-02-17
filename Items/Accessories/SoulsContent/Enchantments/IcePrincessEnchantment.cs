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
	public class IcePrincessEnchantment : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Has a 1/3 chance to summon a shuriken from the sky towards your cursor" +
				"\nIncreases melee damage by 5% and melee speed by 8%\nIncreases mana by 40 and decreases knockback by 15%\nMelee and ranged attack inflict frostburn\n'A sight that chilled me to the bone'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = 6;
			Item.value = Item.sellPrice(silver: 100);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetKnockback(DamageClass.Generic) -= 0.15f;
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
			player.statManaMax += 40;
			if (!hideVisual)
				player.GPlayer().IcePrincessEnch = Item;
			player.frostArmor = true;
		}
		
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "IceyMask");
			recipe.AddIngredient(null, "IceyBody");
			recipe.AddIngredient(null, "IceyLegs");
			recipe.AddIngredient(null, "IceyWings");
			recipe.AddIngredient(null, "Glaicey");
			recipe.AddIngredient(null, "OvercooledSpear");
			recipe.AddIngredient(null, "OvercooledRifle");
			recipe.AddIngredient(null, "OvercooledNailgun");
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "IceyMask");
			recipe2.AddIngredient(null, "IceyBody");
			recipe2.AddIngredient(null, "IceyLegs");
			recipe2.AddIngredient(null, "IceyWings");
			recipe2.AddIngredient(null, "Glaicey");
			recipe2.AddIngredient(null, "OvercooledSpear");
			recipe2.AddIngredient(null, "OvercooledSniperRifle");
			recipe2.AddIngredient(null, "OvercooledNailgun");
			recipe2.AddTile(TileID.CrystalBall);
			recipe2.Register();
		}
	}
}