using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AluminiumGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aluminium Gun");
			Tooltip.SetDefault($"'Compressed to burning hot'\n Has a chance to shoot a special bullet\nThe bullet can pierce 3 times, each hit with an enemy increases it's damage by 10");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 30;
			Item.rare = 1;
			Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 40);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 8;
			Item.crit = 10;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			SoundEngine.PlaySound(SoundID.Item11, player.position);
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
			type = Main.rand.Next(new int[] { type, ModContent.ProjectileType<Projectiles.Ranged.AluminiumRifleShot>()});
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBar, 12);
			recipe.AddIngredient(ItemID.Glass, 18);
			recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddIngredient(null, "UpgradeCrystal", 40);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.TinBar, 12);
			recipe2.AddIngredient(ItemID.Glass, 18);
			recipe2.AddIngredient(ItemID.FallenStar, 4);
			recipe2.AddIngredient(null, "UpgradeCrystal", 40);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}