using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class FG20 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 106;
			Item.height = 50;
			Item.rare = 7;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 435);
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 40;
			Item.crit = 2;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 24f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-34, -8);
		}
		public override bool CanUseItem(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item41.WithPitchOffset(0.25f), player.Center);
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f));

			if (type == ProjectileID.Bullet)
			{
				velocity *= 3f;
				type = ModContent.ProjectileType<Projectiles.Ranged.MechBullet>();
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystBlaster");
			recipe.AddIngredient(ItemID.VenusMagnum);
			recipe.AddIngredient(3350); // Paintball Gun, WHY!? WHY IS IT'S ID NOT JUST "PaintballGun"!? WHAT EVEN IS IT'S ID THEN???
			recipe.AddIngredient(null, "InfraRedBar", 16);
			recipe.AddIngredient(ItemID.SoulofFright, 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}
	}
}