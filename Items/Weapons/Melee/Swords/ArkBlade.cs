using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class ArkBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts 'Devilish' to enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.ArkBlade>(30);
			Item.SetWeaponValues(80, 2f, 6);
			Item.width = 70;
			Item.height = 70;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 495);
			Item.rare = 8;
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
			recipe.AddIngredient(null, "AlloybloodSword");
			recipe.AddIngredient(ItemID.SpectreBar, 20);
			recipe.AddIngredient(ItemID.SoulofFright, 16);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 40);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}