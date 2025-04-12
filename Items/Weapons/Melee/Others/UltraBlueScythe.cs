using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Others
{
	public class UltraBlueScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.UltraBlueScythe>(40);
			Item.SetWeaponValues(60, 8f, 6);
			Item.width = 74;
			Item.height = 72;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 150);
			Item.rare = 4;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			Item.FixSwing(player);
			return null;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 28);
			recipe.AddIngredient(ItemID.MythrilBar, 14);
			recipe.AddIngredient(ItemID.SoulofNight, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TitaniumBar, 28);
			recipe2.AddIngredient(ItemID.OrichalcumBar, 14);
			recipe2.AddIngredient(ItemID.SoulofNight, 18);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.AdamantiteBar, 28);
			recipe3.AddIngredient(ItemID.OrichalcumBar, 14);
			recipe3.AddIngredient(ItemID.SoulofNight, 18);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.TitaniumBar, 28);
			recipe4.AddIngredient(ItemID.MythrilBar, 14);
			recipe4.AddIngredient(ItemID.SoulofNight, 18);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}