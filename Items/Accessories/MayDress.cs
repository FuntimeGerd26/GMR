using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class MayDress : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet's Dress");
			Tooltip.SetDefault("'It's so sad'\nIncreases invincibility frames by 2 seconds\nWeapons have a chance to shoot 3 projectile that deal 75% damage and shoot an aditional special projectile\nIncreases attack speed and damage taken by 10% ");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 64;
			Item.value = Item.sellPrice(silver: 240);
			Item.rare = 5;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GPlayer().MayDress = Item;
			player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DyeBlack, 1);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(null, "DevPlushie");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}