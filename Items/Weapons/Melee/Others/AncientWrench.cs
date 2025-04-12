using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Melee.Others
{
	public class AncientWrench : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.NewSwords.AncientWrench>(30);
			Item.SetWeaponValues(30, 5f, 6);
			Item.width = 68;
			Item.height = 58;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = 3;
			Item.autoReuse = true;
			Item.reuseDelay = 2;
			Item.value = Item.sellPrice(silver: 65);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AncientInfraRedPlating", 25);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 10);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AncientInfraRedPlating", 25);
			recipe2.AddIngredient(null, "InfraRedCrystalShard", 10);
			recipe2.AddIngredient(ItemID.TissueSample, 15);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}