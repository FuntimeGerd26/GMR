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
	public class JackGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Gun");
			Tooltip.SetDefault("Fires Infra-Red Bullets that inflict 'Partially Crystalized' on enemies\nHas a chance to shoot homing bullets");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 26;
			Item.rare = 3;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.buyPrice(silver: 285);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot2");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.crit = 4;
			Item.knockBack = 0.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
			type = Main.rand.NextBool(5) ? ModContent.ProjectileType<Projectiles.Ranged.AncientBulletHome>() : ModContent.ProjectileType<Projectiles.Ranged.JackBullet>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ScrapFragment", 20);
			recipe.AddRecipeGroup("IronBar", 10); // Iron or Lead
			recipe.AddRecipeGroup("GMR:AnyGem", 3);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}