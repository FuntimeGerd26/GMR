using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class ChargeRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Rifle");
			Tooltip.SetDefault("Shoots a burst of 6 bullets, normal bullets move faster");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 80;
			Item.height = 30;
			Item.rare = 4;
			Item.useTime = 4;
            Item.useAnimation = 14;
			Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 125);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 42;
			Item.crit = 4;
			Item.knockBack = 1.5f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));
			for (int i = 0; i < 1; i++)
			{
				int p = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
				if (type == ProjectileID.Bullet)
					Main.projectile[p].extraUpdates += 2;
			}
			type = 0;
			SoundEngine.PlaySound(SoundID.Item11, player.Center);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle);
			recipe.AddIngredient(ItemID.SoulofLight, 14);
			recipe.AddIngredient(ItemID.MythrilBar, 8);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.ClockworkAssaultRifle);
			recipe2.AddIngredient(ItemID.SoulofLight, 14);
			recipe2.AddIngredient(ItemID.OrichalcumBar, 8);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}