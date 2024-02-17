using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Ranged
{
	public class InfraRedMinigun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Minigun");
			Tooltip.SetDefault($" Replaces most bullets with Infra-Red Bullets that inflict 'Partially Crystalized' on enemies");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 62;
			Item.height = 30;
			Item.rare = 5;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 85);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot2");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 34;
			Item.crit = 14;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 18);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity *= 2;
			position = position + 20 * Vector2.UnitY;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
			if (type == ProjectileID.Bullet || type == ProjectileID.MeteorShot || type == ProjectileID.CrystalBullet || type == ProjectileID.CursedBullet || type == ProjectileID.IchorBullet || type == ProjectileID.PartyBullet || type == ProjectileID.NanoBullet || type == ProjectileID.ExplosiveBullet || type == ProjectileID.GoldenBullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "InfraRedBar", 20);
			recipe.AddIngredient(ItemID.HallowedBar, 28);
			recipe.AddIngredient(ItemID.SoulofSight, 18);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}