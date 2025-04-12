using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Swords
{
	public class SpazSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatanium Sword");
			Tooltip.SetDefault("Hitting enemies makes homing eyes fall on top of the enemy and grants you the 'Empowered' buff for 5 seconds\nThe damage of the eyes is [c/FF4444:75%] of the damage dealt");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 58;
			Item.rare = 2;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(silver: 80);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 12;
			Item.crit = 0;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.SpecialSwords.SpazSword>();
			Item.shootSpeed = 6f;
			Item.knockBack = 6f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity * 0, type, damage,
				knockback, player.whoAmI, player.direction * player.gravDir,
				player.itemAnimationMax);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 18);
			recipe.AddIngredient(ItemID.Lens, 6);
			recipe.AddIngredient(ItemID.FallenStar, 8);
			recipe.AddIngredient(null, "GerdDagger");
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.CrimtaneBar, 18);
			recipe2.AddIngredient(ItemID.Lens, 6);
			recipe2.AddIngredient(ItemID.FallenStar, 8);
			recipe2.AddIngredient(null, "GerdDagger");
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}