using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Ranged
{
	public class GerdSniper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-Blue Dart Rifle");
			Tooltip.SetDefault("'U got a problem mate?'\nHaving this decreases ranged weapon speed by 5% and increases damage by 10%\nUpon hitting an enemy the dart will split into 4 energy bolts\nUses darts as ammo");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 86;
			Item.height = 48;
			Item.rare = 4;
			Item.useTime = 20;
			Item.useAnimation = 25;
			Item.reuseDelay = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.sellPrice(silver: 120);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item40;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 45;
			Item.crit = 4;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueDart>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetAttackSpeed(DamageClass.Ranged) -= 0.05f;
			player.GetDamage(DamageClass.Ranged) += 0.10f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, -2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Seed || type == ProjectileID.PoisonDartBlowgun || type == ProjectileID.IchorDart || type == ProjectileID.CrystalDart || type == ProjectileID.CursedDart)
			{
				type = ModContent.ProjectileType<Projectiles.Ranged.UltraBlueDart>();
				SoundEngine.PlaySound(SoundID.Item40, player.position);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 25);
			recipe.AddIngredient(ItemID.AdamantiteBar, 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.HallowedBar, 12);
			recipe2.AddIngredient(ItemID.SoulofNight, 25);
			recipe2.AddIngredient(ItemID.TitaniumBar, 16);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();
		}
	}
}