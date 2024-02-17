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
	public class DevPlushie : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marketable Amalgam");
			Tooltip.SetDefault($"'Wait was it 3 or 4?'\nIncreases invincibility frames by 2 seconds\nWeapons have a chance to shoot 3 projectile that deal 75% damage and shoot an aditional special projectile");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 72;
			Item.value = Item.sellPrice(silver: 140);
			Item.rare = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (ClientConfig.Instance.MultiplicateProj)
			{
				player.GPlayer().DevPlush = Item;
			}
			player.GPlayer().DevInmune = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(ItemID.Silk, 14);
			recipe.AddIngredient(ItemID.SoulofSight, 8);
			recipe.AddIngredient(ItemID.CrossNecklace);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}