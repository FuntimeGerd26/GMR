using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class ScarletLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scalet Launcher");
			Tooltip.SetDefault($"'It can't slow down time sadly'\n Right-click to shoot a rocket that deals 200% damage, that stays still for a second" +
				"\nThis projectile also creates a second explosion");
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 28;
			Item.rare = 2;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 160);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item61;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 28;
			Item.crit = 8;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ScarletRocket>();
			Item.shootSpeed = 1f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, -2);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 90;
				Item.useAnimation = 90;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ScarletRocketWait>();
				Item.shootSpeed = 0.001f;
			}
			else
			{
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ScarletRocket>();
				Item.shootSpeed = 12f;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
            {
				Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<Projectiles.StopAura>(), 0, 0f, player.whoAmI);
				damage = damage * 2;
			}
			else
			{
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.StarCannon);
			recipe.AddIngredient(ItemID.FlareGun);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddIngredient(ItemID.CrimtaneBar, 14);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.StarCannon);
			recipe2.AddIngredient(ItemID.FlareGun);
			recipe2.AddIngredient(ItemID.IllegalGunParts);
			recipe2.AddIngredient(ItemID.DemoniteBar, 14);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}