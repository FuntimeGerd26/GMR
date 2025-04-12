using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class RefinedSpazSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 164;
			Item.height = 42;
			Item.rare = 9;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 435);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item40;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 80;
			Item.crit = 6;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 3f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-42, -6);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 10;
				Item.useAnimation = 30;
				Item.reuseDelay = 50;
				Item.shootSpeed = 12f;
				Item.UseSound = SoundID.Item61.WithVolumeScale(-1f);
				Item.useStyle = ItemUseStyleID.Swing;
				Item.noUseGraphic = true;
			}
			else
			{
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.reuseDelay = 0;
				Item.shootSpeed = 2f;
				Item.UseSound = SoundID.Item40;
				SoundEngine.PlaySound(SoundID.Item75.WithPitchOffset(0.25f), player.Center);
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noUseGraphic = false;
			}
			return true;
		}

		float randomRot;
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 12;

			if (player.altFunctionUse == 2)
			{
				SoundEngine.PlaySound(SoundID.Item7, player.Center);
				SoundEngine.PlaySound(SoundID.Item61.WithPitchOffset(-0.5f).WithVolumeScale(0.5f), player.Center);
				damage = damage * 2;
				type = ProjectileID.Grenade;
			}
			else
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.RefinedSpazmataniumBullet>();
				randomRot = Main.rand.NextFloat(-4f, 4f);

				for (int i = 0; i < 2; i++)
				{
					Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity.RotatedBy(MathHelper.ToRadians(randomRot)), type, (int)(damage * 0.8), knockback / 2, player.whoAmI);
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "RefinedSpazGun");
			recipe.AddIngredient(ItemID.ShroomiteBar, 20);
			recipe.AddIngredient(ItemID.BeetleHusk, 25);
			recipe.AddIngredient(ItemID.FragmentVortex, 8);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal", 2);
			recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
		}
	}
}