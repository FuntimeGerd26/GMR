using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Others
{
	public class ScarletLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 90;
			Item.height = 42;
			Item.rare = 5;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 195);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item61;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 48;
			Item.crit = 4;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ScarletRocket>();
			Item.shootSpeed = 12f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -4);
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
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.ScarletRocket>();
				Item.shootSpeed = 18f;
			}
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse == 2)
            {
				Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<Projectiles.StopAura>(), 0, 0f, player.whoAmI);
				damage = damage * 3;
			}
			else { }
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FlareGun);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddIngredient(ItemID.CobaltBar, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.FlareGun);
			recipe2.AddIngredient(ItemID.IllegalGunParts);
			recipe2.AddIngredient(ItemID.PalladiumBar, 18);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}