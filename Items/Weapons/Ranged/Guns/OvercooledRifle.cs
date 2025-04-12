using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class OvercooledRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(1);
		}

		public override void SetDefaults()
		{
			Item.width = 90;
			Item.height = 32;
			Item.rare = 5;
			Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 160);
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle($"{nameof(GMR)}/Sounds/Items/Ranged/BulletShot1") { Volume = 0.25f, };
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 45;
			Item.crit = 0;
			Item.knockBack = 8f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 8;
			type = ModContent.ProjectileType<Projectiles.Ranged.OvercooledBullet>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "ChargeRifle");
			recipe.AddIngredient(ItemID.SoulofNight, 18);
			recipe.AddIngredient(null, "InfraRedBar", 10);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.FrostCore);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}