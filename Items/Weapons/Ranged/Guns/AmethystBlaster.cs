using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged.Guns
{
	public class AmethystBlaster : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.width = 106;
			Item.height = 24;
			Item.rare = 5;
			Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 50);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41.WithPitchOffset(-0.4f);
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 30;
			Item.crit = 1;
			Item.knockBack = 12f;
			Item.noMelee = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 18f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -4);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position.Y = position.Y - 6f;
			if (type == ProjectileID.Bullet)
				type = ProjectileID.CrystalBullet;
			velocity *= 2f;
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
			damage = (int)(damage * (knockback / 12f));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmethystHandgun");
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddIngredient(ItemID.CrystalShard, 18);
			recipe.AddIngredient(null, "InfraRedCrystalShard", 7);
			recipe.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(null, "AmethystHandgun");
			recipe2.AddIngredient(ItemID.PalladiumBar, 10);
			recipe2.AddIngredient(ItemID.CrystalShard, 18);
			recipe2.AddIngredient(null, "InfraRedCrystalShard", 7);
			recipe2.AddIngredient(null, "BossUpgradeCrystal", 3);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}