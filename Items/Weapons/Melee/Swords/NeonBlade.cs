	using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class NeonBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflits 'Hellfire' to enemies\n'1000 degrees knife'\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.NeonBlade>(18);
			Item.SetWeaponValues(19, 8f, 4);
			Item.width = 68;
			Item.height = 68;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = 1;
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
			recipe.AddIngredient(ItemID.GoldBar, 18);
			recipe.AddIngredient(ItemID.Ruby, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PlatinumBar, 18);
			recipe2.AddIngredient(ItemID.Ruby, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 40);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}