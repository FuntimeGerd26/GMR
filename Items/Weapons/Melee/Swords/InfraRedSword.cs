using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class InfraRedSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.InfraRedSword>(30);
			Item.useTime /= 2;
			Item.SetWeaponValues(50, 6f, 6);
			Item.crit = -2;
			Item.width = 126;
			Item.height = 126;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 270);
			Item.rare = 5;
			Item.autoReuse = true;
			Item.reuseDelay = 1;
		}

		public override bool MeleePrefix()
		{
			return true;
		}

		public override bool CanShoot(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "NeonSaber");
			recipe.AddIngredient(ItemID.SoulofNight, 28);
			recipe.AddIngredient(null, "MagmaticShard", 15);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 6);
			recipe.AddIngredient(null, "InfraRedBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}