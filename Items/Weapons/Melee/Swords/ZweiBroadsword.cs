using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class ZweiBroadsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.ZweiBroadsword>(46);
			Item.SetWeaponValues(50, 12f, 0);
			Item.width = 68;
			Item.height = 68;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 185);
			Item.rare = 3;
			Item.autoReuse = true;
		}

		public override void UpdateInventory(Player player)
		{
			player.statDefense += 5;
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
			recipe.AddIngredient(ItemID.SilverBar, 22);
			recipe.AddIngredient(null, "PrimePlating", 4);
			recipe.AddIngredient(ItemID.Topaz, 5);
			recipe.AddIngredient(null, "UpgradeCrystal", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TungstenBar, 22);
			recipe2.AddIngredient(null, "PrimePlating", 4);
			recipe2.AddIngredient(ItemID.Topaz, 5);
			recipe2.AddIngredient(null, "UpgradeCrystal", 15);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}