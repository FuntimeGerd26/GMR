using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
{
	public class Iceberg : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 50;
			Item.height = 50;
			Item.rare = 3;
			Item.value = Item.buyPrice(silver: 125);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true;
			Item.damage = 18;
			Item.knockBack = 5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.IcebergProj>();
			Item.shootSpeed = 18f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = 18;
				Item.useAnimation = 18;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.IcebergThrow>();
				return true;
			}
			else
			{
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.IcebergProj>();
			}

			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(Main.rand.NextFloat(8f, 12f));
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<Projectiles.Melee.IcebergThrow>(), (int)(damage * 0.65f), knockback, player.whoAmI);
				}
			}
			else
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Melee.Glaicy>(), damage, knockback, player.whoAmI);
				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IceBlock, 30);
			recipe.AddIngredient(ItemID.SnowBlock, 40);
			recipe.AddIngredient(ItemID.Bone, 45);
			recipe.AddRecipeGroup("GMR:AnyGem", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}