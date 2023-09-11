using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class AmethystBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet Blaster");
			Tooltip.SetDefault("'Who thought a gem was a good material to make a gun out of?'\nReplaces normal bullets for crystal bullets");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 20;
			Item.rare = 4;
			Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 50);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.crit = 7;
			Item.knockBack = 2f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
				type = ProjectileID.CrystalBullet;
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
			SoundEngine.PlaySound(SoundID.Item11, player.position);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystHandgun");
			recipe.AddIngredient(ItemID.CobaltBar, 8);
			recipe.AddIngredient(ItemID.CrystalShard, 12);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AmethystHandgun");
			recipe2.AddIngredient(ItemID.PalladiumBar, 8);
			recipe2.AddIngredient(ItemID.CrystalShard, 12);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}