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
	public class AncientHandgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Fires Infra-Red Bullets that inflict 'Partially Crystalized' on enemies\nHas a chance to shoot homing bullets");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 26;
			Item.rare = 3;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.buyPrice(silver: 185);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot2");
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 20;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
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
			recipe.AddRecipeGroup("IronBar", 14); // Iron or Lead
			recipe.AddIngredient(ItemID.ShadowScale, 20);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "ScrapFragment", 20);
			recipe2.AddRecipeGroup("IronBar", 14); // Iron or Lead
			recipe2.AddIngredient(ItemID.TissueSample, 20);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}