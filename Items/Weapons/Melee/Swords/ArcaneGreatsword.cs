using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class ArcaneGreatsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.ArcaneGreatsword>(30);
			Item.SetWeaponValues(40, 8f, 4);
			Item.scale = 1f;
			Item.width = 72;
			Item.height = 72;
			Item.rare = 4;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 250);
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
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(null, "UpgradeCrystal", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.OrichalcumBar, 10);
			recipe2.AddIngredient(ItemID.SoulofLight, 12);
			recipe2.AddIngredient(null, "UpgradeCrystal", 15);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}