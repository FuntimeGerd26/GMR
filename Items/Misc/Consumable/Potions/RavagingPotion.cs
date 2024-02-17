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
	public class RavagingPotion : ModItem
	{
		public override string Texture => "GMR/Items/Misc/Consumable/Potions/GrimmConcoction";

		public override void SetStaticDefaults()
		{
			ItemID.Sets.DrinkParticleColors[Type] = new Color[] { new Color(125, 5, 65, 0), new Color(90, 0, 20, 0), };

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 30;
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 30;
			Item.rare = 5;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 250);
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<Buffs.Buff.Ravaging>();
			Item.buffTime = 3600;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Deathweed, 5);
			recipe.AddIngredient(ItemID.Shiverthorn, 5);
			recipe.AddIngredient(ItemID.Blinkroot, 3);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 8);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}