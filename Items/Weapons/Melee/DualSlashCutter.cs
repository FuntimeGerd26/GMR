using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace GMR.Items.Weapons.Melee
{
	public class DualSlashCutter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Today is friday in california'\nGrants 'Rapid Healing' and 'Wrath' buffs when hitting an enemy\nInflicts 'Venom' on enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 72;
			Item.height = 70;
			Item.rare = 4;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 75;
			Item.crit = 8;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.DualSlashCutterSwing>();
			Item.shootSpeed = 6f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] <= 0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 18);
			recipe.AddIngredient(ItemID.SoulofNight, 22);
			recipe.AddIngredient(ItemID.SoulofLight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipeAlt = CreateRecipe();
			recipeAlt.AddIngredient(ItemID.PalladiumBar, 18);
			recipeAlt.AddIngredient(ItemID.SoulofNight, 22);
			recipeAlt.AddIngredient(ItemID.SoulofLight, 18);
			recipeAlt.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipeAlt.AddTile(TileID.MythrilAnvil);
			recipeAlt.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "DualGunShooter");
			recipe2.Register();
		}
	}
}