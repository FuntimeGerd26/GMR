using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class Glaicey : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.Glaicey>(25);
			Item.SetWeaponValues(14, 6f, 0);
			Item.width = 48;
			Item.height = 48;
			Item.rare = 1;
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
			recipe.AddIngredient(ItemID.IceBlock, 30);
			recipe.AddIngredient(ItemID.SnowBlock, 40);
			recipe.AddRecipeGroup("GMR:AnyGem", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}