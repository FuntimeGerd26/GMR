using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class VR15 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 78;
			Item.height = 26;
			Item.rare = 3;
			Item.useTime = 7;
            Item.useAnimation = 21;
			Item.reuseDelay = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 100);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 15;
			Item.crit = 1;
			Item.knockBack = 6f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));

			if (type == ProjectileID.Bullet)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.MechBullet>();
			}
			SoundEngine.PlaySound(SoundID.Item11, player.Center);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GoldBar, 14);
			recipe.AddIngredient(ItemID.FallenStar, 10);
			recipe.AddIngredient(null, "AncientInfraRedPlating", 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.PlatinumBar, 14);
			recipe2.AddIngredient(ItemID.FallenStar, 10);
			recipe2.AddIngredient(null, "AncientInfraRedPlating", 12);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 2);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}