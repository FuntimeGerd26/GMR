using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class InfraRedSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Sword");
			Tooltip.SetDefault("Inflicts Partially Crystalized debuff to enemies\nOn hit with the blade or projectiles creates an explosion\n[c/DD1166:--Special Melee Weapon--]");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToDopeSword<Projectiles.Melee.CoolSwords.InfraRedSword>(35);
			Item.useTime /= 3;
			Item.SetWeaponValues(47, 2f, 4);
			Item.crit = 10;
			Item.width = 104;
			Item.height = 104;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 270);
			Item.rare = 4;
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
			recipe.AddIngredient(null, "GerdOldSword");
			recipe.AddIngredient(ItemID.SoulofNight, 28);
			recipe.AddRecipeGroup("GMR:AnyGem", 8);
			recipe.AddIngredient(null, "UpgradeCrystal", 45);
			recipe.AddIngredient(null, "ScrapFragment", 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}