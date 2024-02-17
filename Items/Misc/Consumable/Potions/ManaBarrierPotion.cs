using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Consumable.Potions
{
	public class ManaBarrierPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Type] = new Color[] { new Color(0, 55, 90, 0), new Color(0, 20, 80, 0), };

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 28;
			Item.rare = 5;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 250);
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<Buffs.Buff.ManaBarrier>();
			Item.buffTime = 3600;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(ItemID.BottledWater, 2);
			recipe.AddIngredient(ItemID.Waterleaf, 5);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.BeeWax, 2);
			recipe.AddIngredient(ItemID.SoulofLight, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 10);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}