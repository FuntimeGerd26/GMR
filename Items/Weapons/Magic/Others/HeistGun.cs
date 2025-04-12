using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Weapons.Magic.Others
{
	public class HeistGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(3);
		}

		public override void SetDefaults()
		{
			Item.width = 104;
			Item.height = 44;
			Item.rare = 5;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 225);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 40;
			Item.crit = 6;
			Item.knockBack = 0.75f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Magic.GreedBullet>();
			Item.shootSpeed = 18f;
			Item.mana = 4;
		}

		public override bool MagicPrefix()
		{
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-38, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 4;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LaserRifle);
			recipe.AddIngredient(ItemID.SpaceGun);
			recipe.AddIngredient(ItemID.HallowedBar, 18);
			recipe.AddIngredient(ItemID.GoldBar, 25);
			recipe.AddIngredient(ItemID.SoulofNight, 20);
			recipe.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.LaserRifle);
			recipe2.AddIngredient(ItemID.SpaceGun);
			recipe2.AddIngredient(ItemID.HallowedBar, 18);
			recipe2.AddIngredient(ItemID.PlatinumBar, 25);
			recipe2.AddIngredient(ItemID.SoulofNight, 20);
			recipe2.AddIngredient(null, "HardmodeUpgradeCrystal");
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}