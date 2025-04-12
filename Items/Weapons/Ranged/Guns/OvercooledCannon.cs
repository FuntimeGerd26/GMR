using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class OvercooledCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 94;
			Item.height = 42;
			Item.rare = 7;
			Item.useTime = 10;
            Item.useAnimation = 60;
			Item.reuseDelay = 65;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 160);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 75;
			Item.crit = 1;
			Item.knockBack = 14f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-46, -6);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;
			type = ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>();
			for (int i = 0; i < 2; i++)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, (int)(damage * 0.5f), knockback, player.whoAmI);
			}
			SoundEngine.PlaySound(SoundID.Item40, player.position);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "OvercooledRifle");
			recipe.AddIngredient(null, "AncientInfraRedPlating", 22);
			recipe.AddIngredient(null, "InfraRedBar", 18);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 22);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}